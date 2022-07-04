using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public GameObject other;
    public Image image;

    private void Update()
    {
        if (Vector3.Distance(other.transform.position, transform.position) < 1.5f)
        {
            StartCoroutine(Fade());
        }
    }

    IEnumerator Fade()
    {
        float fadeCount = 0; // 첫 알파값
        while (fadeCount <= 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            image.color = new Color(0, 0, 0, fadeCount);
        }

        ChangeScn();
    }

    void ChangeScn()
    {
        SceneManager.LoadScene("ExplainScene");
    }
}