using Game.General.Interfaces;
using Game.General.Properties;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

namespace Game.Ctrller.Map
{
    public sealed class MapStuffEntityGenerator
    {
        private readonly IMapHandler _entityHandler;
        private readonly MonoBehaviour _coroutineCaster;
        private readonly MapStuffGenerationProperty _stuffGenProp;

        private readonly Dictionary<IStuff, int> _generatedCount = new();

        public MapStuffEntityGenerator(MapStuffGenerationProperty stuffGenProp, IMapHandler entityHandler, MonoBehaviour coroutineCaster)
        {
            _entityHandler = entityHandler ?? throw new ArgumentNullException(nameof(entityHandler));
            _coroutineCaster = coroutineCaster != null ? coroutineCaster : throw new ArgumentNullException(nameof(coroutineCaster));
            _stuffGenProp = stuffGenProp;
        }
        public void GenerateStuffs(in List<IMapTerrainDetector> detectors)
        {
            GenerateStuffGradually(detectors);
        }

        private MapStuffEntityGenerator() { }
        private void GenerateStuffGradually(List<IMapTerrainDetector> detectors)
        {
            foreach (var detector in MapUtils.ShuffleRandomly(detectors.ToArray()))
            {
                if (!GetTargetObject(detector.DensityValue, out IStuff generateTarget))
                    continue;

                //bool success = 
                //    detector.TryGenerateStuffObject(generateTarget, _entityHandler.GetStuffObjParent());

                //if (success)
                //    SignTheGenerate(generateTarget);
            }
        }
        private void SignTheGenerate(IStuff generated)
        {
            if (!_generatedCount.ContainsKey(generated))
                _generatedCount.Add(generated, 1);
            else
                _generatedCount[generated]++;
        }
        private readonly Dictionary<IStuff, float> _alternativeTargets = new();
        private bool GetTargetObject(float density, out IStuff target)
        {
            _alternativeTargets.Clear();

            foreach (var stuff in _stuffGenProp.Stuffs)
            {
                if ((_generatedCount.ContainsKey(stuff)) && (_generatedCount[stuff] >= stuff.MaxGenerateNum))
                    continue;

                if (stuff.DensityIsMatch(density))
                    _alternativeTargets.Add(stuff, stuff.GetDensityMatchingValue(density));
            }

            if (_alternativeTargets.Count == 0)
            {
                target = null;
                return false;
            }
            else
            {
                target = _alternativeTargets.Keys.Aggregate((i, j) => _alternativeTargets[i] > _alternativeTargets[j] ? i : j);
                return true;
            }           
        }
        
    }
}
