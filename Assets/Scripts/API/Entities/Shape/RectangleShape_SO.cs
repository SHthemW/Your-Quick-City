using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Yours.QuickCity.Internal;

namespace Yours.QuickCity.Shape
{
    [CreateAssetMenu(fileName = "New RectangleShape", menuName = "Map/Shape/Rectangle")]
    public sealed class RectangleShape_SO : ScriptableObject, IShape
    {
        [Header("Rectangle")]

        [SerializeField]
        private int _size_x;

        [SerializeField]
        private int _size_y;

        [SerializeField, Range(0, 1)]
        private float _maxEdgeConvexityPercent;

        [Header("Advanced")]

        [SerializeField, Range(1, 10)]
        private int _matrixAllocSizeMultiplier = 3;

        public bool[,] GenerateMatrix()
        {
            int matrix_size_x = _size_x * _matrixAllocSizeMultiplier;
            int matrix_size_y = _size_y * _matrixAllocSizeMultiplier;

            bool[,] matrix = new bool[matrix_size_x, matrix_size_y];

            int startX = (int)((float)(_matrixAllocSizeMultiplier - 1) / 2 * _size_x);
            int startY = (int)((float)(_matrixAllocSizeMultiplier - 1) / 2 * _size_y);

            int endX = startX + _size_x;
            int endY = startY + _size_y;

            // init
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    matrix[x, y] = true;
                }
            }

            // edge
            Dictionary<(int x, int y), EdgeOutsideDir> edges = new();

            for (int x = 0; x < matrix_size_x; x++)
            {
                for (int y = 0; y < matrix_size_y; y++)
                {
                    if (!matrix[x, y])
                        continue;

                    if ((x + 1) >= matrix_size_x
                        || (y + 1) >= matrix_size_y
                        || (x - 1) < 0 
                        || (y - 1) < 0)
                        continue;

                    bool left  = matrix[x - 1, y];
                    bool right = matrix[x + 1, y];
                    bool up    = matrix[x, y - 1];
                    bool down  = matrix[x, y + 1];

                    if (left && right && up && down)
                        continue;

                    if (left && !right)
                        edges.Add((x, y), EdgeOutsideDir.Right);

                    else if (!left && right)
                        edges.Add((x, y), EdgeOutsideDir.Left);

                    else if (up && !down)
                        edges.Add((x, y), EdgeOutsideDir.Down);

                    else if (!up && down)
                        edges.Add((x, y), EdgeOutsideDir.Up);
                }
            }

            Debug.Log("edge count:" + edges.Count);

            foreach (var edge in edges)
            {
                try
                {
                    SetConcaveAndConvexity(edge.Key, edge.Value);
                }
                catch (IndexOutOfRangeException e) 
                {
                    Debug.LogWarning(e.Message);
                    continue;
                }
            }

            return matrix;

            void SetConcaveAndConvexity((int x, int y) coord, EdgeOutsideDir extendDir)
            {
                var roughSeed = RandomFunc(0, _maxEdgeConvexityPercent);

                int extendGridNumX = (int)(Mathf.Abs(roughSeed) * _size_x);
                int extendGridNumY = (int)(Mathf.Abs(roughSeed) * _size_y);

                int x, y;

                const int up   = -1;
                const int left = -1;

                switch(extendDir)
                {
                    case EdgeOutsideDir.Up:
                        y = coord.y;
                        for (int count = 0; count < extendGridNumY; count++)
                        {
                            matrix[coord.x, y] = true;
                            y += up;
                        }
                        break;
                    case EdgeOutsideDir.Down:
                        y = coord.y;
                        for (int count = 0; count < extendGridNumY; count++)
                        {
                            matrix[coord.x, y] = true;
                            y -= up;
                        }
                        break;
                    case EdgeOutsideDir.Left:
                        x = coord.x;
                        for (int count = 0; count < extendGridNumX; count++) 
                        {
                            matrix[x, coord.y] = true;
                            x += left;
                        }
                        break;
                    case EdgeOutsideDir.Right:
                        x = coord.x;
                        for (int count = 0; count < extendGridNumX; count++)
                        {
                            matrix[x, coord.y] = true;
                            x -= left;
                        }
                        break;
                    default: 
                        throw new ArgumentException();
                }
            }
        }

        private static readonly System.Random random = new();

        private static readonly Func<float, float, float> RandomFunc = RandomFunc_1;

        private static float RandomFunc_1(float small, float large) // TODO: *Shape: IRandomFunc interface
        {
            return UnityEngine.Random.Range(small, large);
        }
        private static float RandomFunc_2(float small, float large)
        {
            float range = large - small;
            float randomValue = (float)((Math.Asin((2 * random.NextDouble()) - 1) / Math.PI + 0.5) * range + small);
            return randomValue;
        }


        [ContextMenu("Print Debug Graph")]
        private void PrintGraphOnConsole()
        {
            var generated = GenerateMatrix();
            StringBuilder msg = new ("graph: \n");

            for (int x = 0; x < generated.GetLength(0); x++)
            {
                for (int y = 0; y < generated.GetLength(1); y++)
                {
                    msg.Append((generated[x, y] ? "■" : "□") + "  ");           
                }
                msg.Append("\n");
            }
            Debug.Log(msg);
        }

        [ContextMenu("Print Random Func")]
        private void PrintFunctionTestOnConsole()
        {
            const int times = 10000;

            List<float> result = new();

            for (int count = 0; count < times; count++)
            {
                result.Add(RandomFunc(0, 1));
            }

            Histogram<float, string> histogram = new(
                dataSet:      result, 
                getKeyIn:     num => num,
                constrToVal:  interval => new string('|', interval.Count() / 20),
                intervalSize: 0.1f);

            Debug.Log(histogram.DebugMessage);
        }
    }
}
