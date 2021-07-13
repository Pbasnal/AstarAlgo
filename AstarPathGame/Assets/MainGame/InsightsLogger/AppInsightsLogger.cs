//using InsightsLogger.Config;
//using Microsoft.ApplicationInsights;
//using System;
//using System.Collections.Generic;
//using System.Threading;

//namespace InsightsLogger
//{
//    public static class AppInsightsLogger
//    {
//        private static Guid _appId;
//        private static TelemetryClient _logStorage;

//        public static void InitializeLogger(LoggerConfig config)
//        {
//            _logStorage = new TelemetryClient();

//            _logStorage.Context.InstrumentationKey = config.InstrumentationKey;
//            _logStorage.InstrumentationKey = config.InstrumentationKey;

//            _logStorage.Context.User.Id = Environment.UserName;
//            _logStorage.Context.Session.Id = Guid.NewGuid().ToString();
//            _logStorage.Context.Device.OperatingSystem = Environment.OSVersion.ToString();

//            if (!Guid.TryParse(config.InstrumentationKey, out _appId))
//                throw new Exception("Please provide a guid as AppId");
//        }

//        public static void Log(CorrelationVector correlationVector, LogPayload logPayload, LogType logType)
//        {
//            return;

//            if (!ContextValidations(correlationVector, logPayload))
//                throw new ContextValidationFailedException();

//            var properties = new Dictionary<string, string>();

//            properties.Add("AppId", _appId.ToString());
//            properties.Add("CorrelationVector", correlationVector.Vector);
//            properties.Add("Message", logPayload.Message);
//            properties.Add("Payload", logPayload.ToJsonString());
//            properties.Add("PayloadNamespace", logPayload.GetType().FullName);
//            properties.Add("LogType", logType.ToString());
//            properties.Add("ThreadId", Thread.CurrentThread.ManagedThreadId.ToString());
//            properties.Add("TimeSinceLastEventInMs", correlationVector.TimeSinceLastEventInMs.ToString());
//            properties.Add("Timestamp", logPayload.Timestamp.ToLongTimeString());

//            _logStorage.TrackTrace(logPayload.EventName, Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Information, properties);

//            correlationVector.Increment();
//        }

//        public static void LogDebug(CorrelationVector correlationVector, LogPayload logPayload)
//        {
//            Log(correlationVector, logPayload, LogType.Debug);
//        }

//        public static void LogWarning(CorrelationVector correlationVector, LogPayload logPayload)
//        {
//            Log(correlationVector, logPayload, LogType.Debug);
//        }

//        public static void LogError(CorrelationVector correlationVector, LogPayload logPayload)
//        {
//            Log(correlationVector, logPayload, LogType.Debug);
//        }

//        public static void LogException(CorrelationVector correlationVector, Exception exception, LogPayload logPayload)
//        {
//            var properties = new Dictionary<string, string>();
//            properties.Add("AppId", _appId.ToString());
//            properties.Add("CorrelationVector", correlationVector.Vector);
//            properties.Add("Payload", logPayload.ToJsonString());
//            _logStorage.TrackException(exception, properties);

//            Console.Write("Exception : ");
//            Console.WriteLine(exception.Message);

//            correlationVector.Increment();
//        }

//        private static bool ContextValidations(CorrelationVector correlationVector, LogPayload logPayload)
//        {
//            bool valid = correlationVector != null;
//            valid &= _appId != Guid.Empty;
//            valid &= correlationVector.OperationId != Guid.Empty;
//            valid &= logPayload != null;
//            valid &= (logPayload.Message != null && logPayload.Message != string.Empty);

//            return valid;
//        }
//    }
//}
