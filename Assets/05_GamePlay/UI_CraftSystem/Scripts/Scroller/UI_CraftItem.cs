using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_CraftItem : MonoBehaviour
{
    public Image icon_Goods;
    public TextMeshProUGUI text_Goods;

    public void SetData(string iconPath, string count)
    {
        icon_Goods.sprite = Resources.Load<Sprite>("UI/Icons/Icon_" + iconPath);

        text_Goods.text = count;
    }
}
