using System;
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

        protected virtual void ScaleMatrix(float sizeMultiple)
        {
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
}