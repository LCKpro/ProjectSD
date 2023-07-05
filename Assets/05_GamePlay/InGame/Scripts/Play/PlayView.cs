using UnityEngine;
using TMPro;

public class PlayView : MonoBehaviour
{
    #region SingleTon

    private static PlayView _instance = null;

    public static PlayView Instance
    {
        get
        {
            SetInstance();
            return _instance;
        }
    }

    public static void SetInstance()
    {
        if (_instance == null)
        {
            GameObject go = GameObject.Find("PlayView");
            if (go != null)
            {
                _instance = go.GetComponent<PlayView>();
                if (_instance == null)
                {
                    Debug.Log("PlayView Instance Null");
                }
            }
        }
    }

    void OnDestroy()
    {
        _instance = null;
    }

    void OnDisable()
    {
        _instance = null;
    }

    #endregion

    public GameObject ui_Top_Goods;
    public GameObject ui_Clock;
    public GameObject ui_Btn_Grid;
    public GameObject ui_System_Grid;

    public TextMeshProUGUI ui_Text_Gold;
    public TextMeshProUGUI ui_Text_Point;

    public void SetActiveUI(bool isAcive)
    {
        ui_Top_Goods.SetActive(isAcive);
        ui_Clock.SetActive(isAcive);
        ui_Btn_Grid.SetActive(isAcive);
        ui_System_Grid.SetActive(isAcive);
    }

    public void OnClick_Setting()
    {
        SoundManager.instance.PlaySound("NormalClick");
        Core.Instance.uiPopUpManager.Show("UI_Setting");
    }

    public void SetUI_Gold(int gold)
    {
        ui_Text_Gold.text = gold.ToString();
    }

    public void SetUI_Point(int point)
    {
        ui_Text_Point.text = point.ToString();

    }
}
