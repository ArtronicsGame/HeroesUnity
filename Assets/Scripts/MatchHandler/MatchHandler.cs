using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event = EventSystem.Model.Event;

public class MatchHandler : EventBehaviour
{
    public string matchId = "Not Connected";

    private MainTCPConnection _mainTcp;
    private MatchTCPConnection _matchTcp;
    private UDPConnection _matchUdp;
    private MessageHandler _messageHandler;

    void Start()
    {
        GameObject connectionManager = GameObject.Find("ConnectionManager");
        if (connectionManager != null)
        {
            _messageHandler = connectionManager.GetComponentInChildren<MessageHandler>();
        }

        _matchTcp = GetComponent<MatchTCPConnection>();
        _matchUdp = GetComponent<UDPConnection>();
    }

    new void Update()
    {
        base.Update();
    }

    protected override void OnEvent(Event e)
    {
        switch (e.Type)
        {
            case "MatchPlace":
                if (e.Info.ContainsKey("matchPort"))
                {
                    Debug.Log("We Have Match Port");
                    _matchTcp.ConnectMatchServer(int.Parse(e.Info["matchPort"]));
                    _matchUdp.Connect(int.Parse(e.Info["matchPort"]));
                    matchId = e.Info["matchId"];
                    _messageHandler.TCPHandshake("4", matchId);
                    _messageHandler.UDPHandshake("4", matchId);
                }

                break;
            case "MatchUpdate":
                GameObject crate = GameObject.Find("Crate");
                crate.transform.position = new Vector2(float.Parse(e.Info["X"]), float.Parse(e.Info["Y"]));
                crate.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Rad2Deg * float.Parse(e.Info["Angle"]));
                break;
        }
    }
}