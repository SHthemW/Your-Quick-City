using System;
using System.Collections.Generic;
using UnityEngine;
using Yours.QuickCity.Internal;

namespace Yours.QuickCity //TODO: replacement position of Game.General.Properties $$ Interfaces.
{
    [Serializable]
    internal struct MapBasicProperty
    {
        [SerializeField]
        private int _size_x;

        [SerializeField]
        private int _size_y;
       
        [SerializeField]
        private float _tileUnitSize;

        internal readonly int Size_X
        {
            get
            {
                if (_size_x == 0)
                    Debug.LogWarning($"[Map] 检测到未初始化的属性 {nameof(_size_x)} .");
                return _size_x;
            }
        }
        internal readonly int Size_Y
        {
            get
            {
                if (_size_y == 0)
                    Debug.LogWarning($"[Map] 检测到未初始化的属性 {nameof(_size_y)} .");
                return _size_y;
            }
        }
        internal readonly float TileUnitSize
        {
            get
            {
                if (_tileUnitSize == 0)
                    Debug.LogWarning($"[Map] 检测到未初始化的属性 {nameof(_tileUnitSize)} .");
                return _tileUnitSize;
            }
        }
        internal readonly int TotalNodeNum => Size_X * Size_Y;       
    }

    [Serializable]
    internal struct MapBaseGenerationProperty
    {
        [Header("Property")]

        [SerializeField]
        private float _obstaclePercent;

        [Header("Generate")]

        [SerializeField]
        private GameObject[] _floorObjs;

        [SerializeField]
        private GameObject[] _obstacleObjs;

        internal float ObstaclePercent => _obstaclePercent;
        internal GameObject GetRandomFloor()
        {
            return _floorObjs[UnityEngine.Random.Range(0, _floorObjs.Length)];
        }
        internal GameObject GetRandomObstacle()
        {
            return _obstacleObjs[UnityEngine.Random.Range(0, _obstacleObjs.Length)];
        }    
    }

    [Serializable]
    internal struct MapStructureGenerationProperty
    {
        [Header("Properties")]

        [SerializeField]
        private bool _enableStructureDebug;

        [Header("Generate")]

        [SerializeField]
        private List<StructureData_SO> _structureList;

        internal List<IStructure> StructureList => _structureList.ConvertAll(new Converter<StructureData_SO, IStructure>(i => i));
        internal bool EnableStructureDebug => _enableStructureDebug;
    }

    [Serializable]
    internal struct MapStuffGenerationProperty
    {
        [Header("Property")]

        [SerializeField]
        [Tooltip("决定Stuff生成的地形图的分辨率, 不建议大于10. \n[性能开销: 指数]")]
        private int _stuffGenerateAccuracy;
        internal int StuffGenerateAccuracy
        {
            get
            {
                if (_stuffGenerateAccuracy == 0)
                    Debug.LogWarning($"[Map] 检测到未初始化的属性 {nameof(_stuffGenerateAccuracy)} .");
                return _stuffGenerateAccuracy;
            }
        } // TODO: change to "terrain detector resolution" and move to basic prop.

        [SerializeField]
        private float _stuffDistributeDiagramResolution;
        internal readonly float StuffDistributeDiagramResolution
            => _stuffDistributeDiagramResolution;

        [Space, SerializeField]
        [Tooltip("地形探测器设置. 本设置将决定与地形图生成相关的属性.")]
        private TerrainDetectorProperty _detectorSettings;
        internal TerrainDetectorProperty DetectorSettings => _detectorSettings;

        [Header("Generate")]

        [SerializeField]
        private List<StuffData_SO> stuffs;
        internal List<IStuff> Stuffs => stuffs.ConvertAll(new Converter<StuffData_SO, IStuff>(s => s));
    }
}

namespace Yours.QuickCity
{
    internal interface IMap
    {
        MapBasicProperty BasicProperty { get; }
        MapBaseGenerationProperty BaseGenerationProperty { get; }
        MapStructureGenerationProperty StructureGenerationProperty { get; }
        MapStuffGenerationProperty StuffGenerationProperty { get; }
    }
}
