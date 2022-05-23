using System.Collections;
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
    public Boss boss;

    public int enemyACount;
    public int enemyBCount;
    public int enemyCCount;

    [Header ("UI Info")]
    public GameObject gamePanel;

    [Header("Player Info")]
    public Text playerHpTxt;
    public Text playerAmmoTxt;

    [Header("Enemy Info")]
    public Text enemyATxt;
    public Text enemyBTxt;
    public Text enemyCTxt;

    [Header("Boss Info")]
    public RectTransform bossHpGroup;
    public RectTransform bossCurHpBar;

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

    public TalkManager talkManager;


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

        enemyATxt.text = enemyACount.ToString();
        enemyBTxt.text = enemyBCount.ToString();
        enemyCTxt.text = enemyCCount.ToString();

        // 보스 체력바
        if (boss != null)
        {
            bossHpGroup.anchoredPosition = Vector3.down * 30;
            bossCurHpBar.localScale = new Vector3((float)boss.curHp / boss.maxHp, 1, 1);
        }
        else
        {
            bossHpGroup.anchoredPosition = Vector3.up * 200;
        }
    }

    public void GoodEnding()
    {
        StartCoroutine(GoodFade());
        Invoke("VictoryChangeScn", 3f);
    }

    IEnumerator GoodFade()
    {
        uiOff.SetActive(false);
        float fadeCount = 0; // 첫 알파값
        while (fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            image.color = new Color(0, 0, 0, fadeCount);
        }
    }

    public void VictoryChangeScn()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("OpenEndingScene");
    }


    public void GameOver()
    {
        StartCoroutine(GameOverFade());
        Invoke("ChangeScn", 3f);
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
    }

    void ChangeScn()
    {
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
        string talkData = talkManager.GetTalk(id, talkIndex);

        // 대화가 더 이상 없다면
        if (talkData == null)
        {
            isAction = false;
            uiOff.SetActive(true);
            anim.SetBool("isOpen", false);
            talkIndex = 0;      // 대화 초기화
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
