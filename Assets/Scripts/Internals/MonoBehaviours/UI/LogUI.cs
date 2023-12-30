using System;
using UnityEngine;
using UnityEngine.UI;
using Yours.QuickCity.Internal;

namespace Yours.QuickCity
{
    public sealed class LogUI : MonoBehaviour
    {
        [field: SerializeField]
        private Text LogText { get; set; }

        private const int DONE_PERCENT = 98;

        private void Awake()
        {
            _instance = this;
        }
        private static LogUI _instance;
        
        public static void Clear()
        {
            _instance.LogText.text = string.Empty;
        }

        internal static void AppendLog(string message)
        {
            _instance.LogText.text += $"{Style.Gray}\n{message}{Style.End}";
        }

        private static string _staticText;
        private static Func<float> _percentGetter;
        internal static void AppendDynamicPercent(Func<float> percentGetter)
        {
            _percentGetter = percentGetter;
            _staticText = _instance.LogText.text;
            _instance.LogText.text = _staticText + $"({0.0}%)";
        }
        private static void UpdateDynamicPercent(float percentValue)
        {
            string percent = Math.Round(percentValue, 1).ToString();

            _instance.LogText.text = _staticText + (percentValue >= DONE_PERCENT ? "(done.)" : $"({percent}%)");
        }
        internal static void EndDynamicPart()
        {
            _staticText = _instance.LogText.text;
            _percentGetter = null;
        }

        private void Update()
        {
            if (_percentGetter == null)
                return;
            UpdateDynamicPercent(_percentGetter.Invoke());
        }
    }
}
