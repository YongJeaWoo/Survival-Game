﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkManager : MonoBehaviour
{
    // 대화 값 넣기
    Dictionary<int, string[]> talkData;

    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();             // 빈 공간 생성
        GenerateData();
    }

    // 대화 시작
    void GenerateData()
    {
        // 일반 대화
        talkData.Add(1000, new string[] {"빨리 돌아가야 해.."});              
        talkData.Add(2000, new string[] {"훌쩍...", "언제 오는 거야.."});  // 동생

        // 퀘스트 대화
        talkData.Add(1000 + 10, new string[] {"으악! 깜짝이야.", "맞다. 혹시 한 여자아이를 못 봤어? : 0",
                                        "내 여동생인데 그만 헤어지게 되어버렸어.. : 0",
                                        "동생이 많이 아파서 뛰지도 못하는 상황인데.. : 0",
                                        "혹시 찾으면 나에게 찾아올 수 있을까? : 0",
                                        "호텔을 기준으로 1시방향쪽에 우리집이 있어. 아마 그 근처에 있을거야. : 0"});

        talkData.Add(1000 + 10 + 1, new string[] {"어서 가 줘. : 0",
                                                  "난 여기서 무기를 좀 찾아봐야겠어. : 0"});

        talkData.Add(2000 + 20, new string[] {"그쪽은 누구야..? : 1",
                                            "나를 찾고 있는 사람이 있다고? : 1",
                                            "이 상황에 찾는 건 아마 내 오빠겠지? : 1",
                                            "하지만 난 다리를 다쳐서 거기까지 같이 가다가 괜히 발목만 잡을 거야. : 1",
                                            "난 여기 숨어 있을 테니 우리 오빠를 데리고 같이 와 줬으면 해. : 1",
                                            "맞다! 가는 길에 이 마을을 나갈 수 있는 유일한 길목에 큰 괴물이 있어.. : 1",
                                            "그 괴물을 조심해. : 1"});

        talkData.Add(2000 + 20 + 1, new string[] { "난 어서 들어가봐야겠어. 내 걱정은 하지 마. : 1" });

        talkData.Add(1000 + 30, new string[] { "보이지 않는다." });
        talkData.Add(2000 + 30, new string[] {"싸늘한 시체만이 나를 반기고 있다."});
    }

    // 대화를 받기
    public string GetTalk(int id, int talkIndex)
    {
        if (!talkData.ContainsKey(id))
        {
            // 퀘스트 진행 순서 대사가 없을 경우
            // 퀘스트 맨 처음 대사 가지고 오기
            // 아니면 기존 대사 가져오기
            if (!talkData.ContainsKey(id - id % 10))
                return GetTalk(id - id % 100, talkIndex);
            else
                return GetTalk(id - id % 10, talkIndex);
        }

        // 대화가 끝났나?
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }
}
