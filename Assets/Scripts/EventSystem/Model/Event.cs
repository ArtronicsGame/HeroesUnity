using System.Collections.Generic;
using Newtonsoft.Json;

namespace EventSystem.Model
{
    public class Event
    {
        [JsonProperty("_type")] public string Type { get; set; }

        [JsonProperty("_info")] public Dictionary<string, string> Info { get; set; }

        public Event()
        {
            
        }

        public Event(UDPPacket packet)
        {
            if (packet.type == 'U')
            {
                Type = "MatchUpdate";
                Info = new Dictionary<string, string>()
                {
                    {"X", packet.fList[0].ToString()},
                    {"Y", packet.fList[1].ToString()},
                    {"Angle", packet.fList[2].ToString()},
                    {"Id", packet.str}
                };
            }
        }
    }
}