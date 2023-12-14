using System;
using System.Collections.Generic;
using UnityEngine;

namespace Yours.QuickCity.Shape
{ 
    public interface IShape
    {
        bool[,] RandomShapeMatrix { get; }
    }
}
