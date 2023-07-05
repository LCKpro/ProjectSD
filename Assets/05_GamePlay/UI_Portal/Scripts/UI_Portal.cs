using System.Collections;
using UnityEngine;

public class UI_Portal : MonoBehaviour
{
    // 포탈 키는 UIPopUpManager.Show() 할 때 해당 프리팹의 이름 (ex. UI_Portal_CatTower)
    public string portalKey;

    /*private void OnMouseUp()
    {
        if(GamePlay.Instance.gameStateManager.CheckStateType(GameDefine.StateType.None) == false)
        {
            return;
        }

        if(GamePlay.Instance.ui_System.gameObject.activeSelf == true)
        {
            return;
        }

        SoundManager.instance.PlaySound("NormalClick");
        Core.Instance.uiPopUpManager.Show(portalKey);
    }*/

}
