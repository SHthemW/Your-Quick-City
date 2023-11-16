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
            _coroutineCaster.StartCoroutine(GenerateStuffGradually(detectors));
        }

        private MapStuffEntityGenerator() { }
        private IEnumerator GenerateStuffGradually(List<IMapTerrainDetector> detectors)
        {
            foreach (var detector in MapUtils.ShuffleRandomly(detectors.ToArray()))
            {
                if (!GetTargetObject(detector.DensityValue, out IStuff generateTarget))
                    continue;

                yield return _coroutineCaster.StartCoroutine(
                        detector.GenerateStuffAndDestorySelf(generateTarget, _entityHandler.GetStuffObjParent()));

                if (detector.CanStuffGenerateValidly)
                    SignTheGenerate(generateTarget);
            }
        }
        private void SignTheGenerate(IStuff generated)
        {
            if (!_generatedCount.ContainsKey(generated))
                _generatedCount.Add(generated, 1);
            else
                _generatedCount[generated]++;
        }
        private bool GetTargetObject(float density, out IStuff target)
        {
            Dictionary<IStuff, float> alternatives = new();

            foreach (var stuff in _stuffGenProp.Stuffs)
            {
                if ((_generatedCount.ContainsKey(stuff)) && (_generatedCount[stuff] >= stuff.MaxGenerateNum))
                    continue;

                if (stuff.DensityIsMatch(density))
                    alternatives.Add(stuff, stuff.GetDensityMatchingValue(density));
            }

            if (alternatives.Count == 0)
            {
                target = null;
                return false;
            }
            else
            {
                target = alternatives.Keys.Aggregate((i, j) => alternatives[i] > alternatives[j] ? i : j);
                return true;
            }           
        }
        
    }
}
