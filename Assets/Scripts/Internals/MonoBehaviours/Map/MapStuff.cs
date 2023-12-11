using UnityEngine;

namespace Yours.QuickCity.Internal
{
    [RequireComponent(typeof(Collider))]
    internal sealed class MapStuff : MonoBehaviour, IMapStuffEntity
    {
        private bool _isInited = false;

        private void CheckInitalValue()
        {
            if (transform.position.x != 0 || transform.position.z != 0)
                Debug.LogWarning($"[Stuff] 警告: 物体 {gameObject.name} 的 {nameof(transform.position)} 的初始值可能在初始化时被覆盖.");

            if (transform.rotation.eulerAngles.y != 0)
                Debug.LogWarning($"[Stuff] 警告: 物体 {gameObject.name} 的 {nameof(transform.rotation)} 的初始值可能在初始化时被覆盖.");
        }

        bool IMapStuffEntity.TryInit(IStuff conf, MapTerrainDetector data)
        {
            CheckInitalValue();

            // set position
            transform.position = new(data.Position.x, transform.position.y, data.Position.z);
          
            // check if can generate
            foreach (var n in Physics.OverlapSphere(transform.position, conf.GetGenerateSpacing()))
            {
                if (n.gameObject != gameObject && n.TryGetComponent(out IMapStuffEntity _))
                {
                    Destroy(gameObject);
                    return false;
                }
            }
            _isInited = true;

            // set rotation           
            // transform.rotation = conf.GetGenerateDirection(data, transform.rotation.eulerAngles);

            return true;
        }
        bool IMapStuffEntity.IsInited()
        {
            return _isInited;
        }
    }
}
