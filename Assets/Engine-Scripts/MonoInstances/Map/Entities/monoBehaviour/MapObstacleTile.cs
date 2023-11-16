using UnityEngine;

namespace Game.Instances.Map.Entities
{
    internal sealed class MapObstacleTile : MapTileEntity
    {
        protected override Transform Parent => _controller.GetObstacleObjParent();

        private void Start()
        {
            if (GetComponent<Rigidbody>() == null)
                Debug.LogWarning($"[Map][Entity] 警告: 未在障碍物物体 {gameObject.name} 上检测到 {nameof(Rigidbody)} 组件, 物理效果和密度判定将失效.");
        }
    }
}
