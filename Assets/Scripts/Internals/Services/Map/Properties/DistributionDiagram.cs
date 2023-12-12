using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yours.QuickCity.Internal
{
    internal struct Distribution
    {
        internal (float l, float r) Interval { get; set; }
        internal Dictionary<IStuff, float> Content { get; set; }

        /// <summary>
        /// compare specificValue with Interval.
        /// </summary>
        /// <param name="specificValue"></param>
        /// <returns>0 if match, -1 if in left of interval, 1 if in right.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        internal readonly int TryMatch(float specificValue)
        {
            // match
            if (Interval.l <= specificValue && Interval.r >= specificValue) 
                return 0;
            // in left
            else if (specificValue < Interval.l) 
                return -1;
            // in right
            else if (specificValue > Interval.r) 
                return 1;

            else throw new ArgumentOutOfRangeException();
        }       
    }

    internal sealed class DistributionDiagram
    {
        private readonly List<Distribution> _diagram;

        internal DistributionDiagram()
        {
            _diagram = new();
        }
        internal string DebugMessage 
        {
            get
            {
                StringBuilder content = new("Stuff分布信息: \n");

                foreach (var dist in _diagram)
                {
                    content.AppendLine($"[{dist.Interval.l} - {dist.Interval.r}]");

                    foreach (var stuff in dist.Content)
                    {
                        var keyStr = stuff.Key.Obj.name.PadRight(10);
                        var valStr = stuff.Value.ToString().PadRight(10);

                        content.AppendLine($"- {keyStr}: {valStr}");
                    }
                }
                return content.ToString();
            }
         }


        private float _lastAddedIntervalRight = -1;
        internal void AddInAsc(Distribution distribution)
        {
            if (_lastAddedIntervalRight != -1)
                if (distribution.Interval.l < _lastAddedIntervalRight)
                    throw new ArgumentOutOfRangeException("distribution diagram must be add by asc.");

            _diagram.Add(distribution);
            _lastAddedIntervalRight = distribution.Interval.r;
        }

        /// <summary>
        /// get matched interval's value by bin search.
        /// </summary>
        /// <param name="value">specific value</param>
        /// <returns></returns>
        internal Dictionary<IStuff, float> GetMatched(float value)
        {
            return _diagram.First(dist => dist.TryMatch(value) == 0).Content;

            //int left  = 0;
            //int right = _diagram.Count - 1;

            //while (left <= right)
            //{
            //    int middle = (left + right) / 2;
               
            //    switch (_diagram[middle].TryMatch(value))
            //    {
            //        case 0:
            //            return _diagram[middle].Content;
            //        case 1: 
            //            left = middle + 1;
            //            break;
            //        case -1:
            //            right = middle - 1;
            //            break;
            //        default:
            //            throw new InvalidOperationException();
            //    }
            //}
            //throw new KeyNotFoundException($"key: {value}");
        }
    }
}
