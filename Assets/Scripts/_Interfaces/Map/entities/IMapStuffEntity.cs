using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.General.Interfaces
{
    public interface IMapStuffEntity
    {
        bool TryInit(IStuff conf, IMapTerrainDetector data);
        bool IsInited();
    }
}
