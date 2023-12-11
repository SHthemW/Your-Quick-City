using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    internal sealed class MapStuffDistributionDiagramGenerator : StepwiseTask<Dictionary<(float l, float r), Dictionary<IStuff, float>>>
    {
        private readonly MapProperty _map;
        private readonly MapEntities _mapObjects;

        internal MapStuffDistributionDiagramGenerator(MapEntities mapObjects, MapProperty map, int maxTick) : base(maxTick)
        {
            _mapObjects = mapObjects;
            _map = map;
        }
        internal IEnumerator BakeDistribution(float maxDensity)
        {
            if (maxDensity <= 0)
                throw new ArgumentException();

            Result = new();
            
            (float min, float max) = (
                _mapObjects.Stuffs.Min(s => s.MinGenerateDensity),
                _mapObjects.Stuffs.Max(s => s.MaxGenerateDensity)
            );
            float step = (max - min) / _map.StuffDistributeDiagramResolution;

            _targetStepCount = (int)Math.Ceiling(maxDensity / step);

            // 0 - min - max - infinity
            for (float density = 0; density <= maxDensity; density += step)
            {
                _currentStepCount++;

                var range = (left: density, right: density + step);

                // calc possibility (seriously)
                Dictionary<IStuff, float> generateWeight = new();
                foreach (IStuff stuff in _mapObjects.Stuffs)
                {
                    float match = stuff.GetDensityMatchingValue(density);
                    generateWeight.Add(stuff, match);
                }
                // add to result           
                Result.Add(range, generateWeight);

                if (IsTimeToReport())
                {
                    tick = 0;
                    yield return null;
                }
            }
            yield break;
        }
        internal void PrintDistributionDiagram()
        {
            StringBuilder content = new("Stuff分布信息: \n");

            foreach (var dist in Result)
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

        private MapStuffDistributionDiagramGenerator() : base(-1)
            => throw new InvalidOperationException();
    }
}
