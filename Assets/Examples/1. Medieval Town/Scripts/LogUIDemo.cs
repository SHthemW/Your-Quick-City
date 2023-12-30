using System;
using UnityEngine;
using UnityEngine.UI;
using Yours.QuickCity.Internal;

namespace Yours.QuickCity
{
    internal sealed class LogUIDemo : MonoBehaviour, IDebugLogger
    {
        [field: SerializeField]
        private Text LogText { get; set; }

        private const int DONE_PERCENT = 98;
        private string _textTemp;

        private Func<float> _percentGetter;
        private Func<bool>  _endCondition;

        void IDebugLogger.Add(string message)
        {
            LogText.text += $"<color=#DCDCDC>\n{message}</color>";
        }
       
        void IDebugLogger.AddDynamicPerc(Func<float> percentGetter, Func<bool> until)
        {
            _percentGetter = percentGetter;
            _endCondition  = until;

            _textTemp = LogText.text;
            LogText.text = _textTemp + $"({0.0}%)";
        }

        void IDebugLogger.Clear()
        {
            LogText.text = string.Empty;
        }
       
        
        private void Update()
        {
            if (_percentGetter == null || _endCondition == null)
                return;

            if (_endCondition())
            {
                EndDynamicPart();
                return;
            }

            UpdateDynamicPercent(_percentGetter.Invoke());
        }

        private void UpdateDynamicPercent(float percentValue)
        {
            string percent = Math.Round(percentValue, 1).ToString();

            LogText.text = _textTemp + (percentValue >= DONE_PERCENT ? "(done.)" : $"({percent}%)");
        }

        private void EndDynamicPart()
        {
            _textTemp = LogText.text;
            _percentGetter = null;
        }

    }
}
