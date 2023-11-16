using Game.General.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Instances.Map
{
    internal sealed class MapEntityHandler : MonoBehaviour, IMapHandler
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

        Transform IMapHandler.GetFloorObjParent() => _floorObjParent;
        Transform IMapHandler.GetObstacleObjParent() => _obstacleObjParent;
        Transform IMapHandler.GetStuffDetectorParent() => _stuffDetectorParent;
        Transform IMapHandler.GetStuffObjParent() => _stuffObjParent;


    }
}
