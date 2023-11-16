using System;
using System.Collections.Generic;

namespace Game.General.Interfaces
{
    public interface ITowardDataServer
    {
        bool CharacterToward { get;}
        float SpeedInAxisX { get; }
    }
}
