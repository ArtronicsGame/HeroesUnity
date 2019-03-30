using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Newtonsoft.Json;
using UnityEngine;
using Event = EventSystem.Model.Event;

public class MessageHandler : MonoBehaviour
{
    public PlayerInfo playerInfo;
    public MovementData movementData;
    
    private MainTCPConnection _mainTcp;
    private MatchTCPConnection _matchTcp;
    private UDPConnection _matchUdp;
    
    void Start()
    {
        _mainTcp = GetComponentInParent<MainTCPConnection>();

        GameObject matchHandler = GameObject.Find("MatchHandler");
        Debug.Log(matchHandler);
        if (matchHandler != null)
        {
            _matchTcp = matchHandler.GetComponent<MatchTCPConnection>();
            _matchUdp = matchHandler.GetComponent<UDPConnection>();
        }

        NewMatch();
    }

    private bool moving = false;
    void Update()
    {
        if (movementData.speed != 0)
        {
            moving = true;
            SendLocation();
        }else if (moving)
        {
            moving = false;
            SendLocation();
        }
    }

    public bool NewPlayer(string username)
    {
        Event e = new Event();
        e.Type = "PlayerController.new";
        e.Info = new Dictionary<string, string>()
        {
            {"username", username}
        };
        Debug.Log(JsonConvert.SerializeObject(e));
        return _mainTcp.SendData(JsonConvert.SerializeObject(e));
    }
    
    public void NewMatch()
    {
        Event e = new Event();
        e.Type = "New";
        e.Info = new Dictionary<string, string>()
        {
            {"id", playerInfo.ID}
        };
        _matchTcp.SendInitialData(JsonConvert.SerializeObject(e));
    }

    public bool SendLocation()
    {
        Event e = new Event();
        e.Type = "HeroMove";
        e.Info = new Dictionary<string, string>()
        {
            {"id", playerInfo.MatchPlayerID},
            {"speed", movementData.speed.ToString()},
            {"x", movementData.direction.x.ToString()},
            {"y", movementData.direction.y.ToString()}
        };
        return _matchTcp.SendData(JsonConvert.SerializeObject(e) + "\n");
    }
    
    public bool TCPHandshake()
    {
        Event e = new Event();
        e.Type = "MatchController.tcpHandshake";
        e.Info = new Dictionary<string, string>()
        {
            {"id", playerInfo.ID},
            {"matchId", playerInfo.MatchID}
        };
        return _matchTcp.SendData(JsonConvert.SerializeObject(e));
    }
    
    public bool UDPHandshake()
    {
        Event e = new Event();
        e.Type = "MatchController.udpHandshake";
        e.Info = new Dictionary<string, string>()
        {
            {"id", playerInfo.ID},
            {"matchId", playerInfo.MatchID}
        };
        return _matchUdp.SendData(JsonConvert.SerializeObject(e));
    }
}
