using UnityEngine;

public class QuestData : MonoBehaviour
{
    public string questName;
    public int[] npcId;

    public QuestData(string name, int[] npc)
    {
        questName = name;
        npcId = npc;
    }
}
