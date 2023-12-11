using System;
using System.Collections.Generic;
using UnityEngine;
using Yours.QuickCity.Internal;

namespace Yours.QuickCity
{
    [Serializable]
    public struct MapProperty
    {
        [Header("Size")]

        [SerializeField]
        private int _size_x;
        internal readonly int Size_X
        {
            get
            {
                if (_size_x == 0)
                    Debug.LogWarning($"[Map] 检测到未初始化的属性 {nameof(_size_x)} .");
                return _size_x;
            }
        }

        [SerializeField]
        private int _size_y;
        internal readonly int Size_Y
        {
            get
            {
                if (_size_y == 0)
                    Debug.LogWarning($"[Map] 检测到未初始化的属性 {nameof(_size_y)} .");
                return _size_y;
            }
        }

        [SerializeField]
        private float _tileUnitSize;
        internal readonly float TileUnitSize
        {
            get
            {
                if (_tileUnitSize == 0)
                    Debug.LogWarning($"[Map] 检测到未初始化的属性 {nameof(_tileUnitSize)} .");
                return _tileUnitSize;
            }
        }
        internal readonly int TotalNodeNum 
            => Size_X * Size_Y;

        [Header("Generate")]

        [SerializeField]
        private float _obstaclePercent;
        internal readonly float ObstaclePercent => _obstaclePercent;

        [Header("Analysis")]

        [SerializeField]
        [Tooltip("决定Stuff生成的地形图的分辨率, 不建议大于10. \n[性能开销: 指数]")]
        private int _terrainDetectResolution;
        internal readonly int TerrainDetectResolution
        {
            get
            {
                if (_terrainDetectResolution == 0)
                    Debug.LogWarning($"[Map] 检测到未初始化的属性 {nameof(_terrainDetectResolution)} .");
                return _terrainDetectResolution;
            }
        }

        [SerializeField]
        private float _stuffDistributeDiagramResolution;
        internal readonly float StuffDistributeDiagramResolution
        {
            get
            {
                if (_stuffDistributeDiagramResolution == 0)
                    Debug.LogWarning($"[Map] 检测到未初始化的属性 {nameof(_stuffDistributeDiagramResolution)} .");
                return _stuffDistributeDiagramResolution;
            }
        }

        [SerializeField]
        [Tooltip("地形探测器设置. 本设置将决定与地形图生成相关的属性.")]
        private TerrainDetectorProperty _detectorSettings;
        internal readonly TerrainDetectorProperty DetectorSettings 
            => _detectorSettings;
    }

    [Serializable]
    public struct MapEntities
    {
        [Header("Buildings")]

        [SerializeField]
        private GameObject[] _floorObjs;
        internal readonly GameObject GetRandomFloor()
        {
            return _floorObjs[UnityEngine.Random.Range(0, _floorObjs.Length)];
        }

        [SerializeField]
        private GameObject[] _buildingObjs;
        internal readonly GameObject GetRandomBuilding()
        {
            return _buildingObjs[UnityEngine.Random.Range(0, _buildingObjs.Length)];
        }

        [Header("Structures")]

        [SerializeField]
        private List<StructureData_SO> _structureList;
        internal readonly List<IStructure> StructureList 
            => _structureList.ConvertAll(new Converter<StructureData_SO, IStructure>(i => i));

        [Header("Stuffs")]

        [SerializeField]
        private List<StuffData_SO> stuffs;
        internal readonly List<IStuff> Stuffs 
            => stuffs.ConvertAll(new Converter<StuffData_SO, IStuff>(s => s));
    }
}

namespace Yours.QuickCity
{
    public interface IMapData
    {
        IMapConf    Config { get; }
        MapProperty Properties { get; }
        MapEntities GameObjectDef { get; }
    }
}
