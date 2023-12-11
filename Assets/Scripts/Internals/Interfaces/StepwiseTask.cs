using System;
using System.Collections.Generic;

namespace Yours.QuickCity.Internal
{
    internal abstract class StepwiseTask : ITask
    {
        public int tick { get; set; }
        public int maxTick { get; }

        public bool Completed()
        {
            return _currentStepCount >= _targetStepCount;
        }
        public float FinishedPercent()
        {
            return (float)_currentStepCount / _targetStepCount * 100;
        }

        protected StepwiseTask(int maxTick) 
        { 
            this.maxTick = maxTick; 
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
        protected StepwiseTask(int maxTick) : base(maxTick)
        {
        }
        public TResult Result { get; protected set; }
    }
}
