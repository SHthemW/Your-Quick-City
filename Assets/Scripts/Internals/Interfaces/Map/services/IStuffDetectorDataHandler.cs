using System.Collections.Generic;

namespace Yours.QuickCity.Internal
{
    internal interface IStuffDetectorDataHandler
    {
        List<MapTerrainDetector> Detectors { get; }
    }
}
