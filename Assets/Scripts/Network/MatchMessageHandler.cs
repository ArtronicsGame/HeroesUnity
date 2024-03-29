using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Newtonsoft.Json;
using UnityEngine;
using Event = EventSystem.Model.Event;

public class MatchMessageHandler : MonoBehaviour
{
    public static MatchMessageHandler i;
    
    public PlayerInfo playerInfo;
    public MovementData movementData;

    private MatchTCPConnection _matchTcp;
    private UDPConnection _matchUdp;

    private void Awake()
    {
        i = this;
    }

    void Start()
    {
        GameObject matchHandler = GameObject.Find("MatchHandler");
        if (matchHandler != null)
        {
            _matchTcp = matchHandler.GetComponent<MatchTCPConnection>();
            _matchUdp = matchHandler.GetComponent<UDPConnection>();    
        }
    }

    public void NewMatch()
    {
        Event e = new Event();
        e.Type = "New";
        e.Info = new Dictionary<string, string>()
        {
            {"id", playerInfo.PlayerData.ID}
        };
        _matchTcp.SendInitialData(JsonConvert.SerializeObject(e));
    }

    public bool SendLocation()
    {
        UDPPacket packet = new UDPPacket();
        packet.type = 'U';
        packet.fList.Add(movementData.speed);
        packet.fList.Add(movementData.direction.x);
        packet.fList.Add(movementData.direction.y);

        return _matchUdp.SendData(packet.Encode());
    }

    public bool Shoot()
    {
        UDPPacket packet = new UDPPacket();
        packet.type = 'S';

        return _matchUdp.SendData(packet.Encode());
    }

    public bool TCPHandshake()
    {
        Event e = new Event();
        e.Type = "Handshake";
        e.Info = new Dictionary<string, string>()
        {
            {"id", playerInfo.PlayerData.ID},
        };
        return _matchTcp.SendData(JsonConvert.SerializeObject(e));
    }

    public bool UDPHandshake()
    {
        UDPPacket packet = new UDPPacket();
        packet.type = 'H';
        packet.str = playerInfo.PlayerData.ID;

        return _matchUdp.SendData(packet.Encode());
    }
}