using System.Collections.Generic;
using System;
using UnityEngine;

namespace Yours.QuickCity
{
    [Serializable]
    public struct TerrainDetectorProperty
    {
        [SerializeField]
        [Tooltip("探测器能够区分的最大距离. 比此值更远处的目标将不再被区分. \n[性能开销: 无影响]")]
        private float _maxDistanceStandard;
        internal readonly float MaxDistanceStandard
        {
            get
            {
                if (_maxDistanceStandard == 0)
                    Debug.LogWarning($"[Map] 检测到未初始化的属性 {nameof(_maxDistanceStandard)} .");
                return _maxDistanceStandard;
            }
        }

        [SerializeField]
        [Tooltip("探测器探测的方向数. 更高的值将增加地形图的生成精度. 程序会根据方向数量自动计算每个探测方向. \n[性能开销: 线性]")]
        private int _detectDirectionNum;
        internal readonly Vector3[] DetectDirections
        {
            get
            {
                if (_detectDirectionNum < 4)
                    Debug.LogWarning($"[Map] 探测器的方向数过少: {_detectDirectionNum}, 可能无法正确探测地形. 请确保其在4以上.");

                return DetectDirectionMgr.GetDirection(_detectDirectionNum);
            }
        }

        /*
         *  details
         */

        private readonly struct DetectDirectionMgr
        {
            private static readonly Dictionary<int, Vector3[]> _directions = new();
            internal static Vector3[] GetDirection(int dirNum)
            {
                if (_directions.ContainsKey(dirNum))
                    return _directions[dirNum];
                else
                {
                    var result = CalculateDirection(dirNum);
                    _directions.Add(dirNum, result);
                    return result;
                }
            }
            private static Vector3[] CalculateDirection(int dirNum)
            {
                float unitAngle = 360 / dirNum;
                List<Vector3> result = new();

                for (int i = 0; i < dirNum; i++)
                {
                    var rotation = Quaternion.AngleAxis(i * unitAngle, Vector3.up);
                    var dir = (rotation * Vector3.forward).normalized;

                    result.Add(dir);
                }
                return result.ToArray();
            }
        }
    }
}

namespace Yours.QuickCity
{
    public abstract class MapTerrainDetector : MonoBehaviour
    {
        /// <summary>
        /// 该探测器距最近的建筑物的物理距离
        /// </summary>
        public abstract float DensityValue { get; }
        /// <summary>
        /// 该探测器相对于最近的建筑物的贴附方向
        /// </summary>
        public abstract Vector3 AttachDirection { get; }

        public abstract Vector3 Position { get; }

        public abstract void Init(Vector3 position, float size, TerrainDetectorProperty property);
        public abstract void ExecuteDetect();
        public abstract void ShowDebugColor();
    }
}