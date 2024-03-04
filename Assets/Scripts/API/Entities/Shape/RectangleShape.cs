using System;
using System.Collections.Generic;
using UnityEngine;

namespace Yours.QuickCity.Shape
{
    [Serializable]
    public struct RectangleShape : IShape
    {
        [field: Header("Rectangle")]

        [field: SerializeField]
        public int Size_x { get; set; }

        [field: SerializeField]
        public int Size_y { get; set; }

        [field: SerializeField, Range(1, 10)]
        public int EmptySizeMultiplier { get; set; }

        [field: Header("Advanced")]

        [field: SerializeField, Range(0, 1)]
        public float MaxExtendPercent { get; set; }

        [field: SerializeField, Range(-0.9f, 1)]
        public float AttachmentDegree { get; set; }

        private readonly bool[,] NewSolidRectangleMatrix()
        {
            var matrix = new bool[
                Size_x * EmptySizeMultiplier,
                Size_y * EmptySizeMultiplier
                ];

            int startX = (int)((float)(EmptySizeMultiplier - 1) / 2 * Size_x);
            int startY = (int)((float)(EmptySizeMultiplier - 1) / 2 * Size_y);

            int endX = startX + Size_x;
            int endY = startY + Size_y;

            // init solid
            for (int x = startX; x < endX; x++)
                for (int y = startY; y < endY; y++)
                    matrix[x, y] = true;

            return matrix;
        }
        private readonly bool[,] GenerateRough(in bool[,] matrix)
        {
            Dictionary<int, int> extendLengthCount = new();

            foreach (var edge in (this as IShape).Edges(matrix))
            {
                var coord = edge.Key;
                var extendDir = edge.Value;

                var roughSeed = RandomRough(
                    min: 0,
                    max: MaxExtendPercent,
                    minValTendency: AttachmentDegree + 1);

                int extendGridNumX = (int)(Mathf.Abs(roughSeed) * Size_x);
                int extendGridNumY = (int)(Mathf.Abs(roughSeed) * Size_y);

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

        readonly bool[,] IShape.GenerateShapeMatrix(float sizeMultiple)
        {
            var matrix = NewSolidRectangleMatrix();

            matrix = this.GenerateRough(matrix);
            matrix = (this as IShape).ScaleMatrix(matrix, sizeMultiple);
            matrix = (this as IShape).CropMatrix(matrix);

            return matrix;
        }
    }
}