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
        talkData.Add(0, new string[] { "으악! 깜짝이야", "맞다. 혹시 한 여자아이를 못 봤어?", 
                                        "내 여동생인데 그만 헤어지게 되어버렸어..", 
                                        "동생이 많이 아파서 잘 뛰지도 못하는 아인데..",
                                        "혹시 찾으면 나에게 찾아올 수 있을까?"});
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
