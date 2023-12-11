using System;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    [CreateAssetMenu(fileName = "New Map Config", menuName = "Config/Map")]
    public sealed class MapConf_SO : ScriptableObject, IMapConf
    {
        [SerializeField]
        private GameObject _terrainDetector;

        [Header("Debug")]

        [SerializeField] 
        private bool _printMapGridDiagram;

        [SerializeField]
        private bool _showStructureGenerateResult;

        [SerializeField]
        private bool _showStuffDistributionInfo;

        /*
         *  implements
         */

        GameObject IMapConf.TerrainDetector => _terrainDetector;
        bool IMapConf.PrintMapGridDiagram => _printMapGridDiagram;
        bool IMapConf.ShowStructureGenerateResult => _showStructureGenerateResult;
        bool IMapConf.ShowStuffDistributionInfo => _showStuffDistributionInfo;
    }
}
