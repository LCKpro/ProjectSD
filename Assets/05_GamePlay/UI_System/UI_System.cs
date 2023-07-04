using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_System : MonoBehaviour
{
    public UI_CraftSystem craftSystem;

    public GameObject btn_ATK;
    public GameObject btn_DFS;
    public GameObject btn_PD;
    public GameObject btn_OA;

    /// <summary>
    ///  �ý��� UI ��ư �� ����
    /// </summary>
    private void SetActiveFalse()
    {
        btn_ATK.SetActive(false);
        btn_DFS.SetActive(false);
        btn_PD.SetActive(false);
        btn_OA.SetActive(false);
    }

    /// <summary>
    /// ũ������ ��ư Ŭ������ ��
    /// </summary>
    public void OnClick_StartCraftSystem()
    {
        SetActiveFalse();
        btn_ATK.SetActive(true);

        craftSystem.OnCraft(BuildingType.ATK);
    }

    public void OnClick_CraftATK()
    {
        if(btn_ATK.activeSelf == true)
        {
            return;
        }

        SetActiveFalse();
        btn_ATK.SetActive(true);
        craftSystem.OnCraft(BuildingType.ATK);
    }

    public void OnClick_CraftDFS()
    {
        if (btn_DFS.activeSelf == true)
        {
            return;
        }

        SetActiveFalse();
        btn_DFS.SetActive(true);
        craftSystem.OnCraft(BuildingType.DFS);
    }

    public void OnClick_CraftPD()
    {
        if (btn_PD.activeSelf == true)
        {
            return;
        }

        SetActiveFalse();
        btn_PD.SetActive(true);
        craftSystem.OnCraft(BuildingType.PD);
    }

    public void OnClick_CraftOA()
    {
        if (btn_OA.activeSelf == true)
        {
            return;
        }

        SetActiveFalse();
        btn_OA.SetActive(true);
        craftSystem.OnCraft(BuildingType.OA);
    }

    // UI �ٷ� ���� 
    public void HideUIImmediately()
    {
        transform.gameObject.SetActive(false);
    }

    public void OnClick_Hide()
    {
        SoundManager.instance.PlaySound("NormalClick");
        GamePlay.Instance.gameStateManager.SetStateType(GameDefine.StateType.None);
        transform.gameObject.SetActive(false);
    }
}
