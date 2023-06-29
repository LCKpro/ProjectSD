using UnityEngine;
using TMPro;
using KoreanTyper;

public class TypeEffect : MonoBehaviour
{
    // ���� ��� �ӵ� ���� (�ʴ� �� ����)
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

        //�ִϸ��̼� ����
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
        // �ؽ�Ʈ ����
        //if (targetMsg[index] != ' ' || targetMsg[index] != '.')
        audioSource.Play();

        index++;
        // ���� �� ���� �ٽ� ȣ��
        Invoke("Effecting", interval);
    }

    void EffectEnd()
    {
        isAnim = false;
        endCursor.SetActive(true);
    }
}
