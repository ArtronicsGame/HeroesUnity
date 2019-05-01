using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    public InputField inputField;
    private MessageHandler _messageHandler;
    private Manage _manage;
    
    public void RegisterPlayer()
    {
        var connectionManager = GameObject.Find("ConnectionManager");

        if (connectionManager != null)
        {
            _messageHandler = connectionManager.GetComponentInChildren<MessageHandler>();
        }
        
        _messageHandler.NewPlayer(inputField.text);
        
        _manage.Registercont();
    }
}
