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

    private TcpClient _client;
    private Thread _readThread;
    private ResponseAnalyzer _responseAnalyzer;
    
    void Start()
    {
        GameObject connectionManager = GameObject.Find("ConnectionManager");
        if(connectionManager != null)
            _responseAnalyzer = connectionManager.GetComponentInChildren<ResponseAnalyzer>();
    }

    void TcpReader()
    {
        try { 			
            using(StreamReader reader = new StreamReader(_client.GetStream(), Encoding.UTF8)) {
                string line;
                while((line = reader.ReadLine()) != null) {
                    if (line.Contains(";"))
                    {
                        Event e = new Event {Type = "MatchUpdate"};
                        string[] slice = line.Split(';');
                        e.Info = new Dictionary<string, string>()
                        {
                            {"X", slice[0]},
                            {"Y", slice[1]},
                            {"Angle", slice[2]}
                        };
                        _responseAnalyzer.Analyze(e);
                    }
                    else
                    {
                        Debug.Log(line);
                    }
                }
            }
        }         
        catch (SocketException socketException) {             
            Debug.Log("Socket exception: " + socketException);         
        }     
    }

    public void ConnectMatchServer(int port)
    {
        _client = new TcpClient(IPAddress, port);
        _readThread = new Thread(TcpReader);
        _readThread.IsBackground = true;
        _readThread.Start();
    }
    
    public bool SendData(string text)
    {
        try
        {
            Byte[] data =
                System.Text.Encoding.ASCII.GetBytes(text);
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
    
    public bool SendInitialData(string text)
    {
        if(_client == null || !_client.Connected)
            _client = new TcpClient(IPAddress, Port);
        try
        {
            Byte[] data =
                System.Text.Encoding.ASCII.GetBytes(text);
            
            Byte[] bytes = new Byte[1024];             
            using (NetworkStream stream = _client.GetStream()) { 
                int length; 	
                stream.Write(data, 0, data.Length);
                while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) { 						
                    var incomingData = new byte[length]; 						
                    Array.Copy(bytes, 0, incomingData, 0, length); 	
                        
                    string message = System.Text.Encoding.ASCII.GetString(incomingData); 
                    Event response = JsonConvert.DeserializeObject<Event>(message);
                    
                    _client.Close();
                    _client.Dispose();
                    if (!_responseAnalyzer.Analyze(response))
                        return false;
                    break;
                } 				
            } 
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        
    }

    private void OnDestroy()
    {
        if(_readThread != null && _readThread.IsAlive)
        _readThread.Abort();
        if(_client != null && _client.Connected)
            _client.Close();
    }
}