using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultCraft : MonoBehaviour
{
    private void OnEnable()
    {
        SpawnBuilding();
    }

    /// <summary>
    /// ��ư Ŭ���� ����Ʈ�� �ϳ� ������Ʈ ����� ���� �޾ƿͼ� ���� �ǹ� ������
    /// </summary>
    private void SpawnBuilding()
    {
        var tr = GamePlay.Instance.craftingManager;
        string code = tr.GetBuildingCode();

        Debug.Log(code);

        GameObject obj = Resources.Load<GameObject>("GameObject/" + code);

        var building = Instantiate(obj, tr.transform);
        building.transform.position = tr.marker.transform.position;

        transform.gameObject.SetActive(false);
    }
}
