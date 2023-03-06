using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turnable.Layouts;
using Turnable.TiledMap;

namespace Turnable.Places;

public record CollisionMasks(IList<int> Value)
{
    internal CollisionMasks(Map map, int layerIndex, IList<int> maskLayerIndexes) : this(maskLayerIndexes)
    {
        if (!IsValid(map, layerIndex, maskLayerIndexes))
        {
            string readableArrayToString = string.Join(",",
                maskLayerIndexes.Select(maskLayerIndex => maskLayerIndex.ToString()).ToArray());

            throw new ArgumentException(
                $"[{readableArrayToString}] is not a valid value for the mask layer indexes; The indexes must be different from the layer index ({layerIndex}) and each value must be within 0 and {map.Layers.GetUpperBound(0)}");
        }
    }

    private static bool IsValid(Map map, int layerIndex, IList<int> maskLayerIndexes) => !maskLayerIndexes.Any(maskLayerIndex => maskLayerIndex == layerIndex || maskLayerIndex < 0 || maskLayerIndex > map.Layers.GetUpperBound(0));
}