using System;
using System.Collections.Generic;

namespace Yours.QuickCity.Internal
{
    internal abstract class StepwiseTask : ITask
    {
        public int tick { get; set; }
        public abstract int maxTick { get; }

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
            return tick++ > maxTick || _targetStepCount - _currentStepCount <= tick;
        }
    }

    internal abstract class StepwiseTask<TResult> : StepwiseTask, ITask<TResult>
    {
        public TResult Result { get; protected set; }
    }
}
