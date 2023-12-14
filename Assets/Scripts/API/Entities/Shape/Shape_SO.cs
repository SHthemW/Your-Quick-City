using System.Text;
using UnityEngine;

namespace Yours.QuickCity.Shape
{
    public abstract class Shape_SO : ScriptableObject, IShape
    {       
        protected bool[,] _matrix = null;
        protected abstract void GenerateOnMatrix();

        bool[,] IShape.RandomShapeMatrix
        {
            get
            {
                GenerateOnMatrix();
                return _matrix;
            }
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

        [ContextMenu("Generate and Print Debug Graph")]
        private void GenerateAndPrintOnConsole()
        {
            GenerateOnMatrix();
            Debug.Log(ShapeDebugMsg);
        }

        [ContextMenu("Generate and Print Debug Graph * 3")]
        private void GenerateAndPrintOnConsoleFor3Times()
        {
            StringBuilder msg = new();

            for (int i = 0; i < 3; i++)
            {
                GenerateOnMatrix();
                msg.AppendLine(ShapeDebugMsg);
            }

            Debug.Log(msg.ToString());
        }
    }
}