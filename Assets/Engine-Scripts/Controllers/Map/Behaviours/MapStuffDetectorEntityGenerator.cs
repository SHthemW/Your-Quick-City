using Game.General.Interfaces;
using Game.General.Properties;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Ctrller.Map
{
    public sealed class MapStuffDetectorEntityGenerator
    {     
        private readonly IMapHandler _handler;
        private readonly MapUtilObjectConf _conf;

        private readonly MapBasicProperty _map;      
        private readonly MapStuffGenerationProperty _stuffGenProp;

        private int _targetGenerateNum  = 0;
        private int _currentGenerateNum = 0;
        public bool GenerateIsFinished() 
            => _currentGenerateNum >= _targetGenerateNum;

        public MapStuffDetectorEntityGenerator(MapBasicProperty map, MapStuffGenerationProperty stuffProp, MapUtilObjectConf conf, IMapHandler handler)
        {          
            _map = map;
            _conf = conf;
            _stuffGenProp = stuffProp;
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }       
        public IMapTerrainDetector[] GenerateDetectors(Vector3[] coords)
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

        private MapStuffDetectorEntityGenerator() { }
        private float CalculateDebuggerSize()
        {
            return _map.TileUnitSize / (_stuffGenProp.StuffGenerateAccuracy + 1);
        }
    }
}
