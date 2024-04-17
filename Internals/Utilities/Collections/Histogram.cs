using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yours.QuickCity.Internal
{
    internal readonly struct HistogramInterval<TValue>
    {
        internal (float l, float r) Interval { get; }
        internal TValue Value { get; }

        private readonly static StringBuilder _strBuilder = new();
        private readonly string ValueStr => Value switch
        {
            IDictionary dict => dict.Keys.Cast<object>()
                .Zip(
                    second: dict.Values.Cast<object>(), 
                    resultSelector: (k, v) => $"{k.ToString().Split('(').First()} : {v}")
                .Aggregate(
                    seed: _strBuilder.Clear(), 
                    func: (sb, str) => sb.AppendLine(str), 
                    resultSelector: sb => sb.ToString()
                    ),

            _ => Value.ToString()
        };

        internal HistogramInterval((float l, float r) interval, TValue value)
        {
            Interval = interval;
            Value = value;
        }

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
        public   readonly override string ToString()
        {
            return 
                $"[{Math.Round(Interval.l, 1)}-{Math.Round(Interval.r, 1)}]\n" +
                $" {ValueStr}\n";
        }
    }

    internal sealed class Histogram<TValue>
    {
        private readonly List<HistogramInterval<TValue>> _diagram;
        private readonly string _title = "Distribution";

        internal Histogram()
        {
            _diagram = new();
        }     
        internal Histogram(string title)
        {
            _diagram = new();
            _title = title ?? throw new ArgumentNullException();
        }
     
        /// <summary>
        /// construct histogram with existing dataSet.
        /// </summary>
        /// <param name="dataSet">data to construct Histogram</param>
        /// <param name="getKeyIn">function that get interval key in each data</param>
        /// <param name="constrToVal">function that convert an interval to final Histogram value.</param>
        /// <param name="intervalSize">size of each interval</param>
        internal void Construct<TData>(in List<TData> dataSet, Func<TData, float> getKeyIn, Func<IEnumerable<TData>, TValue> constrToVal, float intervalSize)
        {
            List<float> keys = new();

            foreach (var data in dataSet)
                keys.Add(getKeyIn(data));

            keys.Sort();

            int index = 0;
            while (index < keys.Count)
            {
                float intervalStart = keys[index];
                float intervalEnd = intervalStart + intervalSize;
                List<TData> valuesInInterval = new();

                while (index < keys.Count && keys[index] < intervalEnd)
                {
                    valuesInInterval.Add(dataSet[index]);
                    index++;
                }

                TValue value = constrToVal(valuesInInterval);
                _diagram.Add(new HistogramInterval<TValue>((intervalStart, intervalEnd), value));
            }
        }

        private float _lastAddedIntervalRight = -1;
        internal void AddInAsc(HistogramInterval<TValue> distribution)
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
        internal TValue GetMatched(float value)
        {
            return _diagram.First(dist => dist.TryMatch(value) == 0).Value;

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

        public override sealed string ToString()
        {
            StringBuilder content = new($"{_title}: \n");

            foreach (var dist in _diagram)
                content.AppendLine(dist.ToString());
            return content.ToString();
        }
    }
}
