using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using Event = EventSystem.Model.Event;

public class MatchTCPConnection : MonoBehaviour
{
    public string IPAddress = "5.253.27.99";
    public int Port = 40123;
    
    public string MatchIPAddress = "-";
    public int MatchPort = 0;
    
    

    private TcpClient _client;
    private Thread _readThread;
    private ResponseAnalyzer _responseAnalyzer;

    void Start()
    {
        _responseAnalyzer = GetComponentInChildren<ResponseAnalyzer>();
    }

    void TcpReader()
    {
        try
        {
            using (StreamReader reader = new StreamReader(_client.GetStream(), Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        Event response = JsonConvert.DeserializeObject<Event>(line);
                        _responseAnalyzer.Analyze(response);
                    }
                    catch (Exception)
                    {
                        Debug.Log("Non-JSON Message: " + line);
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }

    public void Connect(string ip, int port)
    {
        MatchIPAddress = ip;
        MatchPort = port;
        
        _client = new TcpClient(ip, port);
        _readThread = new Thread(TcpReader);
        _readThread.IsBackground = true;
        _readThread.Start();
    }

    public bool SendData(string text)
    {
        try
        {
            Debug.Log("Match Send: " + text);
            Byte[] data =
                Encoding.ASCII.GetBytes(text);
            NetworkStream stream = _client.GetStream();
            stream.Write(data, 0, data.Length);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public void SendInitialData(string text)
    {
        new Thread((() =>
        {
            if (_client == null || !_client.Connected)
                _client = new TcpClient(IPAddress, Port);
            try
            {
                Byte[] data =
                    Encoding.ASCII.GetBytes(text);

                Byte[] bytes = new Byte[1024];
                using (NetworkStream stream = _client.GetStream())
                {
                    int length;
                    stream.Write(data, 0, data.Length);
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var incomingData = new byte[length];
                        Array.Copy(bytes, 0, incomingData, 0, length);

                        string message = System.Text.Encoding.ASCII.GetString(incomingData);
                        Event response = JsonConvert.DeserializeObject<Event>(message);

                        _client.Close();
                        _client.Dispose();
                        Debug.Log("MatchMaker: " + message);
                        if (_responseAnalyzer.Analyze(response))
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        })).Start();
    }

    private void OnDestroy()
    {
        if (_readThread != null && _readThread.IsAlive)
            _readThread.Abort();
        if (_client != null && _client.Connected)
            _client.Close();
    }
}