using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Event = EventSystem.Model.Event;

public class PlayerManager : EventBehaviour
{
    public PlayerInfo playerInfo;
    
    private MessageHandler _messageHandler;

    new void Start()
    {
        base.Start();
        GameObject connectionManager = GameObject.Find("ConnectionManager");
        if (connectionManager != null)
        {
            _messageHandler = connectionManager.GetComponentInChildren<MessageHandler>();
        }

        if (PlayerPrefs.HasKey("id"))
        {
            _messageHandler.GetPlayer(PlayerPrefs.GetString("id"));
        }
        else
        {
            _messageHandler.NewPlayer("Soroush");
        }
    }

    protected override void OnEvent(Event e)
    {
        switch (e.Type)
        {
            case "NewPlayerResp":
                if ((Status) int.Parse(e.Info["status"]) == Status.STATUS_OK)
                {
                    PlayerPrefs.SetString("id", e.Info["userId"]);
                    _messageHandler.GetPlayer(e.Info["userId"]);
                }
                break;
            case "GetPlayerResp":
                if ((Status) int.Parse(e.Info["status"]) == Status.STATUS_OK)
                {
                    playerInfo.PlayerData = JsonConvert.DeserializeObject<PlayerData>(e.Info["user"]);
                }
                break;
        }
    }
}