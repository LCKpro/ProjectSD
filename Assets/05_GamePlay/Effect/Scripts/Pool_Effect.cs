using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_Effect : MonoBehaviour
{
    public string idName;
    public float duration;

    private void OnEnable()
    {
        Invoke("GoToPool", duration);
    }

    private void OnDisable()
    {
        CancelInvoke("GoToPool");
    }

    public void GoToPool()
    {
        GamePlay.Instance.spawnManager.ReturnEffectPool(idName, this.transform);
    }

}
