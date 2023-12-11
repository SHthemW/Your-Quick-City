using System;
using System.Collections.Generic;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    internal sealed class MapTerrainDetectorGenerator
    {     
        private readonly IMapObjParent _handler;
        private readonly MapUtilObjectConf _conf;

        private readonly MapProperty _map;      

        private int _targetGenerateNum  = 1;
        private int _currentGenerateNum = 0;
        internal bool GenerateIsFinished() 
            => _currentGenerateNum >= _targetGenerateNum;

        internal MapTerrainDetectorGenerator(MapProperty map, MapUtilObjectConf conf, IMapObjParent handler)
        {          
            _map = map;
            _conf = conf;
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }       
        internal IMapTerrainDetector[] GenerateDetectors(Vector3[] coords)
        {
            List<IMapTerrainDetector> detectors = new();
            _targetGenerateNum = coords.Length;

            foreach (var coord in coords)
            {
                var detector = UnityEngine.Object.Instantiate(
                    _conf.TerrainDetector, 
                    _handler.                    TerrainDetectorParent)
                    .GetComponent<IMapTerrainDetector>();

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
