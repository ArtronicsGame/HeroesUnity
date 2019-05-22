using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Event = EventSystem.Model.Event;

public class ShopSceneControl : EventBehaviour
{
    public Button button;
    public GameObject[] panels;
    private string chestInfo;
    private MessageHandler _messageHandler;
    private Manage _manage;
    private ResponseAnalyzer _responseAnalyzer;

    private void Awake()
    {
        var connectionManager = GameObject.Find("ConnectionManager");
        _responseAnalyzer = FindObjectOfType<ResponseAnalyzer>();

        _responseAnalyzer.map.Add("ShopController", this);

        if (connectionManager != null)
        {
            _messageHandler = connectionManager.GetComponentInChildren<MessageHandler>();
        }

        _messageHandler.GetAllItems();
    }

    private void MakeChest()
    {
        foreach (var panel in panels)
        {
            for (var i = 0; i < 2; i++)
            {
                var b = Instantiate(button);
                b.GetComponentInChildren<Text>().text = i.ToString();
                b.transform.SetParent(panel.transform);
                b.transform.localScale = Vector3.one;
            }
        }

        panels[0].transform.parent.GetComponent<ScrollRect>().normalizedPosition = Vector2.zero;
        panels[1].transform.parent.GetComponent<ScrollRect>().normalizedPosition = Vector2.zero;
    }

    protected override void OnEvent(Event e)
    {
        switch ((Status) int.Parse(e.Info["status"]))
        {
            case Status.STATUS_OK:
                Debug.Log("Reached OK");
                print(e.Info["response"]);
//                print("type: "+e.Info["type"] + " price: " + e.Info["price"]);
                foreach (KeyValuePair<string, string> k in e.Info)
                {
                    print("key " + k.Key + " value: " + k.Value);
                }

                MakeChest();
                break;
            case Status.STATUS_FAILED:
                Debug.Log("Failed");
                break;
        }
    }
}