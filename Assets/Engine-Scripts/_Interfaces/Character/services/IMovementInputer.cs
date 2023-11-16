using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.General.Interfaces
{
    public interface IMovementInputer
    {
        Vector2 GetInputDirection();
    }
}
