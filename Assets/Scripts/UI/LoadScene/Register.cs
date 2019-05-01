using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Event = EventSystem.Model.Event;

public class Register : EventBehaviour
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
        
//        _manage.RegisterCont();
    }

    protected override void OnEvent(Event e)
    {
        switch (e.Type)
        {
            case "NewPlayerResp":
            {
                if ((Status) int.Parse(e.Info["status"]) == Status.STATUS_DUPLICATE)
                {
                    gameObject.GetComponent<InputField>().text = "";
                    gameObject.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "Duplicate Entry";
                }
            }
                break;
        }
    }

    private IEnumerator duplicateMsg()
    {
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "Enter your username";

    }
}
