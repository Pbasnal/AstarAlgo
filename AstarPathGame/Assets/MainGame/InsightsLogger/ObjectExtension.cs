using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace InsightsLogger
{
    public static class ObjectExtensions
    {
        public static string ToJsonString(this object value)
        {
            return JsonUtility.ToJson(value);
        }
    }

}
