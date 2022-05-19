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
        bossCurHpBar.localScale = new Vector3((float)boss.curHp / boss.maxHp, 1, 1);
    }
}
