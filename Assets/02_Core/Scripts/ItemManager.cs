using System.Collections.Generic;
using UnityEngine;
using GameCreator.Inventory;

public class ItemManager : MonoBehaviour
{
    [SerializeField]
    private List<ItemData> itemDataList;

    public List<ItemData> GetItemData()
    {
        if(itemDataList != null)
        {
            return itemDataList;
        }

        GameUtils.Log("ItemData Null");

        return null;
    }

    public ItemData GetItemDataByUUID(int uuid)
    {
        foreach (var item in itemDataList)
        {
            if(item.uuid == uuid)
            {
                return item;
            }
        }

        return null;
    }

    public void InitItemData()
    {
        foreach (var data in itemDataList)
        {
            var amount = PlayerPrefs.GetInt(data.uuid.ToString());

            if(amount == 0)
            {
                continue;
            }

            InventoryManager.Instance.AddItemToInventory(data.uuid, amount);
            Debug.Log("아이템 획득 : " + data.uuid + " 개수 : " + amount);
        }
    }

    public void SaveItemData()
    {
        foreach (var data in itemDataList)
        {
            int itemAmount = InventoryManager.Instance.GetInventoryAmountOfItem(data.uuid);

            if (itemAmount == 0)
            {
                continue;
            }

            PlayerPrefs.SetInt(data.uuid.ToString(), itemAmount);

            Debug.Log("아이템 저장 : " + data.uuid + " 개수 : " + itemAmount);
        }
    }
}

[System.Serializable]
public class ItemData
{
    public int uuid;
    public string itemName;
    public string description;
    public Sprite sprite;
    public int price;
    public int maxStack;
    public int weight;
}
