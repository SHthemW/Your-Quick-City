using System;
using System.Collections.Generic;
using UnityEngine;

namespace Yours.QuickCity
{ 
    public interface IShape
    {
        bool[,] GenerateMatrix();
    }
}
