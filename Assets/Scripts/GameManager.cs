using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header ("Fade Effect && UI")]
    public Image image;
    public GameObject imageGo;
    public Image infoUI;

    public GameObject menuCam;

    








    private void Awake()
    {
        StartCoroutine(Fade());
        StartCoroutine(ShowInfo());
    }

    IEnumerator Fade()
    {
        imageGo.gameObject.SetActive(true);
        float fadeCount = 1f; // 첫 알파값
        while (fadeCount > 0f)
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            image.color = new Color(0, 0, 0, fadeCount);
        }

        yield break;
    }

    IEnumerator ShowInfo()
    {
        yield return new WaitForSeconds(2f);
        infoUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(4f);
        infoUI.gameObject.SetActive(false);

        yield break;
    }
}
