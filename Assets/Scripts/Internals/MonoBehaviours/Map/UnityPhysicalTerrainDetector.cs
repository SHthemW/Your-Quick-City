using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    [SelectionBase, Serializable]
    internal sealed class UnityPhysicalTerrainDetector : MapTerrainDetector
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

        [SerializeField]
        private float _closestBuilingDistance = -1;
        /// <summary>
        /// 该探测器距最近的建筑物的物理距离
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// 在读取前必须对参数进行初始化.
        /// </exception>
        private float ClosestBuilingDistance
        {
            get => 
                _closestBuilingDistance != -1 ?
                (float)_closestBuilingDistance :
                throw new InvalidOperationException($"[Map][Detector] 探测器 {gameObject.name} 的探测数据还未初始化, 无法尝试读取它.");
            set => _closestBuilingDistance = value;
        }

        [SerializeField]
        private Vector3 _closestAttachDirection = default;
        /// <summary>
        /// 该探测器相对于最近的建筑物的贴附方向
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// 在读取前必须对参数进行初始化.
        /// </exception>
        private Vector3 ClosestAttachDirection
        {
            get =>
                _closestAttachDirection != default ?
                (Vector3)_closestAttachDirection :
                throw new InvalidOperationException($"[Map][Detector] 探测器 {gameObject.name} 的探测数据还未初始化, 无法尝试读取它.");
            set => _closestAttachDirection = value;
        }

        private static readonly Vector3 CheckBoxSize = new(0.02f, 0.02f, 0.02f);

        /*
         *  private
         */

        private Transform GetDebugger()
        {
            return transform.GetChild(0);
        }
        private float CastRayAndDetectDistance(Vector3 direction)
        {
            // overlap
            if (Physics.CheckBox(
                center:      transform.position,
                halfExtents: CheckBoxSize, 
                orientation: Quaternion.identity))
                return 0;

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, MAX_DIST))
                return hit.distance;
            else
                return INFINITE;
        }

        /*
         *  implements
         */

        public override sealed float DensityValue => this.ClosestBuilingDistance;
        public override sealed Vector3 AttachDirection => this.ClosestAttachDirection;
        public override sealed Vector3 Position => transform.position;

        public override sealed void Init(Vector3 position, float size, TerrainDetectorProperty property)
        {
            transform.position = position;
            GetDebugger().localScale = new Vector3(size, size, size);
            Property = property;          
        }
        public override sealed void ExecuteDetect()
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
        public override sealed void ShowDebugColor()
        {
            var percent = ClosestBuilingDistance / Property.MaxDistanceStandard;

            var mat = GetDebugger().GetComponent<MeshRenderer>().material;
            mat.color = MapUtils.GetDebugColor(percent);
        }
    }
}
