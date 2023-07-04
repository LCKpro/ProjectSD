using UnityEngine;
using TMPro;
using KoreanTyper;

public class TypeEffect : MonoBehaviour
{
    // 글자 재생 속도 변수 (초당 몇 글자)
    public int charPerSeconds;
    public GameObject endCursor;

    private string targetMsg;
    public TextMeshProUGUI msgText;
    public AudioSource audioSource;
    private int index;
    private float interval;
    public bool isAnim;

    public static TypeEffect instance;

    private void Awake()
    {
        instance = this;
    }

    public void SetMsg(string msg)
    {
        if (isAnim)
        {
            msgText.text = targetMsg;
            CancelInvoke("Effecting");
            EffectEnd();
        }
        else
        {
            targetMsg = msg;
            EffectStart();
        }
    }

    private int typingLength = 0;
    void EffectStart()
    {
        msgText.text = "";
        index = 0;
        endCursor.SetActive(false);

        //애니메이션 시작
        interval = 1.0f / charPerSeconds;
        isAnim = true;

        typingLength = targetMsg.GetTypingLength();

        Invoke("Effecting", interval);
    }

    void Effecting()
    {
        if(index > typingLength)
        {
            EffectEnd();
            return;
        }

        /*if (msgText.text == targetMsg)
        {
            EffectEnd();
            return;
        }*/

        msgText.text = targetMsg.Typing(index);

        //msgText.text += targetMsg[index];
        // 텍스트 사운드
        //if (targetMsg[index] != ' ' || targetMsg[index] != '.')
        audioSource.Play();

        index++;
        // 끝날 때 까지 다시 호출
        Invoke("Effecting", interval);
    }

    void EffectEnd()
    {
        isAnim = false;
        endCursor.SetActive(true);
    }
}
