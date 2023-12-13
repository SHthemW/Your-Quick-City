using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Yours.QuickCity
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
        private float _maxEdgeConcavePercent;

        [SerializeField, Range(0, 1)]
        private float _maxEdgeConvexityPercent;

        [Header("Advanced")]

        [SerializeField, Range(1, 10)]
        private int _matrixAllocSizeMultiplier = 3;

        public bool[,] GenerateMatrix()
        {
            bool[,] matrix = new bool[_size_x * _matrixAllocSizeMultiplier, _size_y * _matrixAllocSizeMultiplier];

            int startX = ((_matrixAllocSizeMultiplier - 1) / 2) * _size_x;
            int startY = ((_matrixAllocSizeMultiplier - 1) / 2) * _size_y;

            int endX = startX + _size_x;
            int endY = startY + _size_y;

            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    if (x == startX && y == startY)
                        matrix[x, y] = false;
                    else
                        matrix[x, y] = true;
                }
            }
            return matrix;
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
    }
}
