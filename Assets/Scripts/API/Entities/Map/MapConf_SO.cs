using System;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    [CreateAssetMenu(fileName = "New Map Config", menuName = "Map/Config")]
    public sealed class MapConf_SO : ScriptableObject, IMapConf
    {
        [SerializeField]
        private MapTerrainDetector _terrainDetector;

        [SerializeField]
        private int _tick = 1000;

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

        MapTerrainDetector IMapConf.TerrainDetector => _terrainDetector;
        bool IMapConf.PrintMapGridDiagram => _printMapGridDiagram;
        bool IMapConf.ShowStructureGenerateResult => _showStructureGenerateResult;
        bool IMapConf.ShowStuffDistributionInfo => _showStuffDistributionInfo;
        int IMapConf.Tick => _tick;
    }
}
