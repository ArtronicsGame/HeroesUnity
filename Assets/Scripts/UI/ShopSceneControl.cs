using System;
using UnityEngine;
using UnityEngine.UI;
using Event = EventSystem.Model.Event;

public class ShopSceneControl : EventBehaviour
{
    public Button button;
    public GameObject[] panels;
    private int numberOfItems;

    private struct ItemDetail
    {
        public string Name;
        public int Price;
    }
    private ItemDetail itemDetail;
    private MessageHandler _messageHandler;
    private Manage _manage;

    private void Awake()
    {
        var connectionManager = GameObject.Find("ConnectionManager");

        if (connectionManager != null)
        {
            _messageHandler = connectionManager.GetComponentInChildren<MessageHandler>();
        }

        _messageHandler.GetAllItems();
        
    }
    
    private void Start()
    {
        foreach (var panel in panels)
        {
            for (var i = 0; i < numberOfItems; i++)
            {
                var b = Instantiate(button);
                b.GetComponentInChildren<Text>().text = i.ToString();
                b.GetComponent<ItemBtn>().Name = itemDetail.Name;
                b.GetComponent<ItemBtn>().Price = itemDetail.Price;
                b.transform.SetParent(panel.transform);
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
                numberOfItems = Convert.ToInt32(e.Info["count"]);
                break;
        }
        print(numberOfItems);
    }
}
