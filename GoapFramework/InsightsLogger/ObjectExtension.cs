using Newtonsoft.Json;

namespace InsightsLogger
{
    public static class ObjectExtensions
    {
        public static string ToJsonString(this object value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }

}
