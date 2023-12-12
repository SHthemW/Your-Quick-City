using System.Collections;
using System.Collections.Generic;

namespace Yours.QuickCity.Internal
{
    internal sealed class MapBldgBaseDiagramGenerator : StepwiseTask
    {
        private readonly MapProperty _map;
        private readonly MapEntities _mapObjects;

        private int _obstacleNum => (int)(_map.TotalNodeNum * _map.ObstaclePercent);
        private List<Coord> _allTileCoords { get; set; } = new();

        /*
         *  internal:
         */

        internal MapBldgBaseDiagramGenerator(MapProperty basicProperty, MapEntities entityProperty, int maxTick) : base(maxTick)
        {
            _map  = basicProperty;
            _mapObjects = entityProperty;
        }
        internal IEnumerator GenerateOnDiagram(MapDiagram diagram)
        {
            var randomCoords = GenerateRandomCoords();

            int count = 0;
            yield return ForStep(cond: count < _obstacleNum, inc: () => count++, stepcnt: _obstacleNum, body: () => 
            {
                if (randomCoords.Count == 0)
                    throw new BreakException();

                var currentCoord = randomCoords.Dequeue();

                if (diagram.JudgeIfCanPlaceObstacle(currentCoord))
                {
                    diagram[currentCoord.x, currentCoord.y].PlaceObstacle(_mapObjects.GetRandomBuilding());
                }
            });
        }

        /*
         *  details:
         */

        private MapBldgBaseDiagramGenerator() : base(-1)
            => throw new System.NotImplementedException();
        private Queue<Coord> GenerateRandomCoords()
        {
            // storage all coordinates
            for (int i = 0; i < _map.Size_X; i++)
                for (int j = 0; j < _map.Size_Y; j++)
                    _allTileCoords.Add(new Coord(i, j));

            // get shuffed random queue.
            return new(MapUtils.ShuffleRandomly(_allTileCoords.ToArray()));
        }
    }
}
