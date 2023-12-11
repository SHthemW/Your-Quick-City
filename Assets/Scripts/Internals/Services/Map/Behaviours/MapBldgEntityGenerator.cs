using System;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    internal sealed class MapBldgEntityGenerator
    {
        private readonly MapProperty _map;
        private readonly MapEntities _mapObjects;
        private readonly IMapObjParent _parent;

        private const Direction FLOOR_DEFAULT_DIRECTION = Direction.Up;

        private int _generatedCount;
        private int _targetGenerateCount;

        internal MapBldgEntityGenerator(MapProperty basicProp, MapEntities entityProp, IMapObjParent parent)
        {
            _map = basicProp;
            _mapObjects = entityProp;
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
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
            var spawnFloor = UnityEngine.Object.
                Instantiate(_mapObjects.GetRandomFloor());

            spawnFloor.transform.SetPositionAndRotation(
                position: MapUtils.GetTileActualPosition(_map.TileUnitSize, logicPos),
                rotation: FLOOR_DEFAULT_DIRECTION.ToRotation());

            spawnFloor.transform.SetParent(_parent.FloorObjParent); 
        }
        private void GenerateObstacleTile(Coord logicPos, MapDiagramNodeData data)
        {
            var spawnObstacle = UnityEngine.Object.
                Instantiate(data.NodeObj);

            spawnObstacle.transform.SetPositionAndRotation(
                position: MapUtils.GetTileActualPosition(_map.TileUnitSize, logicPos),
                rotation: data.Direction.ToRotation());

            spawnObstacle.transform.SetParent(_parent.ObstacleObjParent);

            if (!spawnObstacle.TryGetComponent<Rigidbody>(out _))
                Debug.LogWarning(
                    $"[Map] warning: we cannot get {nameof(Rigidbody)} component " +
                    $"in obstacle {spawnObstacle.name}, which means the terrain " +
                    $"analyzer will not work.");
        }
    }
}
