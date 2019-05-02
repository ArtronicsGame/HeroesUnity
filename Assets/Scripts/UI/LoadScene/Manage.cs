using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manage : MonoBehaviour
{
    public GameObject loadInfo;
    public GameObject register;

    public void Register()
    {
        loadInfo.SetActive(false);
        register.SetActive(true);
    }
    
    public void RegisterCont()
    {
        loadInfo.SetActive(true);
        register.SetActive(false);
    }
}
