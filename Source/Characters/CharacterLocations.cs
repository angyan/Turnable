using System.Collections.Immutable;
using Turnable.Layouts;
using Turnable.Places;
using Turnable.TiledMap;

namespace Turnable.Characters;

internal record CharacterLocations(ImmutableDictionary<Character, Location> Value)
{
    internal CharacterLocations() : this(ImmutableDictionary<Character, Location>.Empty)
    {
    }

    internal CharacterLocations AddOnMap(Character character, Location location, Map map, CollisionMasks collisionMasks)
    {
        if (map.GetObstacles(collisionMasks).Contains(location))
        {
            throw new ArgumentException();
        }

        if (Value.ContainsValue(location))
        {
            throw new ArgumentException();
        }
        
        return new CharacterLocations { Value = Value.Add(character, location) };
    }
};
