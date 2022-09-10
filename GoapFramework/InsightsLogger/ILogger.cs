using System;

namespace InsightsLogger
{
    public interface ILogger
    {
        void LogDebug(CorrelationVector correlationVector, LogPayload logPayload);
        void LogWarning(CorrelationVector correlationVector, LogPayload logPayload);
        void LogError(CorrelationVector correlationVector, LogPayload logPayload);
        void LogException(CorrelationVector correlationVector, Exception exception, LogPayload logPayload);
    }
}
