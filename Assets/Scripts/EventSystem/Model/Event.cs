using System.Collections.Generic;
using Newtonsoft.Json;

namespace EventSystem.Model
{
    public class Event
    {
        [JsonProperty("_type")] public string Type { get; set; }

        [JsonProperty("_info")] public Dictionary<string, string> Info { get; set; }
    }
}