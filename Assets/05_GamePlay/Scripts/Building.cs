using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField]
    private BuildingSort buildingSort;

    [SerializeField]
    private int buildingLevel;

    [SerializeField]
    private int maxItemValue = 2;

    [SerializeField]
    private int minItemValue = 0;

    [SerializeField]
    private bool isRootable;

    private Dictionary<int, int> itemList;


    private void Start()
    {
        itemList = new Dictionary<int, int>();
        Init();
    }

    // 빌딩에 들어갈 아이템 설정하기
    private void Init()
    {
        var list = Core.Instance.itemManager.uuidList;

        int rndCount = Random.Range(minItemValue, maxItemValue);

        for (int i = 0; i < rndCount; i++)
        {
            int rndItemCount = Random.Range(1, 10);

            int itemUuid = GameUtils.RandomItem(list);

            if(itemList.ContainsKey(itemUuid) == true)
            {
                itemList[itemUuid] += rndItemCount;
            }
            else
            {
                itemList.Add(itemUuid, rndItemCount);
            }
        }

        foreach (var item in itemList)
        {
            Debug.Log("Key : " + item.Key);
            Debug.Log("Value : " + item.Value);
        }
    }
}
