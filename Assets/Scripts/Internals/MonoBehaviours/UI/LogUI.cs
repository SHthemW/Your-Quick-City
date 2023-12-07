using System;
using UnityEngine;
using UnityEngine.UI;

namespace Yours.QuickCity.Internal
{
    internal enum LogLevel
    {
        Normal, Done, Error
    }

    internal sealed class LogUI : MonoBehaviour
    {
        [field: SerializeField]
        private Text LogText { get; set; }

        private void Awake()
        {
            _instance = this;
        }

        private static LogUI _instance;
        internal static void Log(string message, LogLevel level = LogLevel.Normal)
        {
            _instance.LogText.text = message;

            _instance.LogText.color = level switch 
            { 
                LogLevel.Normal => Color.white,
                LogLevel.Done   => Color.green,
                LogLevel.Error  => Color.red,

                _ => throw new NotImplementedException(),
            };
        }
        internal static void AppendLog(string message)
        {
            _instance.LogText.text += "\n" + message;
        }
    }
}
