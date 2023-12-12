using System;
using System.Collections;
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

        protected int _targetStepCount  = 1;
        protected int _currentStepCount = 0;
        protected bool IsTimeToReport()
        {
            return tick++ > maxTick || _targetStepCount - _currentStepCount <= tick;
        }

        protected StepwiseTask(int maxTick) 
        { 
            this.maxTick = maxTick; 
        }       
        protected IEnumerator ForStep(Action body, int targetStepCount, Action pre = null, Func<bool> endCond = null, Action step = null)
        {
            _targetStepCount = targetStepCount;

            for(pre?.Invoke(); endCond == null || endCond.Invoke(); step?.Invoke())
            {
                _currentStepCount++;

                try
                {
                    body.Invoke();
                }
                catch (ContinueException) { continue; }
                catch (BreakException) { break; }

                if (tick++ > maxTick || _targetStepCount - _currentStepCount <= tick)
                {
                    tick = 0;
                    yield return null;
                }
            }
        }
        protected IEnumerator ForeachStep<T>(Action<T> body, IEnumerable<T> iter, int stepcnt)
        {
            _targetStepCount = stepcnt;

            foreach(T item in iter)
            {
                _currentStepCount++;

                try
                {
                    body.Invoke(item);
                }
                catch (ContinueException) { continue; }
                catch (BreakException)    { break; }

                if (tick++ > maxTick || _targetStepCount - _currentStepCount <= tick)
                {
                    tick = 0;
                    yield return null;
                }
            }
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
