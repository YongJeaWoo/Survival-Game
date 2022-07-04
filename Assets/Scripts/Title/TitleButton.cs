using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour
{
    public GameObject title;
    public GameObject targetCam;
    public GameObject movingCam;

    public Image image;

    public GameObject optionPanel;
    public GameObject buttonCollect;

    public void StartButton()
    {
        title.SetActive(false);
        targetCam.SetActive(false);
        movingCam.SetActive(true);

        image.enabled = false;
        // StartCoroutine(Fade());

    }

    IEnumerator Fade()
    {
        image.enabled = true;

        float fadeCount = 1; // 첫 알파값
        while (fadeCount <= 0f)
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            image.color = new Color(0, 0, 0, fadeCount);
        }
    }

    public void OptionButton()
    {
        Time.timeScale = 0.5f;
        optionPanel.SetActive(true);
        buttonCollect.SetActive(false);
    }

    public void OptionExit()
    {
        Time.timeScale = 1f;
        optionPanel.SetActive(false);
        buttonCollect.SetActive(true);
    }

    public void ExitButton()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
