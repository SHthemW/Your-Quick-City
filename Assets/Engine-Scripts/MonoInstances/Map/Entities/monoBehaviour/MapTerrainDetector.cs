using Game.Ctrller.Map;
using Game.General.Interfaces;
using Game.General.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Instances.Map.Entities
{
    [SelectionBase]
    internal sealed class MapTerrainDetector : MonoBehaviour, IMapTerrainDetector
    {
        private TerrainDetectorProperty? _property = null;
        private TerrainDetectorProperty Property
        {
            get
            {
                if (_property == null)
                    throw new ArgumentNullException(nameof(_property));
                return (TerrainDetectorProperty)_property;
            }
            set => _property = value;
        }

        private const float INFINITE = -1;
        private const float MAX_DIST = 999;

        private float? _closestBuilingDistance = null;
        /// <summary>
        /// 该探测器距最近的建筑物的物理距离
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// 在读取前必须对参数进行初始化.
        /// </exception>
        private float ClosestBuilingDistance
        {
            get => 
                _closestBuilingDistance != null ?
                (float)_closestBuilingDistance :
                throw new InvalidOperationException($"[Map][Detector] 探测器 {gameObject.name} 的探测数据还未初始化, 无法尝试读取它.");
            set => _closestBuilingDistance = value;
        }

        private Vector3? _closestAttachDirection = null;
        /// <summary>
        /// 该探测器相对于最近的建筑物的贴附方向
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// 在读取前必须对参数进行初始化.
        /// </exception>
        private Vector3 ClosestAttachDirection
        {
            get =>
                _closestAttachDirection != null ?
                (Vector3)_closestAttachDirection :
                throw new InvalidOperationException($"[Map][Detector] 探测器 {gameObject.name} 的探测数据还未初始化, 无法尝试读取它.");
            set => _closestAttachDirection = value;
        }

        private bool? _canGenerateStuffValidly = null;
        /// <summary>
        /// 当前探测器所处的位置是否能够实例化Stuff实体
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// 在读取前必须对参数进行初始化.
        /// </exception>
        private bool CanGenerateStuffValidly
        {
            get =>
                _canGenerateStuffValidly != null ?
                (bool)_canGenerateStuffValidly :
                throw new InvalidOperationException($"[Map][Detector] 探测器 {gameObject.name} 的生成数据还未初始化, 无法尝试读取它.");
            set => _canGenerateStuffValidly = value;
        }

        /*
         *  private
         */

        private Transform GetDebugger()
        {
            return transform.GetChild(0);
        }
        private float CastRayAndDetectDistance(Vector3 direction)
        {          
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, MAX_DIST))
                return hit.distance;
            else
                return INFINITE;
        }

        /*
         *  implements
         */

        bool IMapTerrainDetector.CanStuffGenerateValidly => this.CanGenerateStuffValidly;
        float IMapTerrainDetector.DensityValue => this.ClosestBuilingDistance;
        Vector3 IMapTerrainDetector.AttachDirection => this.ClosestAttachDirection;
        Vector3 IMapTerrainDetector.Position => transform.position;

        void IMapTerrainDetector.Init(Vector3 position, float size, TerrainDetectorProperty property)
        {
            transform.position = position;
            GetDebugger().localScale = new Vector3(size, size, size);
            Property = property;          
        }
        void IMapTerrainDetector.ExecuteDetect()
        {
            Dictionary<Vector3, float> dirDistPair = new();
            foreach (var dir in Property.DetectDirections)
            {
                dirDistPair.Add(dir, CastRayAndDetectDistance(dir));
            }

            var closestDis = dirDistPair.First().Value;
            foreach (var d in dirDistPair)
            {
                if (closestDis == INFINITE)
                    closestDis = d.Value;

                if (d.Value == INFINITE)
                    continue;

                closestDis = Mathf.Min(closestDis, d.Value);
            }

            // apply results
            this.ClosestBuilingDistance = closestDis;
            this.ClosestAttachDirection = dirDistPair.First(p => p.Value == closestDis).Key;
        }
        void IMapTerrainDetector.ShowDebugColor()
        {
            var percent = ClosestBuilingDistance / Property.MaxDistanceStandard;

            var mat = GetDebugger().GetComponent<MeshRenderer>().material;
            mat.color = MapUtils.GetDebugColor(percent);
        }
        void IMapTerrainDetector.RegisterToHandler(IStuffDetectorDataHandler handler)
        {
            handler.Detectors.Add(this);
        }
        IEnumerator IMapTerrainDetector.GenerateStuffAndDestorySelf(IStuff conf, Transform parent)
        {
            Destroy(gameObject);
            var obj = Instantiate(conf.Obj, parent);

            if (!obj.TryGetComponent(out IMapStuffEntity stuff))
                throw new NotImplementedException($"[Map][Stuff] 没有在地图Stuff物体 {obj.name} 上找到 {nameof(IMapStuffEntity)} 的实现.");

            this.CanGenerateStuffValidly = stuff.TryInit(conf, this);

            if (this.CanGenerateStuffValidly)
                yield return new WaitUntil(stuff.IsInited);
            else
                yield break;
        }
        
    }
}
