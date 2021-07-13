using System;
using System.Diagnostics;
using UnityEngine;

namespace InsightsLogger
{
    public class ExecutionTimer : MonoBehaviour
    {
        public static TimeSpan Time<T>(Func<T> actionToTime, out T output)
        {
            var timer = Stopwatch.StartNew();
            output = actionToTime();
            timer.Stop();
            return timer.Elapsed;
        }

        public static TimeSpan Time(Action actionToTime)
        {
            var timer = Stopwatch.StartNew();
            actionToTime();
            timer.Stop();
            return timer.Elapsed;
        }
    }
}
