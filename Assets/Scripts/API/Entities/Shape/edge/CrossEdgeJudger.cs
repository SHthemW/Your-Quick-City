using System;
using System.Collections.Generic;

namespace Yours.QuickCity.Shape
{
    public sealed class CrossEdgeJudger : IEdgeJudger
    {
        EdgeOutsideDir IEdgeJudger.Judge(bool[,] map, (int x, int y) coord)
        {
            (int x, int y) = coord;

            if (!map[x, y])
                return EdgeOutsideDir.NotEdge;

            bool left  = map[x - 1, y];
            bool right = map[x + 1, y];
            bool up    = map[x, y - 1];
            bool down  = map[x, y + 1];

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
    }
}
