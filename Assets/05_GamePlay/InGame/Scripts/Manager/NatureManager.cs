using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureManager : MonoBehaviour
{
    public GameObject btn_GetNature;
    public GameObject btn_InteractionWater;
    public GameObject btn_InteractionStone;
    public GameObject btn_InteractionWheat;

    // 우선 오브젝트 근처에 접근했을 때 해당 오브젝트 정보 저장하고 UI 띄우기
    // 트리거 On
    public void TriggerOnNatureObjectData()
    {
        btn_GetNature.gameObject.SetActive(true);
    }

    // Trigger Off
    public void TriggerOffNatureObjectData()
    {
        btn_GetNature.gameObject.SetActive(false);
    }

    // 물 트리거 On
    public void TriggerOnWaterObjectData()
    {
        btn_InteractionWater.gameObject.SetActive(true);
    }

    // 물 Trigger Off
    public void TriggerOffWaterObjectData()
    {
        btn_InteractionWater.gameObject.SetActive(false);
    }

    // 돌 트리거 On
    public void TriggerOnStoneObjectData()
    {
        btn_InteractionStone.gameObject.SetActive(true);
    }

    // 돌 Trigger Off
    public void TriggerOffStoneObjectData()
    {
        btn_InteractionStone.gameObject.SetActive(false);
    }

    // 밀 트리거 On
    public void TriggerOnWheatObjectData()
    {
        btn_InteractionWheat.gameObject.SetActive(true);
    }

    // 밀 Trigger Off
    public void TriggerOffWheatObjectData()
    {
        btn_InteractionWheat.gameObject.SetActive(false);
    }
}
