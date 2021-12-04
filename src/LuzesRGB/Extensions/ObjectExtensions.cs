using Newtonsoft.Json;

namespace System
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object input) =>
            JsonConvert.SerializeObject(input);
    }
}
