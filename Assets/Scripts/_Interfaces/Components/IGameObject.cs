using System;
using UnityEngine;

namespace Game.General.Interfaces
{
    public interface IGameObject
    {
        GameObject gameObject { get; }
        Transform transform => gameObject.transform;
    }
}
