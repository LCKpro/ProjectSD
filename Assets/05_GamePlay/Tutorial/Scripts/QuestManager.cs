using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId = 0;
    public int questActionIndex = 0;
    private Dictionary<int, QuestData> questList;
    bool isSpawn = false;
    bool isHelp = false;

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

        if (questId == 20 && !isHelp)
        {
            //GameManager.instance.helpText.SetActive(true);
            //GameManager.instance.helpText.GetComponent<FadeText>().HideReddy(5.0f);
            isHelp = true;
        }

        if (questId == 30 && !isSpawn)
        {
            //GameManager.instance.rikayon.SetActive(true);
            //GameManager.instance.SubCam();
            isSpawn = true;
        }

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
