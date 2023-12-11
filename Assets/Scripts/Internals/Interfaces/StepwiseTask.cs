using System;
using System.Collections.Generic;

namespace Yours.QuickCity.Internal
{
    internal abstract class StepwiseTask<TResult> : ITask<TResult>
    {       
        public int Trick { get; set; }
        public abstract int MaxTrick { get; }
        public TResult Result { get; protected set; }

        public bool Completed()
        {
            return _currentStepCount >= _targetStepCount;
        }     
        public float FinishedPercent()
        {
            return (float)_currentStepCount / _targetStepCount * 100;
        }

        protected int _targetStepCount { get; set; } = 1;
        protected int _currentStepCount { get; set; } = 0;
        protected bool IsTimeToReport()
        {
            return Trick++ > 1000 || _targetStepCount - _currentStepCount <= Trick;
        }
    }
}
