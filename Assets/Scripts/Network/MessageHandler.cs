using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Newtonsoft.Json;
using UnityEngine;
using Event = EventSystem.Model.Event;

public class MessageHandler : MonoBehaviour
{
    public static MessageHandler i;
    
    public PlayerInfo playerInfo;
    public MovementData movementData;

    private MainTCPConnection _mainTcp;

    private void Awake()
    {
        i = this;
    }
    
    void Start()
    {
        _mainTcp = GetComponentInParent<MainTCPConnection>();
    }


    public bool BuyItems(string item)
    {
        var e = new Event {Type = "ShopController.buy"};
        Debug.Log(JsonConvert.SerializeObject(e));
        return _mainTcp.SendData(JsonConvert.SerializeObject(e));
    }

    public bool GetAllItems()
    {
        var e = new Event {Type = "ShopController.getAllItems",Info = new Dictionary<string, string>() {{"_id", "null"}}};

        Debug.Log(JsonConvert.SerializeObject(e));
        return _mainTcp.SendData(JsonConvert.SerializeObject(e));
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
        Event e = new Event {Type = "PlayerController.get", Info = new Dictionary<string, string>() {{"_id", id}}};
        Debug.Log(JsonConvert.SerializeObject(e));
        return _mainTcp.SendData(JsonConvert.SerializeObject(e));
    }

}