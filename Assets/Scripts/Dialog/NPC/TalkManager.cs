using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkManager : MonoBehaviour
{
    public Text nameText;
    public Text descriptionText;
    public GameObject scanObject;
    public Animator anim;

    // ui 끄기
    public GameObject uiOff;

    public bool isAction;

    public void Action(GameObject scanObj)
    {
        if (isAction)
        {
            isAction = false;
            uiOff.SetActive(true);
            anim.SetBool("isOpen", false);
        }
        else
        {
            isAction = true;
            uiOff.SetActive(false);
            anim.SetBool("isOpen", true);
            scanObject = scanObj;
            nameText.text = scanObject.name;
        }
    }
}
