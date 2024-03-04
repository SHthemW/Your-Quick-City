using System;
using System.Collections;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    internal sealed class MapBldgEntityGenerator : StepwiseTask
    {
        private readonly MapProperty _map;
        private readonly MapEntities _mapObjects;
        private readonly IMapObjParent _parent;

        private const Direction FLOOR_DEFAULT_DIRECTION = Direction.Up;

        internal MapBldgEntityGenerator(MapProperty basicProp, MapEntities entityProp, IMapObjParent parent, int maxTick) : base(maxTick)
        {
            _map = basicProp;
            _mapObjects = entityProp;
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
        }
        internal IEnumerator GenerateByDiagram(Matrix<MapNodeData> diagram)
        {
            yield return Foreach(iter: diagram.Content, body: node =>
            {
                if (_map.GenerateGround)
                    GenerateGroundTile(node.Coordinate);

                if (node.Data.HasContent)
                    GenerateObstacleTile(node.Coordinate, node.Data);
            });
        }

        private MapBldgEntityGenerator() : base(-1)
            => throw new NotImplementedException();
        private void GenerateGroundTile(Coord logicPos)
        {
            var spawnFloor = UnityEngine.Object.
                Instantiate(_mapObjects.GetRandomFloor());

            spawnFloor.transform.SetPositionAndRotation(
                position: MapUtils.GetTileActualPosition(_map.TileUnitSize, logicPos),
                rotation: FLOOR_DEFAULT_DIRECTION.ToRotation());

            spawnFloor.transform.SetParent(_parent.FloorObjParent); 
        }
        private void GenerateObstacleTile(Coord logicPos, MapNodeData data)
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
