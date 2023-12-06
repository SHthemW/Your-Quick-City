using Game.General.Interfaces;
using Game.General.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game.Ctrller.Map
{
    public sealed class MapStuffDataAnalyzer
    {
        private readonly MapStuffGenerationProperty _stuffGenProp;

        private int _targetAnalysisNum  = 1;
        private int _currentAnalysisNum = 0;
        private Dictionary<(float l, float r), Dictionary<IStuff, float>> _distributionDiagram;

        public MapStuffDataAnalyzer(MapStuffGenerationProperty stuffGenProp)
        {
            _stuffGenProp = stuffGenProp;
        }

        public bool Finished() => _currentAnalysisNum >= _targetAnalysisNum;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="detectors"></param>
        /// <returns></returns>
        public Dictionary<(Vector3, Vector3), IStuff> Analysis(in IMapTerrainDetector[] detectors)
        {
            _targetAnalysisNum = detectors.Length;

            var analysisResult = new Dictionary<(Vector3 pos, Vector3 attachDir), IStuff>(capacity: detectors.Length);

            _distributionDiagram = BakeStuffDistributionDiagram(_stuffGenProp, detectors.Max(d => d.DensityValue));

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
                    continue;
                }

                // else, calc result stuff by its weight.

                float  randomSeed  = UnityEngine.Random.Range(0, totalWeightOfInterval);
                IStuff randomStuff = null;

                foreach (var match in distInInterval)
                {
                    randomSeed -= match.Value;
                    if (randomSeed <= 0)
                    {
                        randomStuff = match.Key;
                        break;
                    }
                }

                // add result to result list

                _currentAnalysisNum++;
                analysisResult.Add((pos: detector.Position, attachDir: detector.AttachDirection), randomStuff);
            }
            return analysisResult;
        }

        public void PrintDistributionDiagram()
        {
            StringBuilder content = new();

            foreach (var dist in _distributionDiagram)
            {
                content.Append($"\n[{dist.Key.l} - {dist.Key.r}] \n");

                foreach (var stuff in dist.Value)
                {
                    var keyStr = stuff.Key.Obj.name.PadRight(10);
                    var valStr = stuff.Value.ToString().PadRight(10);

                    content.Append($"- {keyStr}: {valStr} \n");
                }
            }
            Debug.Log(content.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toBeBake"></param>
        /// <param name="maxDensity"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static Dictionary<(float l, float r), Dictionary<IStuff, float>> BakeStuffDistributionDiagram(MapStuffGenerationProperty toBeBake, float maxDensity)
        {
            if (maxDensity <= 0)
                throw new ArgumentException();

            Dictionary<(float, float), Dictionary<IStuff, float>> bakeResult = new();

            (float min, float max) = (
                toBeBake.Stuffs.Min(s => s.MinGenerateDensity),
                toBeBake.Stuffs.Max(s => s.MaxGenerateDensity)
            );
            float step = (max - min) / toBeBake.StuffDistributeDiagramResolution;

            // 0 - min - max - infinity
            for (float density = 0; density <= maxDensity; density += step)
            {
                var range = (left: density, right: density + step);

                // calc possibility (seriously)
                Dictionary<IStuff, float> generateWeight = new();
                foreach (IStuff stuff in toBeBake.Stuffs)
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
