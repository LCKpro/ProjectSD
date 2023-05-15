using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UI_PopUp_BuildingRoot : MonoBehaviour
{
    [SerializeField]
    private GameObject[] toggleObject;

    [SerializeField]
    private Image[] slotImage;

    [SerializeField]
    private TextMeshProUGUI[] slotCount;

    private Dictionary<int, int> _itemList;

    private ItemManager _itemManager;

    public void Init(Dictionary<int, int> itemList)
    {
        foreach (var item in itemList)
        {
            Debug.Log("UUID : " + item.Key + " Count : " + item.Value);
        }

        _itemManager = Core.Instance.itemManager;
        _itemList = itemList;
        SetToggle();
    }

    private void SetToggle()
    {
        if (_itemList.Count < 1)
        {
            GameUtils.Log("UI_PopUp_BuildingRoot", "ItemList Count 0");
            return;
        }

        foreach (var toggle in toggleObject)
        {
            toggle.SetActive(false);
        }

        int i = 0;

        foreach (var item in _itemList)
        {
            if (i > _itemList.Count)
            {
                break;
            }

            // 토글 켜기
            toggleObject[i].SetActive(true);

            // 아이템 매니저에서 아이템 데이터 불러오기
            var data = _itemManager.GetItemDataByUUID(item.Key);

            // 이미지
            slotImage[i].sprite = data.sprite;

            // 개수
            slotCount[i].text = Convert.ToString(item.Value);

            i++;
        }
    }

    public void OnClick_Close()
    {
        Core.Instance.uiPopUpManager.Hide();
    }
}
