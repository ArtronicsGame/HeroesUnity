using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoUpdater : MonoBehaviour
{
    public PlayerInfo playerInfo;
    private void Start()
    {
        GameObject.Find("Coins").GetComponentInChildren<Text>().text = playerInfo.PlayerData.Coins.ToString();
        
        GameObject.Find("Trophies").GetComponentInChildren<Text>().text = playerInfo.PlayerData.Trophies.ToString();
    }

    private void Update()
    {
        var frames = 0;
        if (frames++/100 == 0)
            ScarceUpdate();
    }

    private void ScarceUpdate()
    {
        var playerCoins = playerInfo.PlayerData.Coins;
        var playerTrophies = playerInfo.PlayerData.Trophies;

        if (int.Parse(GameObject.Find("Coins").GetComponentInChildren<Text>().text) != playerCoins)
            GameObject.Find("Coins").GetComponentInChildren<Text>().text = playerCoins.ToString();
        
        if (int.Parse(GameObject.Find("Trophies").GetComponentInChildren<Text>().text) != playerTrophies)
            GameObject.Find("Trophies").GetComponentInChildren<Text>().text = playerTrophies.ToString();
    }
}