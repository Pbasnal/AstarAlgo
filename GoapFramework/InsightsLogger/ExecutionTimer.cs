using System;
using System.Diagnostics;

namespace InsightsLogger
{
    public class ExecutionTimer
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

        public static long TimeTicks(Action actionToTime)
        {
            var startingTicks = DateTime.UtcNow.Ticks;
            actionToTime();            
            return DateTime.UtcNow.Ticks - startingTicks;
        }

        public static long TimeTicks(Action<int> actionToTime, int arg)
        {
            var startingTicks = DateTime.UtcNow.Ticks;
            actionToTime(arg);
            return DateTime.UtcNow.Ticks - startingTicks;
        }
    }
}
