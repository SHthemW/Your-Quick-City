using UnityEngine;

namespace Yours.QuickCity.Internal
{
    internal sealed class CircleMotion : MonoBehaviour
    {
        [SerializeField]
        private string _targetName;

        [SerializeField]
        private float _radius;

        [SerializeField]
        private float _speed;

        [SerializeField]
        private float _heightMin; // 最小高度

        [SerializeField]
        private float _heightMax; // 最大高度

        [SerializeField]
        private float _heightFrequency; // 高度频率

        private Transform _target;
        private float _angle;

        private void Awake()
        {
            _target = GameObject.Find(_targetName).transform;
        }

        private void Update()
        {
            _angle += _speed * Time.deltaTime; // 计算角度

            Vector3 offset = new Vector3(Mathf.Sin(_angle), 0, Mathf.Cos(_angle)) * _radius; // 计算偏移量
            float heightMid = (_heightMax + _heightMin) / 2;
            float heightAmplitude = (_heightMax - _heightMin) / 2;
            offset.y = heightMid + heightAmplitude * Mathf.Sin(_heightFrequency * Time.time); // 设置高度
            transform.position = _target.position + offset; // 更新位置

            transform.LookAt(_target.position); // 朝向圆心
        }
    }
}
