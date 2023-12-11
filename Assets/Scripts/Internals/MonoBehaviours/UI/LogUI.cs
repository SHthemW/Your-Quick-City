using System;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

namespace Yours.QuickCity.Internal
{
    internal sealed class LogUI : MonoBehaviour
    {
        [field: SerializeField]
        private Text LogText { get; set; }

        private void Awake()
        {
            _instance = this;
        }
        private static LogUI _instance;
        

        internal static void AppendLog(string message)
        {
            _instance.LogText.text += "\n" + message;
        }

        private static string _staticText;
        private static Func<float> _percentGetter;
        internal static void AppendDynamicPercent(Func<float> percentGetter)
        {
            _percentGetter = percentGetter;
            _staticText = _instance.LogText.text;
            _instance.LogText.text = _staticText + $"({0.0}%)";
        }
        internal static void UpdateDynamicPercent()
        {
            if (_percentGetter == null)
                return;
            string percent = Math.Round(_percentGetter.Invoke(), 1).ToString();
            _instance.LogText.text = _staticText + $"({percent}%)";
        }
        internal static void EndDynamicPart()
        {
            _staticText = _instance.LogText.text;
            _percentGetter = null;
        }

        private void Update()
        {
            UpdateDynamicPercent();
        }
    }
}
