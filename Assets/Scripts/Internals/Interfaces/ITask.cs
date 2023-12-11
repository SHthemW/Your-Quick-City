using System;
using System.Collections;
using System.Collections.Generic;

namespace Yours.QuickCity.Internal
{
    internal interface ITask<TResult>
    {      
        int Trick { get; set; }
        int MaxTrick { get; }
        TResult Result { get; }

        float FinishedPercent();
        bool Completed();
    }
}
