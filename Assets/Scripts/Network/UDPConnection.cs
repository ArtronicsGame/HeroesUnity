using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using EventSystem.Model;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Event = EventSystem.Model.Event;

public class UDPConnection : MonoBehaviour
{
    public string IPAddress = "5.253.27.99";
    public int Port;

    private UdpClient _client;
    private Thread _readThread;
    private ResponseAnalyzer _responseAnalyzer;

    void Start()
    {
        GameObject connectionManager = GameObject.Find("ConnectionManager");
        if (connectionManager != null)
            _responseAnalyzer = connectionManager.GetComponentInChildren<ResponseAnalyzer>();
    }

    public void Connect(string ip, int port)
    {
        _client = new UdpClient();
        IPAddress = ip;
        Port = port;
        _client.Connect(IPAddress, Port);
        _readThread = new Thread(UdpReader) {IsBackground = true};
        _readThread.Start();
    }

    void UdpReader()
    {
        while (true)
        {
            IPEndPoint remoteIpEndPoint = new IPEndPoint(System.Net.IPAddress.Any, 0);

            // Blocks until a message returns on this socket from a remote host.
            Byte[] receiveBytes = _client.Receive(ref remoteIpEndPoint);
            _responseAnalyzer.Analyze(new Event(new UDPPacket(receiveBytes)));
        }
    }

    public bool SendData(byte[] data)
    {
        try
        {
            _client.Send(data, data.Length);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    void OnDestroy()
    {
        _readThread?.Abort();
        _client?.Close();
    }
}