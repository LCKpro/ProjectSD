using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    private Dictionary<int, string[]> talkData;

    private Dictionary<int, Material> faceMatData_NPC;

    public Material[] faceMat;

    public static TalkManager instance;

    public string[] talkDialogList;

    public Animator npcAnim;

    /*"�̰� ��¼��..:5, 
      "���� ��� ��ٷ���! �� ��Ź �� ����� �� �־�?:4",
      "��� �Ʊ� ������ ���ؼ� �����ĿԴµ�.. �ƹ����� �� �� ������ �Ҿ���� �� ����:5",
      "�ű⿣ �� ������ ������ ���� ������ �־�.. ������ ��Ź������ �̷��� ��Ź�Ұ�!!:0"
    */

    void Awake()
    {
        instance = this;
        talkData = new Dictionary<int, string[]>();
        faceMatData_NPC = new Dictionary<int, Material>();
        GenerateData();
    }

    void GenerateData()
    {
        talkData.Add(1000, talkDialogList);

        faceMatData_NPC.Add(1000 + 0, faceMat[0]);  // �⺻ 1
        faceMatData_NPC.Add(1000 + 1, faceMat[1]);  // ��ȭ 2
        faceMatData_NPC.Add(1000 + 2, faceMat[2]);  // �ù��� 5
        faceMatData_NPC.Add(1000 + 3, faceMat[3]);  // �ʷ��ʷ� 6
        faceMatData_NPC.Add(1000 + 4, faceMat[4]);  // �ҽ� 9
        faceMatData_NPC.Add(1000 + 5, faceMat[5]);  // ��Ȥ���� 13
        faceMatData_NPC.Add(1000 + 6, faceMat[6]);  // �ų� 14
        faceMatData_NPC.Add(1000 + 7, faceMat[7]);  // ȭ�� 16
        faceMatData_NPC.Add(1000 + 8, faceMat[8]);  // ������ 21
    }

    public string GetTalk(int id, int talkIndex)
    {
        Debug.Log("GetTalk" + id + " " + talkIndex);

        // �츮�� ��ųʸ��� ���� Ű���� �ֳ� ����?!
        if (!talkData.ContainsKey(id))
        {
            if (!talkData.ContainsKey(id - id % 10))
            {
                //����Ʈ �� ó�� ��縶�� ���� �� �⺻ ��縦 ��������
                return GetTalk(id - id % 100, talkIndex);
            }
            else
            {
                //�ε��� ������ �̹� ���� ��ȭ. null ��ȯ // �� �� ��ȭ�� �̵�
                return GetTalk(id - id % 10, talkIndex);
            }
        }
        if (talkIndex == talkData[id].Length)
        {
            if (id == 1000)
            {
                GamePlay.Instance.cameraManager.ChangeCam_TargetToPlayer();
            }
            Debug.Log("Null");
            return null;
        }
        else
        {
            if(id == 1000)
            {
                SetDirection(talkIndex);
            }

            return talkData[id][talkIndex];
        }

    }

    public Material GetNPCFaceMat(int id, int portraitIndex)
    {
        Debug.Log("GetNPCFaceMat : " + id + " index : " + portraitIndex);
        return faceMatData_NPC[id + portraitIndex];
    }

    public void SetDirection(int index)
    {
        if (index == 0)
        {
            npcAnim.SetInteger("animation", 34);
            GamePlay.Instance.cameraManager.ChangeCam_Sky();
        }
        else if (index == 5)
        {
            SoundManager.instance.PlaySound("Break");
        }
        else if (index == 8)
        {
            GamePlay.Instance.cameraManager.ChangeCam_NPC();
        }
        else if (index == 21)
        {
            npcAnim.SetInteger("animation", 42);
            SoundManager.instance.PlaySound("Propose");
        }
        else if (index == 22)
        {
            npcAnim.SetInteger("animation", 34);
        }
    }
}
