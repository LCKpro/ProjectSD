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
    /// 버튼 클릭시 디폴트로 하나 오브젝트 만들고 정보 받아와서 새로 건물 지어줌
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
