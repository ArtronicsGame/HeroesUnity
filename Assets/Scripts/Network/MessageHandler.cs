using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Event = EventSystem.Model.Event;

public class MessageHandler : MonoBehaviour
{

    private MainTCPConnection _mainTcp;
    private MatchTCPConnection _matchTcp;
    
    void Start()
    {
        _mainTcp = GetComponentInParent<MainTCPConnection>();

        GameObject matchHandler = GameObject.Find("MatchHandler");
        Debug.Log(matchHandler);
        if(matchHandler != null)
            _matchTcp = matchHandler.GetComponent<MatchTCPConnection>();
        NewMatch();
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
            {"id", "4"}
        };
        _matchTcp.SendInitialData(JsonConvert.SerializeObject(e));
    }
    
    public bool TCPHandshake(string id, string matchId)
    {
        Event e = new Event();
        e.Type = "MatchController.tcpHandshake";
        e.Info = new Dictionary<string, string>()
        {
            {"id", id},
            {"matchId", matchId}
        };
        return _matchTcp.SendData(JsonConvert.SerializeObject(e));
    }
}
