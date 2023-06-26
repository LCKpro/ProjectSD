using System.Collections;
using UnityEngine;

public class UI_Portal : MonoBehaviour
{
    // 포탈 키는 UIPopUpManager.Show() 할 때 해당 프리팹의 이름 (ex. UI_Portal_CatTower)
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
