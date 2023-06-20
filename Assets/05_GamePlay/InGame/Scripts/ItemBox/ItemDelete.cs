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
        Debug.Log("������ Ŭ������");
        if (GamePlay.Instance.itemBoxManager.IsDeleteMode() == true)	// �������� Ŭ�� �Ǿ����� ��
        {
            if (isCheck == false)	// ���� üũ ������ ��
            {
                checkObj.SetActive(true);
                isCheck = true;
                GamePlay.Instance.itemBoxManager.AddUUID(itemUI.item.uuid);
                Debug.Log("������ ��� UUID : " + itemUI.item.uuid);
            }
            else
            {   // üũ ���� ���� �ٽ� ���󺹱�
                ReadySlot();
                GamePlay.Instance.itemBoxManager.RemoveUUID(itemUI.item.uuid);
                Debug.Log("������ ��� ��� UUID : " + itemUI.item.uuid);
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
