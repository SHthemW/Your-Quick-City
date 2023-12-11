using System;
using System.Collections;
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
        private const int MAX_TRICK  = 1000;

        internal float FinishedPercent()
        {
            return (float)_currentGenerateNum / _targetGenerateNum * 100;
        }
        internal bool GenerateIsFinished() 
            => _currentGenerateNum >= _targetGenerateNum;

        internal MapTerrainDetectorGenerator(MapProperty map, MapTerrainDetector detector, IMapObjParent handler)
        {          
            _map = map;
            _detectorObject = detector != null ? detector : throw new ArgumentNullException(nameof(detector));
            _parent = handler ?? throw new ArgumentNullException(nameof(handler));
        }       
        internal List<MapTerrainDetector> Result { get; private set; } = new();
        internal IEnumerator GenerateDetectors(Vector3[] coords)
        {
            _targetGenerateNum = coords.Length;
            int trick = 0;

            foreach (var coord in coords)
            {
                var detector = UnityEngine.Object.Instantiate(
                    _detectorObject, 
                    _parent.TerrainDetectorParent)
                    .GetComponent<MapTerrainDetector>();

                detector.Init(coord, CalculateDebuggerSize(), _map.DetectorSettings);
                detector.ExecuteDetect();
                detector.ShowDebugColor();

                _currentGenerateNum++;
                Result.Add(detector);

                if (trick++ > MAX_TRICK || _targetGenerateNum - _currentGenerateNum <= trick)
                {
                    trick = 0;
                    yield return null;
                }
            }
            yield break;
        }

        private MapTerrainDetectorGenerator() { }
        private float CalculateDebuggerSize()
        {
            return _map.TileUnitSize / (_map.TerrainDetectResolution + 1);
        }
    }
}
