using UnityEngine;
using System;

namespace Yours.QuickCity.Internal
{
    [CreateAssetMenu(fileName = "New Stuff", menuName = "Map/Stuff/New")]
    internal sealed class StuffData_SO : ScriptableObject, IStuff
    {
        [Header("Basic")]

        [SerializeField, Tooltip("Stuff的Prefeb.")]
        private GameObject _obj;

        [Header("General Options")]

        [SerializeField, Tooltip("该Stuff的最大生成数量")]
        private int _maxGenerateNum;

        [Space]

        [SerializeField, Tooltip("可生成区域的范围: 左值")]
        private float _minGenerateDensity;

        [SerializeField, Tooltip("可生成区域的范围: 右值")]
        private float _maxGenerateDensity;

        [Header("Object Options")]

        [SerializeField, Tooltip(
            "物体的生成方向受其贴附物影响的程度. \n\n" +
            "None - 不开启方向性生成, \n物体的旋转角度为默认值 \n\n" +
            "Part - 仅开启东/西/南/北朝向的方向性生成, \n物体方向呈现与坐标轴平行的放射状效果. \n\n" +
            "Full - 开启所有方向的方向性生成, \n物体方向呈现放射状效果.")]
        private DirectionalGenerateType _directionalGenerate;

        [SerializeField, Tooltip("物体生成后的最小初始旋转角度 (在边界间随机取值).")]
        private float _minRotationOffset;

        [SerializeField, Tooltip("物体生成后的最大初始旋转角度 (在边界间随机取值).")]
        private float _maxRotationOffset;

        [Space]

        [SerializeField, Tooltip("两Obj间的最小间距 (在边界间随机取值).")]
        private float _minGenerateSpacing;

        [SerializeField, Tooltip("两Obj间的最大间距 (在边界间随机取值).")]
        private float _maxGenerateSpacing;
       
        private void OnValidate()
        {
            if (_minGenerateDensity > _maxGenerateDensity)
                Debug.LogWarning($"[Map][Stuff] 警告: Stuff {name} 的配置数值出错: 最小生成密度不应大于其最大生成密度.");

            if (_minRotationOffset > _maxRotationOffset)
                Debug.LogWarning($"[Map][Stuff] 警告: Stuff {name} 的配置数值出错: 最小旋转角度不应大于最大旋转角度.");

            if (_minGenerateSpacing > _maxGenerateSpacing)
                Debug.LogWarning($"[Map][Stuff] 警告: Stuff {name} 的配置数值出错: 最小间距不应大于最大间距.");
        }

        /*
         *  implements
         */

        GameObject IStuff.Obj => 
            _obj != null ? _obj : throw new ArgumentNullException($"[Map][Stuff] {name} 的参数 {nameof(_obj)} 未初始化.");

        int IStuff.MaxGenerateNum => _maxGenerateNum;      
        float IStuff.MinGenerateDensity => _minGenerateDensity;
        float IStuff.MaxGenerateDensity => _maxGenerateDensity;

        DirectionalGenerateType IStuff.DirectionalGenerate => _directionalGenerate;
        float IStuff.GetRotationOffset()
        {
            return UnityEngine.Random.Range(_minRotationOffset, _maxRotationOffset);
        }
        float IStuff.GetGenerateSpacing()
        {
            return UnityEngine.Random.Range(_minGenerateSpacing, _maxGenerateSpacing);
        }       
    }

    internal enum DirectionalGenerateType
    {
        None = 0, Part, Full
    }
}
