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

        [SerializeField]
        private float _evenness;

        public bool[,] GenerateMatrix()
        {
            bool[,] matrix = new bool[_size_x, _size_y] ;

            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    if (x == 0 && y == 0)
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
