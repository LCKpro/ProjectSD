using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Portal_CatTower : MonoBehaviour
{
    public Animator anim;
    public string title_Text;
    public string desc_Text;

    public TextMeshProUGUI title_TMP;
    public TextMeshProUGUI desc_TMP;

    private void OnEnable()
    {
        GamePlay.Instance.cameraManager.ChangeCam_PlayerToTarget();
        anim.SetTrigger("Pop");
    }

    public void OnClick_Enter()
    {
        OnClick_Exit();
        GamePlay.Instance.farmingManager.OnClick_GoToFarm();
    }

    public void OnClick_Exit()
    {
        SoundManager.instance.PlaySound("NormalClick");
        Time.timeScale = 1;
        Core.Instance.uiPopUpManager.Hide("UI_Portal_CatTower");
        GamePlay.Instance.cameraManager.ChangeCam_Day();
    }
}
