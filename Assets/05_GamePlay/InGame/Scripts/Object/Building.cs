using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Building : Stat
{
    [SerializeField]
    private GameDefine.BuildingSort buildingSort;

    [SerializeField]
    private int buildingLevel;

    [SerializeField]
    private int maxItemValue = 2;

    [SerializeField]
    private int minItemValue = 0;

    [SerializeField]
    private bool isRootable;

    private Dictionary<int, int> _itemList;

    private bool isClicked = false;

    private void Start()
    {
        _itemList = new Dictionary<int, int>();
        Init();
    }

    // ������ �� ������ �����ϱ�
    private void Init()
    {
        var list = Core.Instance.itemManager.GetItemData();

        int rndCount = Random.Range(minItemValue, maxItemValue);

        for (int i = 0; i < rndCount; i++)
        {
            int rndItemCount = Random.Range(1, 10);

            int itemUuid = GameUtils.RandomItem(list).uuid;

            if(_itemList.ContainsKey(itemUuid) == true)
            {
                _itemList[itemUuid] += rndItemCount;
            }
            else
            {
                _itemList.Add(itemUuid, rndItemCount);
            }
        }

        /*foreach (var item in _itemList)
        {
            Debug.Log("Key : " + item.Key);
            Debug.Log("Value : " + item.Value);
        }*/
    }

    private void OnMouseDown()
    {
        GameUtils.Log("Building", "Ŭ����");

        if(isClicked == false && GamePlay.Instance.gameStateManager.CheckStateType(GameDefine.StateType.None) == true)
        {
            OnClick_ShowRoot();
        }
    }

    public void OnClick_ShowRoot()
    {
        isClicked = true;
        GamePlay.Instance.gameStateManager.SetStateType(GameDefine.StateType.Explore);
        var popup = Core.Instance.uiPopUpManager.ShowAndGet<UI_PopUp_BuildingRoot>("UI_PopUp_BuildingRoot");
        popup.Init(_itemList);
    }

    /// <summary>
    /// �ǹ� �Ĺ� UI ������ �ٽ� Ŭ�� �����ϰ� ����
    /// </summary>
    public void CloseRoot()
    {
        isClicked = false;
        GamePlay.Instance.gameStateManager.SetStateType(GameDefine.StateType.None);
    }
}
