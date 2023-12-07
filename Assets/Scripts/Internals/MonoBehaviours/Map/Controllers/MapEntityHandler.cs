using UnityEngine;

namespace Yours.QuickCity.Internal
{
    internal sealed class MapEntityHandler : MonoBehaviour, IMapObjParent
    {
        [Header("Instance Conf")]

        [SerializeField]
        private Transform _floorObjParent;

        [SerializeField]
        private Transform _obstacleObjParent;

        [SerializeField]
        private Transform _stuffDetectorParent;

        [SerializeField]
        private Transform _stuffObjParent;

        /*
         *  Implements
         */

        Transform IMapObjParent.GetFloorObjParent() => _floorObjParent;
        Transform IMapObjParent.GetObstacleObjParent() => _obstacleObjParent;
        Transform IMapObjParent.GetStuffDetectorParent() => _stuffDetectorParent;
        Transform IMapObjParent.GetStuffObjParent() => _stuffObjParent;


    }
}
