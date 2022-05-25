﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header ("Fade Effect && UI")]
    public Image image;
    public GameObject imageGo;
    public GameObject inGameInfo;
    public Image firstDis;

    [Header ("InGame Units")]
    public Player player;

    [Header("Player Info")]
    public Text playerHpTxt;
    public Text playerAmmoTxt;

    [Header("Inven Window")]
    public Image weapon1Img;
    public Image weapon2Img;
    public Image weapon3Img;
    public Image weaponRImg;

    [Header ("Conversation Info")]
    public Text nameText;
    public Text descriptionText;
    public GameObject scanObject;
    public Animator anim;

    [Header("Info UI Off")]
    public GameObject uiOff;
    public bool isAction;
    public int talkIndex;

    [Header("Another Manager")]
    public TalkManager talkManager;
    public QuestManager questManager;

    private void Start()
    {
        questManager.CheckQuest();
    }

    private void Awake()
    {
        StartCoroutine(Fade());
        StartCoroutine(ShowDisplayInfo());
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

    IEnumerator ShowDisplayInfo()
    {
        yield return new WaitForSeconds(1f);
        firstDis.gameObject.SetActive(true);
        yield return new WaitForSeconds(4f);
        firstDis.gameObject.SetActive(false);
        inGameInfo.gameObject.SetActive(true);

        yield break;
    }

    private void LateUpdate()
    {
        playerHpTxt.text = player.hp.ToString();

        if (player.equipWeapons == null)
            playerAmmoTxt.text = " - / " + player.ammo;
        else if (player.equipWeapons.type == Weapon.Type.Melee)
            playerAmmoTxt.text = " - / " + player.ammo;
        else
            playerAmmoTxt.text = player.equipWeapons.curAmmo + " / " + player.ammo;

        // 무기를 가지고 있는지 없는지 알파값으로 표시
        weapon1Img.color = new Color(1, 1, 1, player.hasWeapons[0] ? 1 : 0);
        weapon2Img.color = new Color(1, 1, 1, player.hasWeapons[1] ? 1 : 0);
        weapon3Img.color = new Color(1, 1, 1, player.hasWeapons[2] ? 1 : 0);
        weaponRImg.color = new Color(1, 1, 1, player.hasGrenades > 0 ? 1 : 0);
    }

    // npc 안 봄
    public void GoodEnding()
    {
        StartCoroutine(VictoryChangeScene());
    }

    // npc 봄
    public void BadEnding()
    {
        StartCoroutine(VictoryButChangeScn());
    }

    // 열린엔딩 npc 안 봤다
    IEnumerator VictoryChangeScene()
    {
        uiOff.SetActive(false);
        float fadeCount = 0; // 첫 알파값
        while (fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            image.color = new Color(0, 0, 0, fadeCount);
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("NPCNOTSeeScene");
    }

    // 충격엔딩 npc 봤다
    IEnumerator VictoryButChangeScn()
    {
        uiOff.SetActive(false);
        float fadeCount = 0; // 첫 알파값
        while (fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            image.color = new Color(0, 0, 0, fadeCount);
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("NPCSeeScene");
    }


    public void GameOver()
    {
        StartCoroutine(GameOverFade());
    }

    IEnumerator GameOverFade()
    {
        yield return new WaitForSeconds(1f);

        float fadeCount = 0; // 첫 알파값
        while (fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            image.color = new Color(0, 0, 0, fadeCount);
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("GameOverScene");
    }
    
    // 대화창 표시
    public void Action(GameObject scanObj)
    {
        scanObject = scanObj;
        ObjData objData = scanObject.GetComponent<ObjData>();
        Talk(objData.id, objData.isNpc);
    }

    void Talk(int id, bool isNpc)
    {
        int questIndex = questManager.GetQuestIndex(id);
        string talkData = talkManager.GetTalk(id + questIndex, talkIndex);

        // 대화가 더 이상 없다면
        if (talkData == null)
        {
            isAction = false;
            uiOff.SetActive(true);
            anim.SetBool("isOpen", false);
            talkIndex = 0;      // 대화 초기화
            questManager.CheckQuest(id);
            Time.timeScale = 1;
            return;
        }

        if (isNpc)
        {
            descriptionText.text = talkData;
        }
        else
        {
            descriptionText.text = talkData;
        }
        Time.timeScale = 0;
        isAction = true;
        talkIndex++;            // 대화 더 있으면 계속 증가
        uiOff.SetActive(false);
        anim.SetBool("isOpen", true);
    }
}