using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Event = EventSystem.Model.Event;

public class MatchHandler : EventBehaviour
{
    public PlayerInfo PlayerInfo;

    private MainTCPConnection _mainTcp;
    private MatchTCPConnection _matchTcp;
    private UDPConnection _matchUdp;
    private MatchMessageHandler _messageHandler;

    new void Start()
    {
        base.Start();
        _messageHandler = GetComponentInChildren<MatchMessageHandler>();
        _matchTcp = GetComponent<MatchTCPConnection>();
        _matchUdp = GetComponent<UDPConnection>();
       
        _messageHandler.NewMatch();
    }

    protected override void OnEvent(Event e)
    {
        switch (e.Type)
        {
            case "MatchPlace":
                Debug.Log("We Have Match Port");
                PlayerInfo.PlayerData.MatchIP = e.Info["matchIP"];
                PlayerInfo.PlayerData.MatchPort = int.Parse(e.Info["matchPort"]);
                _matchTcp.Connect(PlayerInfo.PlayerData.MatchIP, PlayerInfo.PlayerData.MatchPort);
                _matchUdp.Connect(PlayerInfo.PlayerData.MatchIP, PlayerInfo.PlayerData.MatchPort);
                _messageHandler.TCPHandshake();
                _messageHandler.UDPHandshake();

                break;
            case "MatchUpdate":
                GameObject o = GameObject.Find(e.Info["Id"]);
                if (o == null)
                {
                    Debug.Log("Object " + e.Info["Id"] + " Not Found");
                    return;
                }

                o.GetComponent<NetworkDriven>().SetTarget(float.Parse(e.Info["X"]), float.Parse(e.Info["Y"]),
                    Mathf.Rad2Deg * float.Parse(e.Info["Angle"]));
                break;
            case "MatchStart":

                break;
            case "FakeID":
                Debug.Log("Fake ID Received");
                PlayerInfo.MatchPlayerID = int.Parse(e.Info["id"]);
                break;
            case "HeroID":
                PlayerInfo.HeroName = e.Info["HeroID"];
                break;
            case "MatchEnd":
                Debug.Log("Match Ended");
                SceneManager.LoadScene("MenuScene");
                break;
        }
    }
}