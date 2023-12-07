using System.Collections.Generic;

namespace Yours.QuickCity.Internal
{
    public interface IStuffDetectorDataHandler
    {
        List<IMapTerrainDetector> Detectors { get; }
    }
}
