using System;
using System.Collections.Generic;

namespace Game.General.Interfaces
{
    public interface IStuffDetectorDataHandler
    {
        List<IMapTerrainDetector> Detectors { get; }
    }
}
