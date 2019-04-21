﻿using System.Collections;
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

        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetString("id", "5cbc09585e92e60450eca3b5");

        if (PlayerPrefs.HasKey("id"))
        {
            _messageHandler.GetPlayer(PlayerPrefs.GetString("id"));
        }
        else
        {
            // register
            manage.Register();
            // _messageHandler.NewPlayer("Soroush");
        }
    }

    protected override void OnEvent(Event e)
    {
        switch (e.Type)
        {
            case "NewPlayerResp":
                if ((Status)int.Parse(e.Info["status"]) == Status.STATUS_OK)
                {
                    PlayerPrefs.SetString("id", e.Info["userId"]);
                    _messageHandler.GetPlayer(e.Info["userId"]);
                }
                break;
            case "GetPlayerResp":
                if ((Status)int.Parse(e.Info["status"]) == Status.STATUS_OK)
                {
                    playerInfo.PlayerData = JsonConvert.DeserializeObject<PlayerData>(e.Info["user"]);
                }
                break;
        }
    }
}