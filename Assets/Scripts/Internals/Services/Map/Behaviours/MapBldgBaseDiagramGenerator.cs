using System.Collections.Generic;

namespace Yours.QuickCity.Internal
{
    internal sealed class MapBldgBaseDiagramGenerator
    {
        private readonly MapBasicProperty _basicProperty;
        private readonly MapBaseGenerationProperty _baseGenProperty;

        private int _obstacleNum => (int)(_basicProperty.TotalNodeNum * _baseGenProperty.ObstaclePercent);
        private List<Coord> _allTileCoords { get; set; } = new();

        /*
         *  internal:
         */

        internal MapBldgBaseDiagramGenerator(MapBasicProperty basicProperty, MapBaseGenerationProperty entityProperty)
        {
            _basicProperty  = basicProperty;
            _baseGenProperty = entityProperty;
        }
        internal void GenerateOnDiagram(MapDiagram diagram)
        {
            var randomCoords = GenerateRandomCoords();

            for (int i = 0; i < _obstacleNum; i++)
            {
                var currentCoord = randomCoords.Dequeue();

                if (diagram.JudgeIfCanPlaceObstacle(currentCoord))
                {
                    diagram[currentCoord.x, currentCoord.y].PlaceObstacle(_baseGenProperty.GetRandomObstacle());
                }
            }
        }

        /*
         *  details:
         */

        private MapBldgBaseDiagramGenerator() { }
        private Queue<Coord> GenerateRandomCoords()
        {
            // storage all coordinates
            for (int i = 0; i < _basicProperty.Size_X; i++)
                for (int j = 0; j < _basicProperty.Size_Y; j++)
                    _allTileCoords.Add(new Coord(i, j));

            // get shuffed random queue.
            return new(MapUtils.ShuffleRandomly(_allTileCoords.ToArray()));
        }
    }
}
