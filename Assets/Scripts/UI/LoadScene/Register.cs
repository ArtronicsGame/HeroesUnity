using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        
    }

    protected override void OnEvent(Event e)
    {
        if (e.Type != "NewPlayerResp") return;
        switch ((Status) int.Parse(e.Info["status"]))
        {
            case Status.STATUS_DUPLICATE:
                gameObject.GetComponent<InputField>().text = "";
                gameObject.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "Duplicate Entry";
                StartCoroutine(DuplicateMsg());
                break;
            case Status.STATUS_OK:
                Debug.Log("Reached OK");
                PlayerPrefs.SetString("id", e.Info["userId"]);
                _messageHandler.GetPlayer(e.Info["userId"]);
                _manage.RegisterCont();
                break;
        }
    }

    private IEnumerator DuplicateMsg()
    {
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "Enter your username";
    }
}