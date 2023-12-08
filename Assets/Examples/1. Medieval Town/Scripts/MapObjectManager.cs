using UnityEngine;

namespace Yours.QuickCity.Internal
{
    internal sealed class MapObjectManager : MonoBehaviour, IMapObjParent
    {
        [Header("Instance Conf")]

        [SerializeField]
        private Transform _floorObjParent;

        [SerializeField]
        private Transform _obstacleObjParent;

        [SerializeField]
        private Transform _terrainDetectorParent;

        [SerializeField]
        private Transform _stuffObjParent;

        /*
         *  Implements
         */

        Transform IMapObjParent.FloorObjParent => _floorObjParent;
        Transform IMapObjParent.ObstacleObjParent => _obstacleObjParent;
        Transform IMapObjParent.TerrainDetectorParent => _terrainDetectorParent;
        Transform IMapObjParent.StuffObjParent => _stuffObjParent;

    }
}
