using System;
using System.Collections.Generic;
using UnityEngine;

namespace Yours.QuickCity.Shape
{
    [CreateAssetMenu(fileName = "New RectangleShape", menuName = "Map/Shape/Rectangle")]
    public sealed class RectangleShape_SO : Shape_SO
    {
        // arguments

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
     
        private bool[,] NewSolidRectangleMatrix()
        {
            var matrix = new bool[
                _size_x * _emptySizeMultiplier, 
                _size_y * _emptySizeMultiplier
                ];

            int startX = (int)((float)(_emptySizeMultiplier - 1) / 2 * _size_x);
            int startY = (int)((float)(_emptySizeMultiplier - 1) / 2 * _size_y);

            int endX = startX + _size_x;
            int endY = startY + _size_y;

            // init solid
            for (int x = startX; x < endX; x++)
                for (int y = startY; y < endY; y++)
                    matrix[x, y] = true;

            return matrix;
        }
        private bool[,] GenerateRough(in bool[,] matrix)
        {
            Dictionary<int, int> extendLengthCount = new();

            foreach (var edge in (this as IShape).Edges(matrix))
            {
                var coord = edge.Key;
                var extendDir = edge.Value;

                var roughSeed = RandomRough(
                    min: 0,
                    max: _maxExtendPercent,
                    minValTendency: _attachmentDegree + 1);

                int extendGridNumX = (int)(Mathf.Abs(roughSeed) * _size_x);
                int extendGridNumY = (int)(Mathf.Abs(roughSeed) * _size_y);

                int x, y;

                const int up = -1;
                const int left = -1;

                int countKey = (extendDir == EdgeOutsideDir.Up || extendDir == EdgeOutsideDir.Down)
                    ? extendGridNumY
                    : extendGridNumX;

                if (!extendLengthCount.TryAdd(countKey, 1))
                    extendLengthCount[countKey]++;

                switch (extendDir)
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
            return matrix;

            static float RandomRough(float min, float max, float minValTendency)
            {
                float randomFloat = (float)new System.Random().NextDouble();
                float adjustedRandomFloat = (float)Math.Pow(randomFloat, minValTendency);

                return min + adjustedRandomFloat * (max - min);
            }
        }

        public override sealed bool[,] GenerateShapeMatrix(float sizeMultiple)
        {
            var matrix = NewSolidRectangleMatrix();

            matrix = this.GenerateRough(matrix);
            matrix = (this as IShape).ScaleMatrix(matrix, sizeMultiple);
            matrix = (this as IShape).CropMatrix(matrix);

            return matrix;
        }
    }
}
