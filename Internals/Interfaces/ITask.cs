using System;
using System.Collections;
using System.Collections.Generic;

namespace Yours.QuickCity.Internal
{
    internal interface ITask
    {
        int tick { get; set; }
        int maxTick { get; }

        float FinishedPercent();
        bool Completed();
    }

    internal interface ITask<TResult> : ITask
    {      
        TResult Result { get; }
    }
}
