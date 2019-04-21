using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    public InputField inputField;
    private MessageHandler _messageHandler;

    public void RegisterPlayer()
    {
        print(inputField.text);
        // _messageHandler.NewPlayer(inputField.text);
    }
}
