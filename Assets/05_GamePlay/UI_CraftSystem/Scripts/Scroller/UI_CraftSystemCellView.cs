using System;
using System.Collections.Generic;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftSystemCellView : EnhancedScrollerCellView
{
    public Animator anim;

    public TextMeshProUGUI text_Title;
    public Image icon_Building;
    public UI_CraftItem[] craftItemArray;

    private string craftKey;
    private int _index;
    private int typeKey;

    public GameObject infoObj;
    public TextMeshProUGUI text_Info;

    public void SetData(Dictionary<string, object> data, int index)
    {
        infoObj.SetActive(false);   // �����ڽ��� ��������

        _index = index;
        craftKey = Convert.ToString(data["ID"]);
        typeKey = Convert.ToInt32(data["Type"]);

        foreach (var item in craftItemArray)
        {
            item.gameObject.SetActive(false);
        }

        text_Title.text = Convert.ToString(data["NameText_Key"]);
        icon_Building.sprite = Resources.Load<Sprite>(Convert.ToString(data["Icon"]));

        var ingredient = Convert.ToString(data["Ingredient"]);
        var quantity = Convert.ToString(data["Quantity"]);

        if(ingredient.Contains("@") == true)
        {
            var split = ingredient.Split("@");

            for (int i = 0; i < split.Length; i++)
            {
                craftItemArray[i].SetData(split[i], quantity.Split("@")[i]);
                craftItemArray[i].gameObject.SetActive(true);
            }
        }
        else
        {
            craftItemArray[0].SetData(ingredient, quantity);
            craftItemArray[0].gameObject.SetActive(true);
        }

        text_Info.text = Convert.ToString(data["Description_Key"]);
    }

    // ��ư ������ ũ������ �����ϵ���
    public void OnClick_InitCraft()
    {
        GamePlay.Instance.ui_System.HideUIImmediately();
        GamePlay.Instance.craftingManager.InitCraft(typeKey, _index);
    }

    public void OnClick_OpenInfo()
    {
        anim.SetTrigger("Rotate");
        Invoke("ActiveInfoBtn", 0.2f);
    }

    public void OnClick_CloseInfo()
    {
        anim.SetTrigger("Rotate");
        Invoke("DeActiveInfoBtn", 0.2f);
    }

    public void ActiveInfoBtn()
    {
        infoObj.SetActive(true);
    }
    public void DeActiveInfoBtn()
    {
        infoObj.SetActive(false);
    }

}
