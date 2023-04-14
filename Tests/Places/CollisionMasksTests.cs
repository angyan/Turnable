using System.Collections.Immutable;
using System.Runtime.InteropServices;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Turnable.AI.Pathfinding;
using Turnable.Layouts;
using Turnable.Places;
using Turnable.Tiled;
using Turnable.TiledMap;

namespace Tests.Places;

public class CollisionMasksTests
{
    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(0, 1, 2)]
    internal void Collision_masks_can_be_constructed_with_at_least_one_other_layer_index(int layerIndex, params int[] maskLayerIndexes)
    {
        Map map = CreateMapWithThreeLayers();

        CollisionMasks collisionMasks = new(map, layerIndex, maskLayerIndexes);

        collisionMasks.Value.Should().BeEquivalentTo(maskLayerIndexes);
    }

    [Theory]
    [InlineData(0, -1)]
    [InlineData(0, 0)]
    [InlineData(0, 3)]
    [InlineData(1, 0, 2, 3)]
    internal void Collision_masks_cannot_be_constructed_with_invalid_layer_indices(int layerIndex, params int[] maskLayerIndexes)
    {
        Map map = CreateMapWithThreeLayers();

        Action construction = () => new CollisionMasks(map, layerIndex, maskLayerIndexes);;
        string readableArrayToString = string.Join(",",
            maskLayerIndexes.Select(maskLayerIndex => maskLayerIndex.ToString()).ToArray());

        construction.Should().Throw<ArgumentException>()
            .WithMessage($"[{readableArrayToString}] is not a valid value for the mask layer indexes; The indexes must be different from the layer index ({layerIndex}) and each value must be within 0 and {map.Layers.GetUpperBound(0)}");
    }

    private Map CreateMapWithThreeLayers()
    {
        Layer[] layers = new Layer[3];
        layers[0] = new(new[] { 0, 0, 0, 0 }, 2, 1, "Layer", 1, "layer", true, 2, 0, 0);
        layers[1] = new(new[] { 0, 0, 0, 0 }, 2, 1, "Layer", 1, "layer", true, 2, 0, 0);
        layers[2] = new(new[] { 0, 0, 0, 0 }, 2, 1, "Layer", 1, "layer", true, 2, 0, 0);
        Map map = new(-1, 16, false, layers, 1, 1, "orthogonal", "right-down", "1.9.2", 32, Array.Empty<Tileset>(), 32, "map",
            "1.9", 20);

        return map;
    }
}