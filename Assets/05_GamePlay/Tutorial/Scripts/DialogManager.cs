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

    public GameObject tutorial_Actor;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CheckTutorial();
    }

    private void CheckTutorial()
    {
        /*if(PlayerPrefs.GetInt("Tutorial") == 0)
        {
            Debug.Log("튜토리얼 시작. 주석처리해서 튜토리얼은 안뜸");
            PlayView.Instance.SetActiveUI(false);
            tutorial_Actor.SetActive(true);
            TalkAction();
        }
        else
        {
            GamePlay.Instance.uI_DayNightSystem.Init();
        }*/

        // 빌드 때 여기 이닛은 지워야 함
        GamePlay.Instance.uI_DayNightSystem.Init();
        SoundManager.instance.PlayBGM("Day");
        /*PlayView.Instance.SetActiveUI(true);
        GamePlay.Instance.uI_DayNightSystem.Init();
        tutorial_Actor.SetActive(false);*/
    }

    public void Cheat_TalkAction()
    {
        tutorial_Actor.SetActive(true);
        TalkAction();
    }

    public void TalkAction()
    {
        if (PlayerPrefs.GetInt("Tutorial") == 1)
        {
            return;
        }

        PlayView.Instance.SetActiveUI(false);

        Talk(1000);

        // UI대화창 On/Off
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

        // TalkManager에서 인덱스 끝까지 도착했으면 null 반환 = 대화 끝
        if (talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            GamePlay.Instance.cameraManager.ChangeCam_Day();
            PlayerPrefs.SetInt("Tutorial", 1);
            PlayView.Instance.SetActiveUI(true);
            GamePlay.Instance.gameStateManager.SetStateType(GameDefine.StateType.None);
            GamePlay.Instance.uI_DayNightSystem.Init();
            talkBox.gameObject.SetActive(false);
            return;
        }

        Debug.Log("talkData : " + talkData);
        nameText.text = talkData.Split(':')[0];
        talkText.SetMsg(talkData.Split(':')[1]);    // 구분자 넣어서 표정 세팅 가능하게
        //npcRenderer.materials[1] = TalkManager.instance.GetNPCFaceMat(id, int.Parse(talkData.Split(':')[2]));   // 표정 세팅
        npcRenderer.materials[1].CopyPropertiesFromMaterial(TalkManager.instance.GetNPCFaceMat(id, int.Parse(talkData.Split(':')[2])));   // 표정 세팅

        // UI창 보이기
        isAction = true;
        // 다음 대화
        talkIndex++;
    }
}
