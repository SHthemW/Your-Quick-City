using System;
using System.Collections.Generic;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    internal sealed class MapTerrainDetectorGenerator
    {     
        private readonly IMapObjParent _parent;
        private readonly MapTerrainDetector _detectorObject;

        private readonly MapProperty _map;      

        private int _targetGenerateNum  = 1;
        private int _currentGenerateNum = 0;
        internal bool GenerateIsFinished() 
            => _currentGenerateNum >= _targetGenerateNum;

        internal MapTerrainDetectorGenerator(MapProperty map, MapTerrainDetector detector, IMapObjParent handler)
        {          
            _map = map;
            _detectorObject = detector != null ? detector : throw new ArgumentNullException(nameof(detector));
            _parent = handler ?? throw new ArgumentNullException(nameof(handler));
        }       
        internal MapTerrainDetector[] GenerateDetectors(Vector3[] coords)
        {
            List<MapTerrainDetector> detectors = new();
            _targetGenerateNum = coords.Length;

            foreach (var coord in coords)
            {
                var detector = UnityEngine.Object.Instantiate(
                    _detectorObject, 
                    _parent.                    TerrainDetectorParent)
                    .GetComponent<MapTerrainDetector>();

                detector.Init(coord, CalculateDebuggerSize(), _map.DetectorSettings);
                detector.ExecuteDetect();
                detector.ShowDebugColor();

                _currentGenerateNum++;
                detectors.Add(detector);
            }
            return detectors.ToArray();
        }

        private MapTerrainDetectorGenerator() { }
        private float CalculateDebuggerSize()
        {
            return _map.TileUnitSize / (_map.TerrainDetectResolution + 1);
        }
    }
}
