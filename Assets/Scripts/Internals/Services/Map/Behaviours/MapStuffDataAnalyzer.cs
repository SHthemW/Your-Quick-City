using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    internal sealed class MapStuffDataAnalyzer : StepwiseTask<Dictionary<(Vector3 pos, Vector3 attachDir), IStuff>>
    {
        public override sealed int maxTick => 1000;

        internal MapStuffDataAnalyzer() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="detectors"></param>
        /// <returns></returns>
        internal IEnumerator Analysis(MapTerrainDetector[] detectors, Dictionary<(float l, float r), Dictionary<IStuff, float>> distributionDiagram)
        {
            Result = new(capacity: detectors.Length);

            _targetStepCount = detectors.Length;
            var totalMapCoords = detectors.Select(d => d.Position).ToArray();

            foreach (var detector in MapUtils.ShuffleRandomly(detectors))
            {
                _currentStepCount++;

                // for current detector density, get distribution from diagram.

                Dictionary<IStuff, float> distInInterval = 
                    distributionDiagram.First(g => 
                    g.Key.l <= detector.DensityValue && 
                    g.Key.r >= detector.DensityValue).Value;

                // calc distribution weight:
                
                float totalWeightOfInterval = distInInterval.Sum(d => d.Value);

                // if no any weight, skip current detector

                if (totalWeightOfInterval == 0)
                    continue;

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

                if (Result.Any(rst => nearbyCoords.Contains(rst.Key.pos) && rst.Value == resultStuff))
                    continue;

                // append result
                Result.Add((pos: detector.Position, attachDir: detector.AttachDirection), resultStuff);

                if (IsTimeToReport())
                {
                    tick = 0;
                    yield return null;
                }
            }
        }

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
    }
}
