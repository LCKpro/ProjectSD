using System.Collections;
using UnityEngine;

public class UI_Portal : MonoBehaviour
{
    // 포탈 키는 UIPopUpManager.Show() 할 때 해당 프리팹의 이름 (ex. UI_Portal_CatTower)
    public string portalKey;

    private void OnMouseDown()
    {
        Core.Instance.uiPopUpManager.Show(portalKey);
    }

}
