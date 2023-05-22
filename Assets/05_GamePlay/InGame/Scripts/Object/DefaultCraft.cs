using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultCraft : MonoBehaviour
{
    private void Start()
    {
        SpawnBuilding();
    }

    private void SpawnBuilding()
    {
        string code = GamePlay.Instance.craftingManager.GetBuildingCode();

        Debug.Log(code);

        GameObject obj = Resources.Load<GameObject>("GameObject/" + code);

        Instantiate(obj, transform);
    }
}
