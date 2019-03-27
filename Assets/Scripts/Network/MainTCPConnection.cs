using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using Event = EventSystem.Model.Event;

public class MainTCPConnection : MonoBehaviour
{
    // Start is called before the first frame update

    public string IPAddress = "5.253.27.99";
    public int Port = 8008;

    private TcpClient _client;
    private Thread _readThread;
    private ResponseAnalyzer _responseAnalyzer;

    int GetNewPort()
    {
        _client = new TcpClient(IPAddress, Port);
        NetworkStream inputStream = _client.GetStream();
        Byte[] data = new Byte[256];
        int dataSize = inputStream.Read(data, 0, data.Length);
        string message = System.Text.Encoding.ASCII.GetString(data, 0, dataSize);
        int newPort = int.Parse(message);
        print("New Port: " + newPort);

        inputStream.Close();
        _client.Close();
        _client.Dispose();
        return newPort;
    }
    
    void Start()
    {
        _responseAnalyzer = GetComponentInChildren<ResponseAnalyzer>();
        new Thread(Bootstrap).Start();
    }

    void Bootstrap()
    {
        Port = GetNewPort();
        _client = new TcpClient(IPAddress, Port);
        _readThread = new Thread(TcpReader);
        _readThread.IsBackground = true;
        _readThread.Start();
    }

    void TcpReader()
    {
        try { 			
            Byte[] bytes = new Byte[1024];             
            while (true) { 				
                // Get a stream object for reading 				
                using (NetworkStream stream = _client.GetStream()) { 
                    int length; 								
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) { 						
                        var incomingData = new byte[length]; 						
                        Array.Copy(bytes, 0, incomingData, 0, length); 	
                        
                        string message = System.Text.Encoding.ASCII.GetString(incomingData);
                        try
                        {
                            Event response = JsonConvert.DeserializeObject<Event>(message);
                            _responseAnalyzer.Analyze(response);
                        }
                        catch (Exception)
                        {
                            Debug.Log("Non-JSON Message: " + message);
                        }
                    } 				
                } 			
            }         
        }         
        catch (SocketException socketException) {             
            Debug.Log("Socket exception: " + socketException);         
        }     
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

    private void OnDestroy()
    {
        if(_readThread != null && _readThread.IsAlive)
        _readThread.Abort();
        if(_client != null && _client.Connected)
            _client.Close();
    }
}