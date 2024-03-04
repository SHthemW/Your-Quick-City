using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Yours.QuickCity.Internal;

namespace Yours.QuickCity.Shape
{
    public abstract class Shape_SO : ScriptableObject, IShape
    {
        public abstract bool[,] GenerateShapeMatrix(float sizeMultiple);

        #region Test

        [Header("<Test Generare Arguments>")]

        [SerializeField]
        private float _argSizeMultiple = 1f;

        [ContextMenu("Test Generate")]
        private void GenerateAndPrintOnConsole()
        {
            StringBuilder msg = new();

            var matrix = GenerateShapeMatrix(sizeMultiple: _argSizeMultiple);

            msg.Append("generate: \n" + GetConsoleGraphByMatrix(matrix));
            Debug.Log(msg.ToString());
        }
        private string GetConsoleGraphByMatrix(in bool[,] matrix)
        {
            StringBuilder graphMsg = new("graph: \n");

            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    graphMsg.Append((matrix[x, y] ? "■" : "□") + "  ");
                }
                graphMsg.Append("\n");
            }

            return graphMsg.ToString();
        }

        #endregion
    }
}