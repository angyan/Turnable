using Turnable.Layouts;

namespace Turnable.Characters;

internal static class CharacterManager
{
    internal static Stat Update(this Stat stat, int newValue)
    {
        if (newValue <= stat.Min)
        {
            stat.RaiseMinReached(newValue);
            return stat with { Value = stat.Min };
        }
        else if (newValue >= stat.Max)
        {
            stat.RaiseMaxReached(newValue);
            return stat with { Value = stat.Max };
        }
        else
        {
            return stat with { Value = newValue };
        }
    }
}
