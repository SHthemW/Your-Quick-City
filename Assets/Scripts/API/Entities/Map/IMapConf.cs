using System;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    public interface IMapConf
    {
        MapTerrainDetector TerrainDetector { get; }

        // debug options
        bool PrintMapGridDiagram         { get; }
        bool ShowStructureGenerateResult { get; }
        bool ShowStuffDistributionInfo   { get; }

        // tick
        int Tick { get; }
    }
}
