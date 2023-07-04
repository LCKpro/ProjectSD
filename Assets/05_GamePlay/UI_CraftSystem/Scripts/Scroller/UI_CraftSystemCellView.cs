using System;
using System.Collections.Generic;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameCreator.Inventory;

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
    public GameObject deActiveObj;

    private Dictionary<string, string> ingredientDic = new Dictionary<string, string>();

    public void SetData(Dictionary<string, object> data, int index)
    {
        ingredientDic.Clear();
        infoObj.SetActive(false);   // 인포박스는 가려주자
        deActiveObj.SetActive(false);

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
                ingredientDic.Add(split[i], quantity.Split("@")[i]);
                craftItemArray[i].SetData(split[i], quantity.Split("@")[i]);
                craftItemArray[i].gameObject.SetActive(true);
            }
        }
        else
        {
            ingredientDic.Add(ingredient, quantity);
            craftItemArray[0].SetData(ingredient, quantity);
            craftItemArray[0].gameObject.SetActive(true);
        }

        text_Info.text = Convert.ToString(data["Description_Key"]);

        // 구매 가능하면 디액티브가 보이면 안됨
        deActiveObj.SetActive(!CheckPurchase());
    }

    // 버튼 누르면 크래프팅 가능하도록
    public void OnClick_InitCraft()
    {
        if(CheckPurchase() == false)
        {
            Debug.Log("구매 불가");
            return;
        }

        SoundManager.instance.PlaySound("NormalClick");
        ConsumeItem();
        GamePlay.Instance.gameStateManager.SetStateType(GameDefine.StateType.Build);
        GamePlay.Instance.ui_System.HideUIImmediately();
        GamePlay.Instance.craftingManager.InitCraft(typeKey, _index);
    }

    private bool CheckPurchase()
    {
        foreach (var ingre in ingredientDic)
        {
            switch (ingre.Key)
            {
                case "Item_ETC_Gold":
                    {
                        return GetAmount(1834752059) >= Convert.ToInt32(ingre.Value);
                    }
                case "Item_ETC_Point":
                    {
                        return GetAmount(750108221) >= Convert.ToInt32(ingre.Value);
                    }
                case "Item_ETC_Wood":
                    {
                        return GetAmount(25686138) >= Convert.ToInt32(ingre.Value);
                    }
                case "Item_ETC_Stone":
                    {
                        return GetAmount(816283316) >= Convert.ToInt32(ingre.Value);
                    }
                case "Item_ETC_Diamond":
                    {
                        return GetAmount(1273281591) >= Convert.ToInt32(ingre.Value);
                    }
                case "Item_ETC_Oil":
                    {
                        return GetAmount(1379574024) >= Convert.ToInt32(ingre.Value);
                    }
                default:
                    break;
            }
        }

        return false;
    }

    private void ConsumeItem()
    {
        foreach (var ingre in ingredientDic)
        {
            switch (ingre.Key)
            {
                case "Item_ETC_Gold":
                    {
                        InventoryManager.Instance.SubstractItemFromInventory(1834752059, Convert.ToInt32(ingre.Value));
                    }
                    break;
                case "Item_ETC_Point":
                    {
                        InventoryManager.Instance.SubstractItemFromInventory(750108221, Convert.ToInt32(ingre.Value));
                    }
                    break;
                case "Item_ETC_Wood":
                    {
                        InventoryManager.Instance.SubstractItemFromInventory(25686138, Convert.ToInt32(ingre.Value));
                    }
                    break;
                case "Item_ETC_Stone":
                    {
                        InventoryManager.Instance.SubstractItemFromInventory(816283316, Convert.ToInt32(ingre.Value));
                    }
                    break;
                case "Item_ETC_Diamond":
                    {
                        InventoryManager.Instance.SubstractItemFromInventory(1273281591, Convert.ToInt32(ingre.Value));
                    }
                    break;
                case "Item_ETC_Oil":
                    {
                        InventoryManager.Instance.SubstractItemFromInventory(1379574024, Convert.ToInt32(ingre.Value));
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private int GetAmount(int uuid)
    {
        return InventoryManager.Instance.GetInventoryAmountOfItem(uuid);
    }

    public void OnClick_OpenInfo()
    {
        SoundManager.instance.PlaySound("NormalClick");
        anim.SetTrigger("Rotate");
        Invoke("ActiveInfoBtn", 0.2f);
    }

    public void OnClick_CloseInfo()
    {
        SoundManager.instance.PlaySound("NormalClick");
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
