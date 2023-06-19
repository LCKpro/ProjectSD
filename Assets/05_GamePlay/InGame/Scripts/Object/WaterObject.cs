using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GamePlay.Instance.natureManager.TriggerOnWaterObjectData();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GamePlay.Instance.natureManager.TriggerOffWaterObjectData();
        }
    }
}
