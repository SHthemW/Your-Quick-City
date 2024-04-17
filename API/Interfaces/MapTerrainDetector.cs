using System.Collections.Generic;
using System;
using UnityEngine;

namespace Yours.QuickCity
{
    [Serializable]
    public struct TerrainDetectorProperty
    {
        [SerializeField]
        [Tooltip("̽�����ܹ����ֵ�������. �ȴ�ֵ��Զ����Ŀ�꽫���ٱ�����. \n[���ܿ���: ��Ӱ��]")]
        private float _maxDistanceStandard;
        internal readonly float MaxDistanceStandard
        {
            get
            {
                if (_maxDistanceStandard == 0)
                    Debug.LogWarning($"[Map] ��⵽δ��ʼ�������� {nameof(_maxDistanceStandard)} .");
                return _maxDistanceStandard;
            }
        }

        [SerializeField]
        [Tooltip("̽����̽��ķ�����. ���ߵ�ֵ�����ӵ���ͼ�����ɾ���. �������ݷ��������Զ�����ÿ��̽�ⷽ��. \n[���ܿ���: ����]")]
        private int _detectDirectionNum;
        internal readonly Vector3[] DetectDirections
        {
            get
            {
                if (_detectDirectionNum < 4)
                    Debug.LogWarning($"[Map] ̽�����ķ���������: {_detectDirectionNum}, �����޷���ȷ̽�����. ��ȷ������4����.");

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
        /// ��̽����������Ľ�������������
        /// </summary>
        public abstract float DensityValue { get; }
        /// <summary>
        /// ��̽�������������Ľ��������������
        /// </summary>
        public abstract Vector3 AttachDirection { get; }

        public abstract Vector3 Position { get; }

        public abstract void Init(Vector3 position, float size, TerrainDetectorProperty property);
        public abstract void ExecuteDetect();
        public abstract void ShowDebugColor();
    }
}