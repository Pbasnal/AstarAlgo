using System;
using System.Collections.Generic;

namespace InsightsLogger
{
    public class LogPayload
    {
        public LogPayload()
        {
            Timestamp = DateTime.UtcNow;
        }

        public string EventName { get; set; }
        public string Message { get; set; }
        public object Payload { get; set; }
        public DateTime Timestamp { get; set; }

        public static object AddMultiplePayloads(params object[] payloads)
        {
            IList<object> multiplePayloads = new List<object>();

            foreach (var payload in payloads)
            {
                multiplePayloads.Add(payload);
            }

            return multiplePayloads;
        }
    }
}