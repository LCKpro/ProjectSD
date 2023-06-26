using System.Collections;
using UnityEngine;

public class UI_Portal : MonoBehaviour
{
    // ��Ż Ű�� UIPopUpManager.Show() �� �� �ش� �������� �̸� (ex. UI_Portal_CatTower)
    public string portalKey;

    private void OnMouseDown()
    {
        if(GamePlay.Instance.gameStateManager.CheckStateType(GameDefine.StateType.None) == false)
        {
            return;
        }

        Core.Instance.uiPopUpManager.Show(portalKey);
    }

}
