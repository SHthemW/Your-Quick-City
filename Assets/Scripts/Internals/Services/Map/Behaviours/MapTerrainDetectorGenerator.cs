using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    internal sealed class MapTerrainDetectorGenerator : StepwiseTask<List<MapTerrainDetector>>
    {
        private readonly MapProperty _map;
        private readonly IMapObjParent _parent;
        private readonly MapTerrainDetector _detectorObject;
        
        public override int MaxTrick => 1000;

        internal MapTerrainDetectorGenerator(MapProperty map, MapTerrainDetector detector, IMapObjParent handler)
        {          
            _map = map;
            _detectorObject = detector != null ? detector : throw new ArgumentNullException(nameof(detector));
            _parent = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        internal IEnumerator GenerateDetectors(Vector3[] coords)
        {
            _targetStepCount = coords.Length;
            Result = new();

            foreach (var coord in coords)
            {
                var detector = UnityEngine.Object.Instantiate(
                    _detectorObject, 
                    _parent.TerrainDetectorParent)
                    .GetComponent<MapTerrainDetector>();

                detector.Init(coord, CalculateDebuggerSize(), _map.DetectorSettings);
                detector.ExecuteDetect();
                detector.ShowDebugColor();

                _currentStepCount++;
                Result.Add(detector);

                if (IsTimeToReport())
                {
                    Trick = 0;
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
