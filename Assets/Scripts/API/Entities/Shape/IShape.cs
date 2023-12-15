using System;
using System.Collections.Generic;
using UnityEngine;

namespace Yours.QuickCity.Shape
{ 
    public interface IShape
    {
        bool[,] GenerateShapeMatrix(float sizeMultiple);

        static (int x, int y) SizeOf(bool[,] matrix)
        {
            return (matrix.GetLength(0), matrix.GetLength(1));
        }
    }
}
