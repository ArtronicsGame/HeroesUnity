using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using UnityEditor;
using UnityEngine;
using Event = EventSystem.Model.Event;

public class MainTCPConnection : MonoBehaviour
{
    // Start is called before the first frame update

    public string IPAddress = "5.253.27.99";
    public int Port = 8008;
    public List<GameObject> enableOnConnection;

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
        Thread bootstrapThread = new Thread(Bootstrap);
        bootstrapThread.Start();
    }

    private void Update()
    {
        if(_client != null && _client.Connected && _readThread != null && _readThread.IsAlive)
            for (int i = 0; i < enableOnConnection.Count; i++)
                enableOnConnection[i].SetActive(true);
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
            
            using (StreamReader reader = new StreamReader(_client.GetStream(), Encoding.ASCII))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Debug.Log("Line: " + line);
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
        catch (SocketException socketException) {             
            Debug.Log("Socket exception: " + socketException);         
        }     
    }

    public bool SendData(string text)
    {
        try
        {
            Byte[] data =
                System.Text.Encoding.ASCII.GetBytes(text + "\n");
            NetworkStream stream = _client.GetStream();
            stream.Write(data, 0, data.Length);
            stream.Flush();
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