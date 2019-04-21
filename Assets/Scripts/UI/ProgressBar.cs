using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ProgressBar : MonoBehaviour
{
    public Image background;
    public Image progress;

    private void Awake()
    {
        StartCoroutine(Progress());
    }

    private IEnumerator Progress()
    {
        while (progress.fillAmount < 1)
        {
            progress.fillAmount += Time.deltaTime / 10;
            yield return null;
        }

        SceneManager.LoadScene("MenuScene");
    }
}
