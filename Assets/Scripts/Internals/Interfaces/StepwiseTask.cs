using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

        private int _targetStepCount  = 1;
        private int _currentStepCount = 0;
        private bool IsTimeToReport => 
            tick > maxTick || _targetStepCount - _currentStepCount <= tick;

        protected StepwiseTask(int maxTick) 
        { 
            this.maxTick = maxTick; 
        }       
        protected IEnumerator ForStep(Action body, int stepcnt, bool cond = true, Action inc = null)
        {
            _targetStepCount = stepcnt;

            for(; cond == true; inc?.Invoke())
            {
                tick++;
                _currentStepCount++;

                try
                {
                    body.Invoke();
                }
                catch (ContinueException) { continue; }
                catch (BreakException) { break; }

                if (IsTimeToReport)
                {
                    tick = 0;
                    yield return null;
                }
            }
            yield break;
        }
        protected IEnumerator ForeachStep<T>(Action<T> body, IEnumerable<T> iter, int stepcnt = -1)
        {
            _targetStepCount = stepcnt == -1 ? iter.Count() : stepcnt;

            foreach(T item in iter)
            {
                tick++;
                _currentStepCount++;

                try
                {
                    body.Invoke(item);
                }
                catch (ContinueException) { continue; }
                catch (BreakException)    { break; }

                if (IsTimeToReport)
                {
                    tick = 0;
                    yield return null;
                }
            }
            yield break;
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
