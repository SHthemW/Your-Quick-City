using System;
using System.Collections.Generic;

namespace Yours.QuickCity.Shape
{
    public enum EdgeOutsideDir
    {
        NotEdge, Up, Down, Left, Right
    }

    public interface IEdgeJudger
    {
        EdgeOutsideDir Judge(bool[,] map, (int x, int y) coord);
    }
}
