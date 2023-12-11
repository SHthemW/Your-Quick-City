using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    internal sealed class MapStuffDataAnalyzer
    {
        private readonly MapProperty _map;
        private readonly MapEntities _mapObjects;

        private int _targetAnalysisNum  = 1;
        private int _currentAnalysisNum = 0;

        internal float FinishedPercent()
        {
            return ((float)_currentAnalysisNum / _targetAnalysisNum) * 100;
        }

        private Dictionary<(float l, float r), Dictionary<IStuff, float>> _distributionDiagram;

        internal MapStuffDataAnalyzer(MapEntities mapObjects, MapProperty map)
        {
            _mapObjects = mapObjects;
            _map = map;
        }

        internal bool Finished() => _currentAnalysisNum >= _targetAnalysisNum;
        internal Dictionary<(Vector3 pos, Vector3 attachDir), IStuff> Result { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="detectors"></param>
        /// <returns></returns>
        internal IEnumerator Analysis(MapTerrainDetector[] detectors)
        {
            _targetAnalysisNum = detectors.Length;

            Result = new (capacity: detectors.Length);
            int trick = 0;

            var totalMapCoords = detectors.Select(d => d.Position).ToArray();

            _distributionDiagram = BakeStuffDistributionDiagram(detectors.Max(d => d.DensityValue));

            foreach (var detector in MapUtils.ShuffleRandomly(detectors))
            {
                // for current detector density, get distribution from diagram.

                Dictionary<IStuff, float> distInInterval = 
                    _distributionDiagram.First(g => 
                    g.Key.l <= detector.DensityValue && 
                    g.Key.r >= detector.DensityValue).Value;

                // calc distribution weight:
                
                float totalWeightOfInterval = distInInterval.Sum(d => d.Value);

                // if no any weight, skip current detector

                if (totalWeightOfInterval == 0)
                {
                    _currentAnalysisNum++;
                    trick++;
                    continue;
                }

                // else, calc result stuff by its weight.

                float  randomSeed  = UnityEngine.Random.Range(0, totalWeightOfInterval);
                IStuff resultStuff = null;

                foreach (var match in distInInterval)
                {
                    randomSeed -= match.Value;
                    if (randomSeed <= 0)
                    {
                        resultStuff = match.Key;
                        break;
                    }
                }

                // check if current result match the space distance limit

                var nearbyCoords = GetNearbyCoordIndexes(
                    map:         totalMapCoords, 
                    radius:      resultStuff.GetGenerateSpacing(),
                    centerIndex: Array.IndexOf(totalMapCoords, detector.Position)
                    )
                    .Select(i => totalMapCoords[i])
                    .ToArray();

                if (Result.Any(rst => 
                    nearbyCoords.Contains(rst.Key.pos) && rst.Value == resultStuff))
                {
                    trick++;
                    _currentAnalysisNum++;
                    continue;
                }

                // append result
                _currentAnalysisNum++;
                Result.Add((pos: detector.Position, attachDir: detector.AttachDirection), resultStuff);

                if (trick++ > 1000 || _targetAnalysisNum - _currentAnalysisNum <= trick)
                {
                    trick = 0;
                    yield return null;
                }
            }
        }

        internal void PrintDistributionDiagram()
        {
            StringBuilder content = new("Stuff分布信息: \n");

            foreach (var dist in _distributionDiagram)
            {
                content.AppendLine($"[{dist.Key.l} - {dist.Key.r}]");

                foreach (var stuff in dist.Value)
                {
                    var keyStr = stuff.Key.Obj.name.PadRight(10);
                    var valStr = stuff.Value.ToString().PadRight(10);

                    content.AppendLine($"- {keyStr}: {valStr}");
                }
            }
            Debug.Log(content.ToString());
        }

        private MapStuffDataAnalyzer()
            => throw new NotImplementedException();

        private static int[] GetNearbyCoordIndexes(Vector3[] map, int centerIndex, float radius)
        {
            if (map == null || map.Length == 0)
                throw new ArgumentException(nameof(map));

            if (centerIndex > map.Length - 1)
                throw new ArgumentOutOfRangeException(nameof(centerIndex));

            List<int> nearbyIndexes = new();

            for (int i = 0; i < map.Length; i++)
            {
                if (i == centerIndex) 
                    continue;

                if (Vector3.Distance(map[i], map[centerIndex]) <= radius)
                    nearbyIndexes.Add(i);
            }
            return nearbyIndexes.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toBeBake"></param>
        /// <param name="maxDensity"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private Dictionary<(float l, float r), Dictionary<IStuff, float>> BakeStuffDistributionDiagram(float maxDensity)
        {
            if (maxDensity <= 0)
                throw new ArgumentException();

            Dictionary<(float, float), Dictionary<IStuff, float>> bakeResult = new();

            (float min, float max) = (
                _mapObjects.Stuffs.Min(s => s.MinGenerateDensity),
                _mapObjects.Stuffs.Max(s => s.MaxGenerateDensity)
            );
            float step = (max - min) / _map.StuffDistributeDiagramResolution;

            // 0 - min - max - infinity
            for (float density = 0; density <= maxDensity; density += step)
            {
                var range = (left: density, right: density + step);

                // calc possibility (seriously)
                Dictionary<IStuff, float> generateWeight = new();
                foreach (IStuff stuff in _mapObjects.Stuffs)
                {
                    float match = stuff.GetDensityMatchingValue(density);
                    generateWeight.Add(stuff, match);
                }
                // add to result           
                bakeResult.Add(range, generateWeight);
            }

            return bakeResult;
        }
    }
}
