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

    public void TalkAction()
    {
        Talk(1000);

        // UI��ȭâ On/Off
        talkBox.SetBool("isShow", isAction);
    }

    void Talk(int id)
    {
        GamePlay.Instance.gameStateManager.SetStateType(GameDefine.StateType.Tutorial);
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

            if(talkIndex == 5 && id == 1000)
            {
                talkBox.SetTrigger("Shake");
            }
        }

        // TalkManager���� �ε��� ������ ���������� null ��ȯ = ��ȭ ��
        if (talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            Debug.Log(QuestManager.instance.CheckQuest(id));
            GamePlay.Instance.gameStateManager.SetStateType(GameDefine.StateType.None);
            return;
        }

        Debug.Log("talkData : " + talkData);
        nameText.text = talkData.Split(':')[0];
        talkText.SetMsg(talkData.Split(':')[1]);    // ������ �־ ǥ�� ���� �����ϰ�
        //npcRenderer.materials[1] = TalkManager.instance.GetNPCFaceMat(id, int.Parse(talkData.Split(':')[2]));   // ǥ�� ����
        npcRenderer.materials[1].CopyPropertiesFromMaterial(TalkManager.instance.GetNPCFaceMat(id, int.Parse(talkData.Split(':')[2])));   // ǥ�� ����

        // UIâ ���̱�
        isAction = true;
        // ���� ��ȭ
        talkIndex++;
    }
}
