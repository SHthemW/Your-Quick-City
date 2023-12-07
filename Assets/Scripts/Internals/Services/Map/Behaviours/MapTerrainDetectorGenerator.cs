using System;
using System.Collections.Generic;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    internal sealed class MapTerrainDetectorGenerator
    {     
        private readonly IMapObjParent _handler;
        private readonly MapUtilObjectConf _conf;

        private readonly MapSizeProperty _map;      
        private readonly MapStuffGenerationProperty _stuffGenProp;

        private int _targetGenerateNum  = 1;
        private int _currentGenerateNum = 0;
        internal bool GenerateIsFinished() 
            => _currentGenerateNum >= _targetGenerateNum;

        internal MapTerrainDetectorGenerator(MapSizeProperty map, MapStuffGenerationProperty stuffProp, MapUtilObjectConf conf, IMapObjParent handler)
        {          
            _map = map;
            _conf = conf;
            _stuffGenProp = stuffProp;
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
                    _handler.GetStuffDetectorParent())
                    .GetComponent<IMapTerrainDetector>();

                detector.Init(coord, CalculateDebuggerSize(), _stuffGenProp.DetectorSettings);
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
            return _map.TileUnitSize / (_stuffGenProp.StuffGenerateAccuracy + 1);
        }
    }
}
