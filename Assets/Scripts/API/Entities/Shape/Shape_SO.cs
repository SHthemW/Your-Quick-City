using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Yours.QuickCity.Shape
{
    public abstract class Shape_SO : ScriptableObject, IShape
    {       
        protected bool[,] _matrix = null;
        protected abstract void GenerateOnMatrix();

        bool[,] IShape.GenerateShapeMatrix(float sizeMultiple)
        {
            // generate
            GenerateOnMatrix();

            // scale
            ScaleMatrix(sizeMultiple);

            // corp
            CropMatrix();

            return _matrix;
        }

        protected virtual string ShapeDebugMsg
        {
            get
            {
                StringBuilder graphMsg = new("graph: \n");

                for (int x = 0; x < _matrix.GetLength(0); x++)
                {
                    for (int y = 0; y < _matrix.GetLength(1); y++)
                    {
                        graphMsg.Append((_matrix[x, y] ? "■" : "□") + "  ");
                    }
                    graphMsg.Append("\n");
                }
                return graphMsg.ToString();
            }
        }
        protected virtual string StatsDebugMsg 
        { 
            get; 
        }

        protected Dictionary<(int x, int y), EdgeOutsideDir> Edges
        {
            get
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
        }
        protected virtual EdgeOutsideDir IsEdge(bool[,] map, (int x, int y) coord)
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

        protected virtual void ScaleMatrix(float sizeMultiple)
        {
            if (sizeMultiple <= 0)
                throw new ArgumentException();

            if (sizeMultiple == 1)
                return;

            int originalWidth = _matrix.GetLength(0);
            int originalHeight = _matrix.GetLength(1);

            int newWidth =  (int)(originalWidth * sizeMultiple);
            int newHeight = (int)(originalHeight * sizeMultiple);

            bool[,] outputMatrix = new bool[newWidth, newHeight];

            for (int i = 0; i < newWidth; i++)
            {
                for (int j = 0; j < newHeight; j++)
                {
                    int x = (int)(i / sizeMultiple);
                    int y = (int)(j / sizeMultiple);

                    // 使用最近邻插值法进行缩放
                    outputMatrix[i, j] = _matrix[x, y];
                }
            }
            _matrix = outputMatrix;
        }
        protected virtual void CropMatrix()
        {
            int width  = _matrix.GetLength(0);
            int height = _matrix.GetLength(1);

            int minX = width;
            int minY = height;
            int maxX = 0;
            int maxY = 0;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (_matrix[x, y])
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
                    outputMatrix[x, y] = _matrix[minX + x, minY + y];
                }
            }
            _matrix = outputMatrix;
        }

        [Header("Test")]

        [SerializeField] float _testScaleMultiple = 1;

        [ContextMenu("Test Generate")]
        private void GenerateAndPrintOnConsole()
        {
            StringBuilder msg = new();

            GenerateOnMatrix();
            msg.Append("generate: \n" + ShapeDebugMsg);

            ScaleMatrix(_testScaleMultiple);
            msg.Append("scaled: \n" + ShapeDebugMsg);

            CropMatrix();
            msg.Append("corped: \n" + ShapeDebugMsg);

            msg.Append("stats: \n" + StatsDebugMsg);

            Debug.Log(msg.ToString());
        }

    }

    public enum EdgeOutsideDir
    {
        NotEdge, Up, Down, Left, Right
    }
}