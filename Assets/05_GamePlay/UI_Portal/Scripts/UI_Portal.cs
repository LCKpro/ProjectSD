using System.Collections;
using UnityEngine;

public class UI_Portal : MonoBehaviour
{
    // ��Ż Ű�� UIPopUpManager.Show() �� �� �ش� �������� �̸� (ex. UI_Portal_CatTower)
    public string portalKey;

    private void OnMouseDown()
    {
        Core.Instance.uiPopUpManager.Show(portalKey);
    }

}
