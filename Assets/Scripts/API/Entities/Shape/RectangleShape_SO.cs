using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yours.QuickCity.Internal;

namespace Yours.QuickCity.Shape
{
    [CreateAssetMenu(fileName = "New RectangleShape", menuName = "Map/Shape/Rectangle")]
    public sealed class RectangleShape_SO : Shape_SO
    {
        [Header("Rectangle")]

        [SerializeField]
        private int _size_x;

        [SerializeField]
        private int _size_y;

        [SerializeField, Range(1, 10)]
        private int _emptySizeMultiplier = 3;

        [Header("Advanced")]

        [SerializeField, Range(0, 1)]
        private float _maxExtendPercent;

        [SerializeField, Range(-0.9f, 1)]
        private float _attachmentDegree = 0;

        protected override sealed string ShapeDebugMsg
        {
            get
            {
                Histogram<KeyValuePair<int, int>, string> extendStat = new(
                    dataSet: _extendLengthCount.ToList(),
                    getKeyIn: kvp => kvp.Key,
                    constrToVal: dic => new string('|', dic.Sum(kvp => kvp.Value)),
                    intervalSize: 1
                    );

                return base.ShapeDebugMsg
                    + "stat:\n"
                    + extendStat.DebugMessage;
            }
        }
        protected override sealed void GenerateOnMatrix()
        {
            int matrix_size_x = _size_x * _emptySizeMultiplier;
            int matrix_size_y = _size_y * _emptySizeMultiplier;

            _matrix = new bool[matrix_size_x, matrix_size_y];

            int startX = (int)((float)(_emptySizeMultiplier - 1) / 2 * _size_x);
            int startY = (int)((float)(_emptySizeMultiplier - 1) / 2 * _size_y);

            int endX = startX + _size_x;
            int endY = startY + _size_y;

            // init solid
            for (int x = startX; x < endX; x++)
                for (int y = startY; y < endY; y++)
                    _matrix[x, y] = true;

            // edge
            Dictionary<(int x, int y), EdgeOutsideDir> edges = new();

            for (int x = 0; x < matrix_size_x; x++)
            {
                for (int y = 0; y < matrix_size_y; y++)
                {
                    if ((x + 1) >= matrix_size_x
                        || (y + 1) >= matrix_size_y
                        || (x - 1) < 0
                        || (y - 1) < 0)
                        continue;

                    IEdgeJudger edgeJudger = new CrossEdgeJudger();
                    var direction = edgeJudger.Judge(_matrix, (x, y));

                    if (direction != EdgeOutsideDir.NotEdge)
                        edges.Add((x, y), direction);
                }
            }

            foreach (var edge in edges)
                SetConcaveAndConvexity(edge.Key, edge.Value);
        }

        private void SetConcaveAndConvexity((int x, int y) coord, EdgeOutsideDir extendDir)
        {
            var roughSeed = RandomRoughFunc(
                min:            0, 
                max:            _maxExtendPercent, 
                minValTendency: _attachmentDegree + 1);

            int extendGridNumX = (int)(Mathf.Abs(roughSeed) * _size_x);
            int extendGridNumY = (int)(Mathf.Abs(roughSeed) * _size_y);

            int x, y;

            const int up = -1;
            const int left = -1;

            int countKey = (extendDir == EdgeOutsideDir.Up || extendDir == EdgeOutsideDir.Down)
                ? extendGridNumY
                : extendGridNumX;

            if (!_extendLengthCount.TryAdd(countKey, 1))
                _extendLengthCount[countKey]++;

            switch (extendDir)
            {
                case EdgeOutsideDir.Up:
                    y = coord.y;
                    for (int count = 0; count < extendGridNumY; count++)
                    {
                        _matrix[coord.x, y] = true;
                        y += up;
                    }
                    break;
                case EdgeOutsideDir.Down:
                    y = coord.y;
                    for (int count = 0; count < extendGridNumY; count++)
                    {
                        _matrix[coord.x, y] = true;
                        y -= up;
                    }
                    break;
                case EdgeOutsideDir.Left:
                    x = coord.x;
                    for (int count = 0; count < extendGridNumX; count++)
                    {
                        _matrix[x, coord.y] = true;
                        x += left;
                    }
                    break;
                case EdgeOutsideDir.Right:
                    x = coord.x;
                    for (int count = 0; count < extendGridNumX; count++)
                    {
                        _matrix[x, coord.y] = true;
                        x -= left;
                    }
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private static float RandomRoughFunc(float min, float max, float minValTendency)
        {
            float randomFloat         = (float)new System.Random().NextDouble();
            float adjustedRandomFloat = (float)Math.Pow(randomFloat, minValTendency);

            return min + adjustedRandomFloat * (max - min);
        }

        private readonly Dictionary<int, int> _extendLengthCount = new();

        [ContextMenu("Print Random Func")]
        private void PrintFunctionTestOnConsole()
        {
            const int times = 10000;

            List<float> result = new();

            for (int count = 0; count < times; count++)
            {
                result.Add(RandomRoughFunc(0, 1, _attachmentDegree + 1));
            }

            Histogram<float, string> histogram = new(
                dataSet: result,
                getKeyIn: num => num,
                constrToVal: interval => new string('|', interval.Count() / 20),
                intervalSize: 0.1f);

            Debug.Log(histogram.DebugMessage);
        }
    }
}
