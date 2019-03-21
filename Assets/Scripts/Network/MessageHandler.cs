using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Event = EventSystem.Model.Event;

public class MessageHandler : MonoBehaviour
{

    private TCPConnection _tcp;
    
    void Start()
    {
        _tcp = GetComponentInParent<TCPConnection>();
//        NewPlayer("Ali");
    }

    bool NewPlayer(string username)
    {
        Event e = new Event();
        e.Type = "PlayerController.new";
        e.Info = new Dictionary<string, string>()
        {
            {"username", username}
        };
        Debug.Log(JsonConvert.SerializeObject(e));
        return _tcp.SendData(JsonConvert.SerializeObject(e));
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
