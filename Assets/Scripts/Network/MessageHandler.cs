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

    void Start()
    {
        _mainTcp = GetComponentInParent<MainTCPConnection>();
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

    public bool GetPlayer(string id)
    {
        Event e = new Event();
        e.Type = "PlayerController.get";
        e.Info = new Dictionary<string, string>()
        {
            {"_id", id}
        };
        Debug.Log(JsonConvert.SerializeObject(e));
        return _mainTcp.SendData(JsonConvert.SerializeObject(e));
    }

}