using UnityEngine;
using UnityEngine.Events;

public class UIPopUpVisible : MonoBehaviour
{
    public UnityEvent ShowCallback = null;
    public UnityEvent HideCallback = null;

    public void Show()
    {
        if (ShowCallback != null)
        {
            ShowCallback.Invoke();
            //			Observable.TimerFrame (1).Subscribe (_ => {
            //			});
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        if (HideCallback != null)
        {
            HideCallback.Invoke();
            //			Observable.TimerFrame (1).Subscribe (_ => {
            //			});
        }
    }
}