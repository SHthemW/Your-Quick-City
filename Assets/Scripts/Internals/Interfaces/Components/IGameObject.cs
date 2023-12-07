using UnityEngine;

namespace Yours.QuickCity.Internal
{
    public interface IGameObject
    {
        GameObject gameObject { get; }
        Transform transform => gameObject.transform;
    }
}
