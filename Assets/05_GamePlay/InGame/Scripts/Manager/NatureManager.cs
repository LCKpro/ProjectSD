using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureManager : MonoBehaviour
{
    public GameObject btn_GetNature;

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
}
