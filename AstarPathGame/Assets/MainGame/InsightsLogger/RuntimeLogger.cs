using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InsightsLogger
{
    public class ListWrapper<T>
    {
        public List<T> collection;
        private ListWrapper(params T[] list)
        {
            collection = list.ToList();
        }
        private ListWrapper(List<T> list)
        {
            collection = list;
        }

        public static ListWrapper<T> Wrap(params T[] list) => new ListWrapper<T>(list);

        public static ListWrapper<T> Wrap(List<T> list) => new ListWrapper<T>(list);
    }

    public class RuntimeLogger : MonoBehaviour
    {
        public string logsFolderPath = "";
        public bool debugGameAtRunTime;
        public bool breakPerFrame;
        public bool printDebugMessages;

        private static ConcurrentQueue<string> _debugMessages = new ConcurrentQueue<string>();

        static RuntimeLogger _instance;
        public static bool DebuggerIsOn => Instance.debugGameAtRunTime;
        public static bool BreakPerFrame => Instance.breakPerFrame;

        public static RuntimeLogger Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<RuntimeLogger>();
                }
                return _instance;
            }
        }

        private Action<string, string, string> _processDebugMessages;

        public static void AddDebugListener(Action<string, string, string> listener)
        {
            Instance._processDebugMessages += listener;
        }

        public static void RemoveDebugListener(Action<string, string, string> listener)
        {
            Instance._processDebugMessages -= listener;
        }

        private static string CreateDebugMessage(CorrelationVector correlationVector, string eventName, string message, object payload)
        {
            CorrelationVector.correlationVector.Increment();
            return $"{correlationVector.Vector}> {eventName} | {message}" + $"\n{payload.ToJsonString()}";
        }

        public static void LogDebug(string eventName, string message, object payload)
        {
            var msg = CreateDebugMessage(
                CorrelationVector.correlationVector,
                eventName, message, payload);
            if (Instance.printDebugMessages) Debug.Log(msg);

            // File.WriteAllText($"{Instance.logsFolderPath}/{CorrelationVector.correlationVector}", msg);

            if (Instance.debugGameAtRunTime) _debugMessages.Enqueue(msg);
        }

        public static void LogError(string eventName, string message, object payload)
        {
            var msg = CreateDebugMessage(
                CorrelationVector.correlationVector,
                eventName, message, payload);
            if (Instance.printDebugMessages) Debug.LogError(msg);

            // File.WriteAllText($"{Instance.logsFolderPath}/{CorrelationVector.correlationVector}", msg);

            if (Instance.debugGameAtRunTime) _debugMessages.Enqueue(msg);
        }

        public static void LogException(Exception exception, string eventName, string message, object payload)
        {
            LogError(eventName, message, payload);
            if (Instance.printDebugMessages) Debug.LogException(exception);

            var exceptionString = exception.ToJsonString();
            // File.WriteAllText($"{Instance.logsFolderPath}/{CorrelationVector.correlationVector}", exceptionString);

            if (Instance.debugGameAtRunTime) _debugMessages.Enqueue(exceptionString);
        }

        public static void LogWarning(string eventName, string message, object payload)
        {
            var msg = CreateDebugMessage(
                CorrelationVector.correlationVector,
                eventName, message, payload);
            if (Instance.printDebugMessages) Debug.LogWarning(msg);

            // File.WriteAllText($"{Instance.logsFolderPath}/{CorrelationVector.correlationVector}", msg);

            if (Instance.debugGameAtRunTime) _debugMessages.Enqueue(msg);
        }

        private void Awake()
        {
            var _ = Instance; // this will create the instance as soon as the game is started
        }
        private void Update()
        {
            if (!debugGameAtRunTime) return;

            while (_debugMessages.TryDequeue(out string debugMsg))
            {
                var msg = debugMsg.Split('\n')[0];
                var eventNameAndMessage = msg.Split('>')[1].Trim();
                var eventName = eventNameAndMessage.Split('|')[0].Trim();
                var eventMessage = eventNameAndMessage.Split('|')[1].Trim();
                var payload = debugMsg.Split('\n')[1];

                Instance._processDebugMessages?.Invoke(eventName, eventMessage, payload);
            }
        }
    }
}