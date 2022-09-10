namespace InsightsLogger
{
    public interface ISimpleLogger
    {
        void LogDebug(string log);
        void LogInfo(string log);
        void LogError(string log);
    }
}
