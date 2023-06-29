using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId = 0;
    public int questActionIndex = 0;
    private Dictionary<int, QuestData> questList;

    public static QuestManager instance;
    void Awake()
    {
        instance = this;
        questList = new Dictionary<int, QuestData>();

        GenerateData();
    }

    void GenerateData()
    {
        questList.Add(0, new QuestData("아치와 만남", new int[] { 1000 }));
        questList.Add(10, new QuestData("퀘스트 끝", new int[] { 0 }));
    }

    public int GetQuestTalkIndex(int id)
    {
        return questId + questActionIndex;
    }

    public string CheckQuest(int id)
    {
        if (id == questList[questId].npcId[questActionIndex])
            questActionIndex++;

        // 퀘스트액션 인덱스가 배열크기와 같을 경우(퀘스트 끝까지 도달했을 때) 넘어감
        if (questActionIndex == questList[questId].npcId.Length)
            NextQuest();

        return questList[questId].questName;
    }

    // 오버로딩. 매개변수에 따라 다른 함수 호출
    public string CheckQuest()
    {
        return questList[questId].questName;
    }

    void NextQuest()
    {
        Debug.Log("다음으로.");
        questId += 10;
        questActionIndex = 0;
    }
}
