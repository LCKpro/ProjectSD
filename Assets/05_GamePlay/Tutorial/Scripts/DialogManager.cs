using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public Animator talkBox;
    public TextMeshProUGUI nameText;
    public TypeEffect talkText;

    public bool isAction;
    public int talkIndex;

    public static DialogManager instance;

    public SkinnedMeshRenderer npcRenderer;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Debug.Log(QuestManager.instance.CheckQuest());
    }

    public void Action()
    {
        Talk(1000);

        // UI대화창 On/Off
        talkBox.SetBool("isShow", isAction);
    }

    void Talk(int id)
    {
        int questTalkIndex;
        string talkData;

        /*if (TypeEffect.instance.isAnim)
        {
            TypeEffect.instance.SetMsg("");
            return;
        }*/
        if (talkText.isAnim)
        {
            talkText.SetMsg("");
            return;
        }
        else
        {
            questTalkIndex = QuestManager.instance.GetQuestTalkIndex(id);
            talkData = TalkManager.instance.GetTalk(id + questTalkIndex, talkIndex);
        }

        // TalkManager에서 인덱스 끝까지 도착했으면 null 반환 = 대화 끝
        if (talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            Debug.Log(QuestManager.instance.CheckQuest(id));
            return;
        }

        nameText.text = talkData.Split(':')[0];
        talkText.SetMsg(talkData.Split(':')[1]);    // 구분자 넣어서 표정 세팅 가능하게
        npcRenderer.materials[1] = TalkManager.instance.GetNPCFaceMat(id, int.Parse(talkData.Split(':')[2]));   // 표정 세팅

        // UI창 보이기
        isAction = true;
        // 다음 대화
        talkIndex++;
    }
}
