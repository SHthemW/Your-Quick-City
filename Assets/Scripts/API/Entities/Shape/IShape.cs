using System;
using System.Collections.Generic;

namespace Yours.QuickCity.Shape
{
    public interface IShape
    {
        bool[,] GenerateShapeMatrix(float sizeMultiple);

        // default implements

        Dictionary<(int x, int y), EdgeOutsideDir> Edges(in bool[,] _matrix)
        {
            int size_x = _matrix.GetLength(0);
            int size_y = _matrix.GetLength(1);

            var result = new Dictionary<(int x, int y), EdgeOutsideDir>(capacity: size_x * size_y);

            for (int x = 0; x < size_x; x++)
            {
                for (int y = 0; y < size_y; y++)
                {
                    if ((x + 1) >= size_x
                        || (y + 1) >= size_y
                        || (x - 1) < 0
                        || (y - 1) < 0)
                        continue;

                    var direction = IsEdge(_matrix, (x, y));

                    if (direction != EdgeOutsideDir.NotEdge)
                        result.Add((x, y), direction);
                }
            }
            return result;
        }
        EdgeOutsideDir IsEdge(in bool[,] map, (int x, int y) coord)
        {
            (int x, int y) = coord;

            if (!map[x, y])
                return EdgeOutsideDir.NotEdge;

            bool left = map[x - 1, y];
            bool right = map[x + 1, y];
            bool up = map[x, y - 1];
            bool down = map[x, y + 1];

            if (left && right && up && down)
                return EdgeOutsideDir.NotEdge;

            if (left && !right)
                return EdgeOutsideDir.Right;

            else if (!left && right)
                return EdgeOutsideDir.Left;

            else if (up && !down)
                return EdgeOutsideDir.Down;

            else if (!up && down)
                return EdgeOutsideDir.Up;

            return EdgeOutsideDir.NotEdge;
        }

        bool[,] ScaleMatrix(in bool[,] matrix, float sizeMultiple)
        {
            if (sizeMultiple <= 0)
                throw new ArgumentException();

            if (sizeMultiple == 1)
                return matrix;

            int originalWidth = matrix.GetLength(0);
            int originalHeight = matrix.GetLength(1);

            int newWidth = (int)(originalWidth * sizeMultiple);
            int newHeight = (int)(originalHeight * sizeMultiple);

            bool[,] outputMatrix = new bool[newWidth, newHeight];

            for (int i = 0; i < newWidth; i++)
            {
                for (int j = 0; j < newHeight; j++)
                {
                    int x = (int)(i / sizeMultiple);
                    int y = (int)(j / sizeMultiple);

                    // 使用最近邻插值法进行缩放
                    outputMatrix[i, j] = matrix[x, y];
                }
            }
            return outputMatrix;
        }
        bool[,] CropMatrix(in bool[,] matrix)
        {
            int width = matrix.GetLength(0);
            int height = matrix.GetLength(1);

            int minX = width;
            int minY = height;
            int maxX = 0;
            int maxY = 0;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (matrix[x, y])
                    {
                        minX = Math.Min(minX, x);
                        minY = Math.Min(minY, y);
                        maxX = Math.Max(maxX, x);
                        maxY = Math.Max(maxY, y);
                    }
                }
            }

            int croppedWidth = maxX - minX + 1;
            int croppedHeight = maxY - minY + 1;

            bool[,] outputMatrix = new bool[croppedWidth, croppedHeight];

            for (int x = 0; x < croppedWidth; x++)
            {
                for (int y = 0; y < croppedHeight; y++)
                {
                    outputMatrix[x, y] = matrix[minX + x, minY + y];
                }
            }
            return outputMatrix;
        }

        // static methods

        static (int x, int y) SizeOf(bool[,] matrix)
        {
            return (matrix.GetLength(0), matrix.GetLength(1));
        }
    }

    public enum EdgeOutsideDir
    {
        NotEdge, Up, Down, Left, Right
    }
}
