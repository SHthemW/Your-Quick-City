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
        
        internal MapTerrainDetectorGenerator(MapProperty map, MapTerrainDetector detector, IMapObjParent handler, int maxTick) : base(maxTick)
        {          
            _map = map;
            _detectorObject = detector != null ? detector : throw new ArgumentNullException(nameof(detector));
            _parent = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        internal IEnumerator GenerateDetectors(Vector3[] coords)
        {
            Result = new();

            yield return Foreach(iter: coords, stepCount: coords.Length, body: coord => 
            {
                var detector = UnityEngine.Object.Instantiate(
                    _detectorObject,
                    _parent.TerrainDetectorParent)
                    .GetComponent<MapTerrainDetector>();

                detector.Init(coord, CalculateDebuggerSize(), _map.DetectorSettings);
                detector.ExecuteDetect();
                detector.ShowDebugColor();

                Result.Add(detector);
            });
        }

        private MapTerrainDetectorGenerator() : base(-1)
            => throw new InvalidOperationException();
        private float CalculateDebuggerSize()
        {
            return _map.TileUnitSize / (_map.TerrainDetectResolution + 1);
        }
    }
}
