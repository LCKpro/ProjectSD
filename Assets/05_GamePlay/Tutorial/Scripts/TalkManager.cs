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

    /*"이걸 어쩌지..:5, 
      "저기 잠깐만 기다려줘! 내 부탁 좀 들어줄 수 있어?:4",
      "사실 아까 괴물을 피해서 도망쳐왔는데.. 아무래도 그 때 지갑을 잃어버린 것 같아:5",
      "거기엔 내 소중한 사람들과 찍은 사진이 있어.. 무리한 부탁이지만 이렇게 부탁할게!!:0"
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

        faceMatData_NPC.Add(1000 + 0, faceMat[0]);  // 기본 1
        faceMatData_NPC.Add(1000 + 1, faceMat[1]);  // 대화 2
        faceMatData_NPC.Add(1000 + 2, faceMat[2]);  // 시무룩 5
        faceMatData_NPC.Add(1000 + 3, faceMat[3]);  // 초롱초롱 6
        faceMatData_NPC.Add(1000 + 4, faceMat[4]);  // 불신 9
        faceMatData_NPC.Add(1000 + 5, faceMat[5]);  // 매혹당함 13
        faceMatData_NPC.Add(1000 + 6, faceMat[6]);  // 신남 14
        faceMatData_NPC.Add(1000 + 7, faceMat[7]);  // 화남 16
        faceMatData_NPC.Add(1000 + 8, faceMat[8]);  // 힝힝구 21
    }

    public string GetTalk(int id, int talkIndex)
    {
        Debug.Log("GetTalk" + id + " " + talkIndex);

        // 우리가 딕셔너리에 넣은 키값이 있냐 없냐?!
        if (!talkData.ContainsKey(id))
        {
            if (!talkData.ContainsKey(id - id % 10))
            {
                //퀘스트 맨 처음 대사마저 없을 때 기본 대사를 가져오자
                return GetTalk(id - id % 100, talkIndex);
            }
            else
            {
                //인덱스 같으면 이미 끝난 대화. null 반환 // 그 전 대화로 이동
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
