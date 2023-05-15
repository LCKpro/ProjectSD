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

            // ��� �ѱ�
            toggleObject[i].SetActive(true);

            // ������ �Ŵ������� ������ ������ �ҷ�����
            var data = _itemManager.GetItemDataByUUID(item.Key);

            // �̹���
            slotImage[i].sprite = data.sprite;

            // ����
            slotCount[i].text = Convert.ToString(item.Value);

            i++;
        }
    }

    public void OnClick_Close()
    {
        Core.Instance.uiPopUpManager.Hide();
    }
}
