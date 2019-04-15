using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public enum Equipments
{
    NONE = -1
}

[CreateAssetMenu(fileName = "Player Info")]
public class PlayerInfo : ScriptableObject
{
    public PlayerData PlayerData = new PlayerData();
        
    public int MatchPlayerID;
    public string HeroName;
}

public class PlayerData{
    [JsonProperty("_id")] public string ID;
    [JsonProperty("username")] public string Username;
    [JsonProperty("playerClanPosition")] public string PlayerClanPosition;
    [JsonProperty("matchPort")] public int MatchPort;
    [JsonProperty("matchID")] public string MatchIP;
    [JsonProperty("trophies")] public int Trophies;
    [JsonProperty("coins")] public int Coins;
    [JsonProperty("experience")] public int Experience;
    [JsonProperty("currentHero")] public string CurrentHero;
    [JsonProperty("heroesProperties")] public Dictionary<string, HeroInfo> HeroProperties;
}

public class HeroInfo
{
    [JsonProperty("isUnlocked")] public bool IsUnlocked;
    [JsonProperty("experience")] public int Experience;
    [JsonProperty("trophies")] public int Trophies;
    [JsonProperty("basicInfo")] public BasicInfo BasicInfo;
}

public class BasicInfo
{
    [JsonProperty("level")] public int Level;
    [JsonProperty("equip1")] public Equipments Equip1;
    [JsonProperty("equip2")] public Equipments Equip2;
}