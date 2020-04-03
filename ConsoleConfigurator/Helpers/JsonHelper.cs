using Newtonsoft.Json;
using System.Collections.Generic;


namespace ConsoleConfigurator.Helpers
{
    static class JsonHelper
    {
        public static string ToJson(Dictionary<string,string> dictionary)
        {
            return JsonConvert.SerializeObject(dictionary, Formatting.Indented);
        }

        public static Dictionary<string,string> ToDictionary(string json)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }
    }
}
