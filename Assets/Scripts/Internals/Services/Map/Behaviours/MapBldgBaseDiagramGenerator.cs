using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Yours.QuickCity.Internal
{
    internal sealed class MapBldgBaseDiagramGenerator : StepwiseTask
    {
        private readonly MapProperty _map;
        private readonly MapEntities _mapObjects;

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

            int obstacleNumb = (int)(_map.ObstaclePercent * diagram.Content.Count());

            int count = 0;
            yield return ForStep(cond: count < obstacleNumb, inc: () => count++, stepcnt: obstacleNumb, body: () => 
            {
                if (randomCoords.Count == 0)
                    throw new BreakException();

                var currentCoord = randomCoords.Dequeue();

                if (diagram.JudgeIfCanPlaceObstacle(currentCoord))
                {
                    diagram[currentCoord.x, currentCoord.y].PlaceObstacle(_mapObjects.GetRandomBuilding());
                }
            });

            Queue<Coord> GenerateRandomCoords()
            {
                // storage all coordinates
                var allCoords = diagram.Content.Select(node => node.Coordinate);

                // get shuffed random queue.
                return new(MapUtils.ShuffleRandomly(allCoords.ToArray()));
            }
        }

        /*
         *  details:
         */

        private MapBldgBaseDiagramGenerator() : base(-1)
            => throw new System.NotImplementedException();
    }
}
