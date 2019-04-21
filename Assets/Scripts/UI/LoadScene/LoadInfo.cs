using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadInfo : MonoBehaviour
{
    public Image progress;

    private void Awake()
    {
        progress.fillAmount = 0;
        StartCoroutine(LoadGame());
    }

    public IEnumerator LoadGame()
    {

        while (progress.fillAmount < 1)
        {
            progress.fillAmount += Time.deltaTime / 10;

            yield return null;
        }

        SceneManager.LoadScene("MenuScene");
    }
}