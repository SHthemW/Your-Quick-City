using System;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    [Serializable]
    internal struct MapUtilObjectConf 
    {
        [SerializeField]
        private GameObject _terrainDetector;
        internal GameObject TerrainDetector
            => _terrainDetector != null ? _terrainDetector : throw new ArgumentNullException(nameof(_terrainDetector));
    }
}

namespace Yours.QuickCity.Internal
{
    internal interface IMapConf
    {
        MapUtilObjectConf UtilObjectConf { get; }
    }
}
