using System.Collections.Generic;

namespace Yours.QuickCity.Internal
{
    internal interface IStuffDetectorDataHandler
    {
        List<IMapTerrainDetector> Detectors { get; }
    }
}
