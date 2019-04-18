using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO: For release All Scenes Must be disabled

public class UIControl : MonoBehaviour
{
    [Tooltip("scenes[0]: ClansScene\n"
              + "scenes[1]: ChestsScene\n"
              + "scenes[2]: InAppScene\n"
              + "scenes[3]: HomeScene\n"
              + "scenes[4]: ActionScene\n"
              + "scenes[5]: ItemScene\n"
              + "scenes[6]: ShopScene\n")]

    public GameObject[] scenes;
    public Button[] buttons;
    private int prevScene = 3;

    private void Start()
    {
        scenes[3].SetActive(true);
        buttons[3].image.color = new Color32(196, 19, 46, 255);
    }

    public void ChangeScene(int i)
    {
        scenes[prevScene].SetActive(false);
        buttons[prevScene].image.color = new Color32(255, 255, 255, 255);
        scenes[i].SetActive(true);
        buttons[i].image.color = new Color32(196, 19, 46, 255);
        prevScene = i;

    }



}
