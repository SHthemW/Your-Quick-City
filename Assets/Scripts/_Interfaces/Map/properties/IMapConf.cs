using Game.General.Properties;
using System;
using UnityEngine;

namespace Game.General.Properties
{
    [Serializable]
    public struct MapUtilObjectConf 
    {
        [SerializeField]
        private GameObject _terrainDetector;
        public GameObject TerrainDetector
            => _terrainDetector != null ? _terrainDetector : throw new ArgumentNullException(nameof(_terrainDetector));
    }
}

namespace Game.General.Interfaces
{
    public interface IMapConf
    {
        MapUtilObjectConf UtilObjectConf { get; }
    }
}
