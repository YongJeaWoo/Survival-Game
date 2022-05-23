using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public GameObject bossHpBar;

    public GameObject[] questObject;
    public int questId;
    public int questOrder;      // 퀘스트 대화 순서

    Dictionary<int, QuestData> questList;

    private void Awake()
    {
        questList = new Dictionary<int, QuestData>();   // 생성
        GenerateData();
    }

    void GenerateData()
    {
        questList.Add(10, new QuestData("동생을 찾아보자", 
                                        new int[] {1000, 2000}));

        questList.Add(20, new QuestData("보스를 잡자",
                                        new int[] {2000, 5000}));

        questList.Add(30, new QuestData("퀘스트 클리어",
                                        new int[] {0}));
    }

    public int GetQuestIndex(int id)
    {
        return questId + questOrder;
    }

    public string CheckQuest(int id)
    {
        if (id == questList[questId].npcId[questOrder])
            questOrder++;

        if (questOrder == questList[questId].npcId.Length)
            NextQuest();

        ControlObject();

        return questList[questId].questName;
    }

    public string CheckQuest()
    {
        return questList[questId].questName;
    }

    void NextQuest()
    {
        questId += 10;
        questOrder = 0;
    }

    void ControlObject()
    {
        switch (questId)
        {
            case 10:
                if (questOrder == 1)
                {
                    questObject[0].SetActive(true);
                }
                break;
            case 20:
                {
                    if (questOrder == 1)
                    {
                        questObject[0].SetActive(false);
                        questObject[1].SetActive(true);
                        bossHpBar.SetActive(true);
                    }
                }
                break;
            case 30:
                break;
        }
    }
}
