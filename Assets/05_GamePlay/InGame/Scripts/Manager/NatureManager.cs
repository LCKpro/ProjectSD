using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureManager : MonoBehaviour
{
    public GameObject btn_GetNature;
    public GameObject btn_InteractionWater;

    // �켱 ������Ʈ ��ó�� �������� �� �ش� ������Ʈ ���� �����ϰ� UI ����
    // Ʈ���� On
    public void TriggerOnNatureObjectData()
    {
        btn_GetNature.gameObject.SetActive(true);
    }

    // Trigger Off
    public void TriggerOffNatureObjectData()
    {
        btn_GetNature.gameObject.SetActive(false);
    }

    // �� Ʈ���� On
    public void TriggerOnWaterObjectData()
    {
        btn_InteractionWater.gameObject.SetActive(true);
    }

    // �� Trigger Off
    public void TriggerOffWaterObjectData()
    {
        btn_InteractionWater.gameObject.SetActive(false);
    }
}
