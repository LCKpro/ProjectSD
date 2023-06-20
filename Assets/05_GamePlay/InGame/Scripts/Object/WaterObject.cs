using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterObject : MonoBehaviour
{
    private GameDefine.FarmingType _farmingType = GameDefine.FarmingType.None;

    private void OnTriggerEnter(Collider other)
    {
        if (_farmingType == GameDefine.FarmingType.OnFarming)
        {
            return;
        }

        if (other.tag == "Player")
        {
            _farmingType = GameDefine.FarmingType.OnFarming;
            GamePlay.Instance.natureManager.TriggerOnWaterObjectData();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _farmingType = GameDefine.FarmingType.OffFarming;
            GamePlay.Instance.natureManager.TriggerOffWaterObjectData();
        }
    }
}
