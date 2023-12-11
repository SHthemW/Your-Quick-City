using Codice.Client.BaseCommands;
using System.Collections.Generic;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    internal sealed class MapTileCoordsGenerator
    {
        private readonly MapProperty _map;

        private const float HANGING_HEIGHT = 1;

        internal MapTileCoordsGenerator(MapProperty basicProp)
        {
            _map = basicProp;
        }
        internal Vector3[] GenerateCoords(MapDiagram map)
        {
            var offsets = GeneratePositionOffsets();
            List<Vector3> result = new();

            foreach (var node in map.Content)
            {
                if (node.IsObstacle && _map.IgnoreBuildingAreasWhenAnalysis)
                    continue;

                var tilePos = MapUtils.GetTileActualPosition(_map.TileUnitSize, node.Coordinate);

                foreach (var offset in offsets)
                    result.Add(offset + tilePos);
            }

            return result.ToArray();
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

        private MapTileCoordsGenerator() { }
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
