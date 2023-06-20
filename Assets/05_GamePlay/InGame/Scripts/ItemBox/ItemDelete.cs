using UnityEngine;
using UnityEngine.Events;

public class ItemDelete : MonoBehaviour
{
    [Space]
    public UnityEvent eventOnHoverEnter;
    public UnityEvent eventOnHoverExit;

    public GameCreator.Inventory.ItemUI itemUI;
    public GameObject checkObj;
    public Animator inventoryAnimator;

    private bool isCheck = false;
    private bool isEvent = false;
    public void OnClick_Delete()
    {
        Debug.Log("아이템 클릭했음");
        if (GamePlay.Instance.itemBoxManager.IsDeleteMode() == true)	// 쓰레기통 클릭 되어있을 때
        {
            if (isCheck == false)	// 아직 체크 안했을 때
            {
                checkObj.SetActive(true);
                isCheck = true;
                GamePlay.Instance.itemBoxManager.AddUUID(itemUI.item.uuid);
                Debug.Log("아이템 등록 UUID : " + itemUI.item.uuid);
            }
            else
            {   // 체크 했을 때는 다시 원상복구
                ReadySlot();
                GamePlay.Instance.itemBoxManager.RemoveUUID(itemUI.item.uuid);
                Debug.Log("아이템 등록 취소 UUID : " + itemUI.item.uuid);
            }

            return;
        }

        if (isEvent == false)
        {
            if (this.eventOnHoverEnter != null)
            {
                isEvent = true;
                this.eventOnHoverEnter.Invoke();
                inventoryAnimator.SetBool("State", true);
            }
        }
        else
        {
            if (this.eventOnHoverExit != null)
            {
                isEvent = false;
                this.eventOnHoverExit.Invoke();
                inventoryAnimator.SetBool("State", false);
            }
        }
    }

    public void ReadySlot()
    {
        checkObj.SetActive(false);
        isCheck = false;
    }
}
