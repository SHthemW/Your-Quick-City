using System;

namespace Yours.QuickCity.Internal
{
    internal sealed class MapBldgEntityGenerator
    {
        private readonly MapBasicProperty _basicProperty;
        private readonly MapBaseGenerationProperty _baseGenProperty;
        private readonly IMapHandler _controller;

        private const Direction FLOOR_DEFAULT_DIRECTION = Direction.Up;

        private int _generatedCount;
        private int _targetGenerateCount;

        internal MapBldgEntityGenerator(MapBasicProperty basicProp, MapBaseGenerationProperty entityProp, IMapHandler controller)
        {
            _basicProperty = basicProp;
            _baseGenProperty = entityProp;
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
        }
        internal void GenerateByDiagram(MapDiagram diagram)
        {
            _targetGenerateCount = diagram.TotalNodeNum;

            foreach (var node in diagram.Content)
            {
                GenerateGroundTile(node.Coordinate);

                if (node.IsObstacle)
                    GenerateObstacleTile(node.Coordinate, node.NodeData);

                _generatedCount++;
            }
        }
        internal bool GenerateIsFinished()
        {
            return _generatedCount >= _targetGenerateCount;
        }

        private MapBldgEntityGenerator() { }
        private void GenerateGroundTile(Coord logicPos)
        {
            var spawnTile =
                        UnityEngine.Object.Instantiate(_baseGenProperty.GetRandomFloor()).
                        GetComponent<IMapTileEntity>();

            spawnTile.Init(new TileProperty(
                MapUtils.GetTileActualPosition(_basicProperty.TileUnitSize, logicPos), 
                FLOOR_DEFAULT_DIRECTION), 
                _controller);
        }
        private void GenerateObstacleTile(Coord logicPos, MapDiagramNodeData data)
        {
            var spawnObs =
                    UnityEngine.Object.Instantiate(data.NodeObj).
                    GetComponent<IMapTileEntity>();

            spawnObs.Init(new TileProperty(
                MapUtils.GetTileActualPosition(_basicProperty.TileUnitSize, logicPos), 
                data.Direction), 
                _controller);
        }       
    }
}
