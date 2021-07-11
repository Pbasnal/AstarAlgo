using System;
using System.Diagnostics;

namespace TestHelpers
{
    public static class Timer
    {
        public static TimeSpan CaptureExecutionTime<T>(
            Func<T> actionToTime, out T output)
        {
            var timer = Stopwatch.StartNew();
            output = actionToTime();
            timer.Stop();

            return timer.Elapsed;
        }
    }
}