using System;
using InsightsLogger;

namespace ConsoleInterface
{
    public enum LogLevel
    {
        Debug = 0,
        Info,
        Warning,
        Error,
        None
    }

    public class ConsoleLogger : ISimpleLogger
    {
        private int logLevel;

        public ConsoleLogger()
        {
            logLevel = (int)LogLevel.Debug;
        }

        public void SetLogLevel(LogLevel logLevel)
        {
            this.logLevel = (int)logLevel;
        }

        public void LogDebug(string log)
        {
            if (logLevel <= (int)LogLevel.Debug)
            {
                Log(LogLevel.Debug, log, ConsoleColor.DarkYellow);
            }
        }

        public void LogInfo(string log)
        {
            if (logLevel <= (int)LogLevel.Info)
            {
                Log(LogLevel.Info, log, ConsoleColor.Blue);
            }
        }

        public void LogError(string log)
        {
            if (logLevel <= (int)LogLevel.Error)
            {
                Log(LogLevel.Error, log, ConsoleColor.Red);
            }
        }

        private static void Log(LogLevel logLevel, string log, ConsoleColor color)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine($"[{logLevel}] {log}");
            Console.ForegroundColor = defaultColor;
        }
    }
}