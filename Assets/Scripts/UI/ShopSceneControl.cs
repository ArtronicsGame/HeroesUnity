using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSceneControl : MonoBehaviour
{
    public Button button;
    public GameObject[] panels;
    private void Start()
    {
        foreach (var panel in panels)
        {
            for (int i = 0; i < 50; i++)
            {
                Button b = Instantiate(button);
                b.GetComponentInChildren<Text>().text = i.ToString();
                b.transform.SetParent(panel.transform);
            }
        }

        panels[0].transform.parent.GetComponent<ScrollRect>().normalizedPosition = Vector2.zero;
        panels[1].transform.parent.GetComponent<ScrollRect>().normalizedPosition = Vector2.zero;

    }
}
