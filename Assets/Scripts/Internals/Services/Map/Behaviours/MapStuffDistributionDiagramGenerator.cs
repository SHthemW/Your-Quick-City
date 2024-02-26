using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    internal sealed class MapStuffDistributionDiagramGenerator : StepwiseTask<Histogram<Dictionary<IStuff, float>>>
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
            float curDensity = 0;

            yield return For(
                continueCondition: curDensity <= maxDensity, 
                stepCount: (int)Math.Ceiling(maxDensity / step), 
                body: () =>
            {
                var range = (left: curDensity, right: curDensity + step);

                // calc possibility (seriously)
                Dictionary<IStuff, float> generateWeight = new();
                foreach (IStuff stuff in _mapObjects.Stuffs)
                {
                    float match = stuff.GetDensityMatchingValue(curDensity);
                    generateWeight.Add(stuff, match);
                }
                // add to result           
                Result.AddInAsc(new HistogramInterval<Dictionary<IStuff, float>>(range, generateWeight));
            }, 
            endStepFunc: () => curDensity += step);
        }
        internal void PrintDistributionDiagram()
        {
            Debug.Log(Result);
        }

        private MapStuffDistributionDiagramGenerator() : base(-1)
            => throw new InvalidOperationException();
    }
}
