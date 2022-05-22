using System.Collections;
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

    // 대사 시작
    // TODO : 대사를 더 추가하면서 동시에 적 오브젝트들을 멈추고 대사는 진행이 되어야 함
    void GenerateData()
    {
        talkData.Add(1, new string[] {"훌쩍...", "언제 오는 거야.."});
    }

    public string GetTalk(int id, int talkIndex)
    {
        // 대화가 끝났나?
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }
}
