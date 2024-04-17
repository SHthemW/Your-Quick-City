using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    internal sealed class MapTileCoordsGenerator : StepwiseTask<List<Vector3>>
    {
        private readonly MapProperty _map;

        private const float HANGING_HEIGHT = 1;

        internal MapTileCoordsGenerator(MapProperty basicProp, int maxTick) : base(maxTick)
        {
            _map = basicProp;
        }
        internal IEnumerator GenerateCoords(Matrix<MapNodeData> map)
        {           
            Result = new();

            var offsets = GeneratePositionOffsets();

            yield return Foreach(iter: map.Content, body: node => 
            {
                if (node.Data.HasContent && _map.IgnoreBuildingAreasWhenAnalysis)
                    throw new ContinueException();

                var tilePos = MapUtils.GetTileActualPosition(_map.TileUnitSize, node.Coordinate);

                foreach (var offset in offsets)
                    Result.Add(offset + tilePos);
            });
        }
        internal void PrintDebugInfo()
        {
            string offsetMsg = "";

            foreach (var p in GeneratePositionOffsets())
                offsetMsg += p + "\n";

            Debug.Log(
                "[Debug][Map Stuff] \n " +
                "offset coords in each: \n" + 
                offsetMsg);
        }

        private MapTileCoordsGenerator() : base(-1)
            => throw new System.InvalidOperationException();
        private HashSet<Vector3> GeneratePositionOffsets()
        {
            float unitDistance = (float)_map.TileUnitSize / _map.TerrainDetectResolution;
            float halfDistance = unitDistance / 2;

            Vector3 origPoint = new(-_map.TileUnitSize / 2, HANGING_HEIGHT, -_map.TileUnitSize / 2);
            HashSet<Vector3> result = new();

            for (int x = 0; x < _map.TerrainDetectResolution; x++)
            {
                for (int y = 0; y < _map.TerrainDetectResolution; y++)
                {
                    Vector3 offsetPos = origPoint + new Vector3(
                        halfDistance + (x * unitDistance),
                        0,
                        halfDistance + (y * unitDistance)
                        );

                    result.Add(offsetPos);
                }
            }
            return result;
        }
    }
}
