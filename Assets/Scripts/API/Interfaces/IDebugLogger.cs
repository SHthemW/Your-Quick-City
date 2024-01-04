using System;
using System.Collections.Generic;

namespace Yours.QuickCity.Internal
{
    public interface IDebugLogger
    {
        void Add(string message) => UnityEngine.Debug.Log(message);

        void AddDynamicPerc(Func<float> percent, Func<bool> until);

        void Clear();
    }
}
