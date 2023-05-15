using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
