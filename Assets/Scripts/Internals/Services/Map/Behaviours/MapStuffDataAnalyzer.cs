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
        internal MapStuffDataAnalyzer(int maxTick) : base(maxTick) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="detectors"></param>
        /// <returns></returns>
        internal IEnumerator Analysis(MapTerrainDetector[] detectors, Histogram<Dictionary<IStuff, float>> distributionDiagram)
        {
            Result = new(capacity: detectors.Length);

            var totalMapCoords = detectors.Select(d => d.Position).ToArray();

            // temps
            Dictionary<IStuff, float> distInInterval = new(capacity: detectors.Length);

            yield return Foreach(iter: MapUtils.ShuffleRandomly(detectors), body: detector => 
            {
                // for current detector density, get distribution from diagram.
                distInInterval = distributionDiagram.GetMatched(detector.DensityValue);

                // calc distribution weight:
                float totalWeightOfInterval = distInInterval.Sum(d => d.Value);

                // if no any weight, skip current detector
                if (totalWeightOfInterval == 0)
                    throw new ContinueException();

                // else, calc result stuff by its weight.

                float randomSeed = UnityEngine.Random.Range(0, totalWeightOfInterval);
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

                UpdateNearbyCoords(
                    map: totalMapCoords,
                    radius: resultStuff.GetGenerateSpacing(),
                    centerIndex: Array.IndexOf(totalMapCoords, detector.Position));

                if (Result.Any(rst => _nearbyCoords.Contains(rst.Key.pos) && rst.Value == resultStuff))
                    throw new ContinueException();

                // append result
                Result.Add((pos: detector.Position, attachDir: detector.AttachDirection), resultStuff);
            });
        }

        private MapStuffDataAnalyzer() : base(-1)
            => throw new InvalidOperationException();

        private static readonly List<Vector3> _nearbyCoords = new();
        private static void UpdateNearbyCoords(Vector3[] map, int centerIndex, float radius)
        {
            if (map == null || map.Length == 0)
                throw new ArgumentException(nameof(map));

            if (centerIndex > map.Length - 1)
                throw new ArgumentOutOfRangeException(nameof(centerIndex));

            _nearbyCoords.Clear();

            for (int i = 0; i < map.Length; i++)
            {
                if (i == centerIndex)
                    continue;

                if (Vector3.Distance(map[i], map[centerIndex]) <= radius)
                    _nearbyCoords.Add(map[i]);
            }
        }
    }
}
