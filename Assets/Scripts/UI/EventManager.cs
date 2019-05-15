using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    public PlayerInfo playerInfo;
    private MessageHandler _messageHandler;
    private ScrollSnapRect _scrollSnap;
    
    private void Awake()
    {
        _scrollSnap = FindObjectOfType<ScrollSnapRect>();
        var connectionManager = GameObject.Find("ConnectionManager");

        if (connectionManager != null)
        {
            _messageHandler = connectionManager.GetComponentInChildren<MessageHandler>();
        }    
    }
    
    public void OnPlayBtnClicked()
    {
        playerInfo.PlayerData.CurrentHero = "Tank";
        SceneManager.LoadScene("MatchScene");
    }
}
