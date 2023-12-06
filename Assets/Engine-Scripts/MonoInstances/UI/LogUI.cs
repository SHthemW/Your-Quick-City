using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Instances.UI
{
    public enum LogLevel
    {
        Normal, Done, Error
    }

    public sealed class LogUI : MonoBehaviour
    {
        [field: SerializeField]
        private Text LogText { get; set; }

        private void Awake()
        {
            _instance = this;
        }

        private static LogUI _instance;
        public static void Log(string message, LogLevel level = LogLevel.Normal)
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
        public static void AppendLog(string message)
        {
            _instance.LogText.text += "\n" + message;
        }
    }
}
