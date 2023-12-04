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

        private readonly List<IMapTerrainDetector> _detectors;

        public MapStuffDetectorEntityGenerator(MapBasicProperty map, MapStuffGenerationProperty stuffProp, MapUtilObjectConf conf, IMapHandler handler)
        {          
            _map = map;
            _conf = conf;
            _stuffGenProp = stuffProp;
            _detectors = new();
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }       
        public List<IMapTerrainDetector> GenerateDetectors(Vector3[] coords)
        {
            foreach (var coord in coords)
            {
                var detector = UnityEngine.Object.Instantiate(
                    _conf.TerrainDetector, 
                    _handler.GetStuffDetectorParent())
                    .GetComponent<IMapTerrainDetector>();

                detector.Init(coord, CalculateDebuggerSize(), _stuffGenProp.DetectorSettings);
                detector.ExecuteDetect();
                detector.ShowDebugColor();
                
                _detectors.Add(detector);
            }
            return _detectors;
        }

        private MapStuffDetectorEntityGenerator() { }
        private float CalculateDebuggerSize()
        {
            return _map.TileUnitSize / (_stuffGenProp.StuffGenerateAccuracy + 1);
        }
    }
}
