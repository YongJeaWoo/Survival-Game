using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    [Header("Enemy Info")]
    public Text enemyATxt;
    public Text enemyBTxt;
    public Text enemyCTxt;

    public int enemyACount;
    public int enemyBCount;
    public int enemyCCount;

    [Header("Unit Info")]
    public Player player;
    public Boss boss;

    [Header("Enemy Spawn Info")]
    public Transform[] enemyZones;
    public GameObject[] enemies;
    public List<int> enemyList;

    [Header("Boss Spawn Info")]
    public Transform bossZone;
    public GameObject finalBoss;
    public GameObject bossHpGroup;
    public RectTransform bossCurHp;
    public bool bossAppear = false;

    // 싱글톤
    private static SpawnManager instance = null;

    private void Awake()
    {
        // 싱글톤
        if (null == instance)
        {
            instance = this;
        }

        StartCoroutine(Spawn());
        enemyList = new List<int>();
    }

    public static SpawnManager Instance
    {
        get
        {
            if (null == instance)
                instance = new SpawnManager();
            return instance;
        }
    }


    private void LateUpdate()
    {
        enemyATxt.text = enemyACount.ToString();
        enemyBTxt.text = enemyBCount.ToString();
        enemyCTxt.text = enemyCCount.ToString();

        // bossCurHp.localScale = new Vector3(boss.curHp / boss.maxHp, 1, 1);
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(5f);

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

            while (enemyList.Count > 0)
            {
                int ranZone = Random.Range(0, 3);
                GameObject instantEnemy = Instantiate(enemies[enemyList[0]], enemyZones[ranZone].position, enemyZones[ranZone].rotation);
                Enemy enemy = instantEnemy.GetComponent<Enemy>();
                enemy.target = player.transform;
                enemyList.RemoveAt(0);
                yield return new WaitForSeconds(4f);
            }
        }
    }

    public void SpawnBoss()
    {
        if (bossAppear)
            return;

        bossAppear = true;

        // 생성
        GameObject instantBoss = Instantiate(finalBoss, bossZone.position, bossZone.rotation);
        instantBoss.transform.Rotate(Vector3.up * 180);

        Boss boss = instantBoss.GetComponent<Boss>();
        bossHpGroup.SetActive(true);
        
        if (instantBoss != null)
        {
            bossCurHp.localScale = new Vector3((float)boss.curHp / boss.maxHp, 1, 1);
        }

        boss.target = player.transform;
    }
}
