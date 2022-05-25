﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public GameObject[] questObject;
    public int questId;
    public int questOrder;      // 퀘스트 대화 순서

    Dictionary<int, QuestData> questList;

    // 싱글톤
    private static QuestManager instance = null;
    public static QuestManager Instance
    {
        get
        {
            if (null == instance)
                instance = new QuestManager();
            return instance;
        }
    }

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }

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
                                        new int[] {2000, 1000}));

        questList.Add(40, new QuestData("NPC와의 대화",
                                        new int[] {2000, 1000}));

        questList.Add(50, new QuestData("엔딩 분기 시작",
                                        new int[] {2000, 2000, 2000}));

        questList.Add(60, new QuestData("엔딩을 향해서",
                                        new int[] {0}));
    }

    public int GetQuestIndex(int id)
    {
        return questId + questOrder;
    }

    public string CheckQuest(int id)
    {
        if (id == questList[questId].npcId[questOrder])
        {
            questOrder++;
            Debug.Log(questList[questId].questName);      // 퀘스트 오더가 늘어 났을때 발생
        }

        if (questOrder == questList[questId].npcId.Length)
        {
            NextQuest();
            Debug.Log(questList[questId].questName);      // 다음 퀘스트가 진행 됐을때 발생
        }

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
                break;
            case 20:
                {
                    if (questOrder == 1)    // 진행도
                    {
                        SpawnManager.Instance.SpawnBoss();
                    }
                }
                break;
            case 30:
                break;
            case 40:
                break;
            case 50:
                break;
            case 60:
                break;
            case 70:
                break;
        }
    }
}