using UnityEngine;

namespace Yours.QuickCity.Internal
{
    internal interface IGameObject
    {
        GameObject gameObject { get; }
        Transform transform => gameObject.transform;
    }
}
