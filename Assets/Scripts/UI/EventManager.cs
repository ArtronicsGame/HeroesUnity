using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
//    private AssetBundle matchSe
    public void OnPlayBtnClicked()
    {
        SceneManager.LoadScene(2);
    }
}
