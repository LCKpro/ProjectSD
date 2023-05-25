using System;
using System.Collections.Generic;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftSystemCellView : EnhancedScrollerCellView
{
    public TextMeshProUGUI text_Title;
    public Image icon_Building;
    public UI_CraftItem[] craftItemArray;


    public void SetData(Dictionary<string, object> data)
    {
        foreach (var item in craftItemArray)
        {
            item.gameObject.SetActive(false);
        }

        text_Title.text = Convert.ToString(data["NameText_Key"]);
        Debug.Log(data["NameText_Key"]);
        icon_Building.sprite = Resources.Load<Sprite>(Convert.ToString(data["Icon"]));

        var ingredient = Convert.ToString(data["Ingredient"]);
        var quantity = Convert.ToString(data["Quantity"]);

        Debug.Log(ingredient);

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
    }
}
