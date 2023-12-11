using System;
using System.Collections;
using System.Collections.Generic;

namespace Yours.QuickCity.Internal
{
    internal interface ITask<TResult>
    {      
        int tick { get; set; }
        int maxTick { get; }
        TResult Result { get; }

        float FinishedPercent();
        bool Completed();
    }
}
