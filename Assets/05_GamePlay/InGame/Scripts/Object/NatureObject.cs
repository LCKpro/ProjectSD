using GameCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GamePlay.Instance.natureManager.TriggerOnNatureObjectData();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GamePlay.Instance.natureManager.TriggerOffNatureObjectData();
        }
    }
}
