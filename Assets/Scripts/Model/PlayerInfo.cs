using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Info")]
public class PlayerInfo : ScriptableObject
{
    [JsonProperty("ID")] public string ID;
    [JsonProperty("MatchID")] public string MatchID;
    [JsonProperty("MatchPlayerID")] public string MatchPlayerID;
    [JsonProperty("HeroName")] public string HeroName;
}