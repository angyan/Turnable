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
        // Is the character being added at a location that is an obstacle?
        if (map.GetObstacles(collisionMasks).Contains(location))
        {
            throw new ArgumentException($"Character '{character.Name}' cannot be added at {location} because there is an obstacle there");
        }

        // Is the character being added twice at two different locations?
        if (Value.ContainsKey(character) && Value[character] != location) 
        {
            throw new ArgumentException($"Character '{character.Name}' cannot be added at {Value[character]} and then at {location}");
        }

        // Is the character being added at a location that is already occupied by another character?
        if (Value.ContainsValue(location) && !Value.ContainsKey(character))
        {
            throw new ArgumentException($"Character '{character.Name}' cannot be added at {location} because another character is already located at {location}");
        }

        // Is the character being added twice at the same location?
        if (Value.ContainsValue(location) && Value[character] == location)
        {
            throw new ArgumentException($"Character '{character.Name}' cannot be added twice at {location}");
        }
        
        return new CharacterLocations { Value = Value.Add(character, location) };
    }
};
