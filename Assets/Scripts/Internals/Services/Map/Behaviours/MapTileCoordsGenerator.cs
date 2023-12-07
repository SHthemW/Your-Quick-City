using System.Collections.Generic;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    public sealed class MapTileCoordsGenerator
    {
        private readonly MapBasicProperty _basicProp;
        private readonly MapStuffGenerationProperty _stuffGenProp;

        private const float HANGING_HEIGHT = 1;

        public MapTileCoordsGenerator(MapBasicProperty basicProp, MapStuffGenerationProperty stuffProp)
        {
            _basicProp = basicProp;
            _stuffGenProp = stuffProp;
        }
        public Vector3[] GenerateCoords(MapDiagram map)
        {
            var offsets = GeneratePositionOffsets();
            List<Vector3> result = new();

            foreach (var node in map.Content)
            {
                if (node.IsObstacle)
                    continue;

                var tilePos = MapUtils.GetTileActualPosition(_basicProp.TileUnitSize, node.Coordinate);

                foreach (var offset in offsets)
                    result.Add(offset + tilePos);
            }

            return result.ToArray();
        }
        public void PrintDebugInfo()
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
            float unitDistance = (float)_basicProp.TileUnitSize / _stuffGenProp.StuffGenerateAccuracy;
            float halfDistance = unitDistance / 2;

            Vector3 origPoint = new(-_basicProp.TileUnitSize / 2, HANGING_HEIGHT, -_basicProp.TileUnitSize / 2);
            HashSet<Vector3> result = new();

            for (int x = 0; x < _stuffGenProp.StuffGenerateAccuracy; x++)
            {
                for (int y = 0; y < _stuffGenProp.StuffGenerateAccuracy; y++)
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
