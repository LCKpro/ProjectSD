using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCreator.Inventory;

public class ItemBoxManager : MonoBehaviour
{
    private GameDefine.ItemBoxType itemBoxType = GameDefine.ItemBoxType.None;

    private List<int> uuidList = new List<int>();

    public bool IsDeleteMode()
    {
        return itemBoxType == GameDefine.ItemBoxType.Delete;
    }

    public void ResetList()
    {
        itemBoxType = GameDefine.ItemBoxType.None;
        uuidList.Clear();
    }

    public void AddUUID(int uuid)
    {
        uuidList.Add(uuid);
    }

    public void RemoveUUID(int uuid)
    {
        uuidList.Remove(uuid);
    }

    public void OnClick_Delete()
    {
        if(itemBoxType == GameDefine.ItemBoxType.None)
        {
            itemBoxType = GameDefine.ItemBoxType.Delete;
            Debug.Log("Delete 상태로 변경");
            return;
        }

        if(uuidList.Count > 0)
        {
            Debug.Log("Delete 작업 실행");

            foreach (var uuid in uuidList)
            {
                int itemAmount = InventoryManager.Instance.GetInventoryAmountOfItem(uuid);

                InventoryManager.Instance.SubstractItemFromInventory(uuid, itemAmount);
                Debug.Log("아이템 삭제 : " + uuid + " 개수 : " + itemAmount);
            }

            InventoryUIManager.CloseInventory();
            InventoryUIManager.OpenInventory();
        }
        else
        {
            Debug.Log("아이템 삭제없음");
            ResetList();
        }
    }

    public void OnClick_OpenInventory()
    {
        SoundManager.instance.PlaySound("NormalClick");

        if (btn_Delete != null)
        {
            btn_Delete.UnCheckDelete();
        }

        ResetList();

        if (GamePlay.Instance.gameStateManager.CheckStateType(GameDefine.StateType.OpenInventory) == true)
        {
            GamePlay.Instance.gameStateManager.SetStateType(GameDefine.StateType.None);
            InventoryUIManager.CloseInventory();
        }
        else if (GamePlay.Instance.gameStateManager.CheckStateType(GameDefine.StateType.None) == true)
        {
            GamePlay.Instance.gameStateManager.SetStateType(GameDefine.StateType.OpenInventory);
            InventoryUIManager.OpenInventory();
        }
        else
        {
            Debug.Log("건축중이나 건물 파밍중에는 인벤토리를 열 수 없음");
        }
    }

    private DeleteButton btn_Delete = null;

    public void SendDeleteBtn(DeleteButton btn)
    {
        if (btn_Delete == null)
        {
            btn_Delete = btn;
        }
    }

}
