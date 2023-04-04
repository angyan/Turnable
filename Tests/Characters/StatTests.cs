using System.Collections.Immutable;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Turnable.AI.Pathfinding;
using Turnable.Characters;
using Turnable.Layouts;
using Turnable.Places;
using Turnable.Tiled;
using Turnable.TiledMap;

namespace Tests.Characters;

public class StatTests
{
    [Theory]
    [InlineData(10)]
    [InlineData(0)]
    [InlineData(-100)]
    internal void Values_are_clamped_to_a_minimum(int value)
    {
        Stat sut = new(50, 10, 100);

        Stat newStat = sut.Update(value);

        newStat.Value.Should().Be(10);
    }

    [Theory]
    [InlineData(100)]
    [InlineData(110)]
    internal void Values_are_clamped_to_a_maximum(int value)
    {
        Stat sut = new(50, 10, 100);

        Stat newStat = sut.Update(value);

        newStat.Value.Should().Be(100);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(110)]
    internal void A_stat_cannot_be_constructed_with_a_value_outside_its_clamps(int value)
    {
        // No arrange

        Action construction = () => new Stat(value, 10, 100);

        construction.Should().Throw<ArgumentException>()
            .WithMessage($"{value} is not a valid value for a Stat that has a Min of 10 and a Max of 100");
    }

    [Fact]
    internal void An_event_is_raised_when_the_value_is_clamped_to_the_minimum()
    {
        Stat sut = new(10, 10, 100);
        using var monitoredSut = sut.Monitor();

        Stat newStat = sut.Update(5);

        monitoredSut.Should().Raise(nameof(Stat.MinReached)).WithSender(sut)
            .WithArgs<StatClampedArgs>(args => args.TriedValue == 5);
    }

    [Fact]
    internal void An_event_is_raised_when_the_value_is_clamped_to_the_maximum()
    {
        Stat sut = new(10, 10, 100);
        using var monitoredSut = sut.Monitor();

        Stat newStat = sut.Update(110);

        monitoredSut.Should().Raise(nameof(Stat.MaxReached)).WithSender(sut)
            .WithArgs<StatClampedArgs>(args => args.TriedValue == 110);
    }

    [Fact]
    internal void An_event_is_raised_when_the_value_is_updated()
    {
        Stat sut = new(10, 10, 100);
        using var monitoredSut = sut.Monitor();

        Stat newStat = sut.Update(50);

        monitoredSut.Should().Raise(nameof(Stat.ValueUpdated)).WithSender(sut)
            .WithArgs<StatUpdatedArgs>(args => args.NewValue== 50);
    }

    [Fact]
    internal void An_event_is_not_raised_when_the_value_is_not_clamped()
    {
        Stat sut = new(10, 10, 100);
        using var monitoredSut = sut.Monitor();

        Stat newStat = sut.Update(50);

        monitoredSut.Should().NotRaise(nameof(Stat.MinReached));
        monitoredSut.Should().NotRaise(nameof(Stat.MaxReached));
    }

    [Fact]
    internal void All_subscriptions_to_events_are_kept_when_a_stat_is_updated()
    {
        int subscriber1CallCount = 0;
        void Subscriber(object sender, StatClampedArgs args)
        {
            subscriber1CallCount++;
        }
        Stat sut = new(10, 10, 100);
        sut.MinReached += Subscriber;

        Stat newStat = sut.Update(0);
        Stat _ = newStat.Update(0);

        subscriber1CallCount.Should().Be(2);
    }
}