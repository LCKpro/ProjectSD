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
        questList.Add(0, new QuestData("��ġ�� ����", new int[] { 1000 }));
        questList.Add(10, new QuestData("����Ʈ ��", new int[] { 0 }));
    }

    public int GetQuestTalkIndex(int id)
    {
        return questId + questActionIndex;
    }

    public string CheckQuest(int id)
    {
        if (id == questList[questId].npcId[questActionIndex])
            questActionIndex++;

        // ����Ʈ�׼� �ε����� �迭ũ��� ���� ���(����Ʈ ������ �������� ��) �Ѿ
        if (questActionIndex == questList[questId].npcId.Length)
            NextQuest();

        return questList[questId].questName;
    }

    // �����ε�. �Ű������� ���� �ٸ� �Լ� ȣ��
    public string CheckQuest()
    {
        return questList[questId].questName;
    }

    void NextQuest()
    {
        Debug.Log("��������.");
        questId += 10;
        questActionIndex = 0;
    }
}
