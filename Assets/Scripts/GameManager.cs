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

    [Header("Another Manager")]
    public TalkManager talkManager;
    public QuestManager questManager;

    [Header("EnemySpawnZone")]
    public Transform[] enemyZones;
    public GameObject[] enemies;
    public List<int> enemyList;

    private void Start()
    {
        questManager.CheckQuest();
    }

    private void Awake()
    {
        enemyList = new List<int>();

        StartCoroutine(Fade());
        StartCoroutine(ShowDisplayInfo());
        StartCoroutine(Spawn());
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

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(10f);

        for (int index = 0; index < 10; index++)
        {
            int ran = Random.Range(0, 3);
            enemyList.Add(ran);

            switch (ran)
            {
                case 0:
                    enemyACount++;
                    break;
                case 1:
                    enemyBCount++;
                    break;
                case 2:
                    enemyCCount++;
                    break;
            }
        }

        while(enemyList.Count > 0)
        {
            int ranZone = Random.Range(0, 4);
            GameObject instantEnemy = Instantiate(enemies[enemyList[0]], enemyZones[ranZone].position, enemyZones[ranZone].rotation);
            Enemy enemy = instantEnemy.GetComponent<Enemy>();
            enemy.target = player.transform;
            enemyList.RemoveAt(0);  // 생성 후 지우기
            yield return new WaitForSeconds(4f);
        }
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
