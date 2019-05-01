using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Event = EventSystem.Model.Event;

public class PlayerManager : EventBehaviour
{
    public PlayerInfo playerInfo;
    public Manage manage;
    private MessageHandler _messageHandler;

    new void Start()
    {
        base.Start();
        GameObject connectionManager = GameObject.Find("ConnectionManager");

        if (connectionManager != null)
        {
            _messageHandler = connectionManager.GetComponentInChildren<MessageHandler>();
        }

//        PlayerPrefs.SetString("id", "5ca5ac747824b4087e86b4e0");
//        PlayerPrefs.DeleteAll();
        
        if (PlayerPrefs.HasKey("id"))
        {
            _messageHandler.GetPlayer(PlayerPrefs.GetString("id"));
        }
        else
        {
            // register
            manage.Register();
        }
    }    

    protected override void OnEvent(Event e)
    {
        switch (e.Type)
        {
            
            case "GetPlayerResp":
                if ((Status)int.Parse(e.Info["status"]) == Status.STATUS_OK)
                {
                    playerInfo.PlayerData = JsonConvert.DeserializeObject<PlayerData>(e.Info["user"]);
                }
                break;
        }
    }
}