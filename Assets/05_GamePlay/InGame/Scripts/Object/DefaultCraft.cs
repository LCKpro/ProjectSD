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
        var gamePlay = GamePlay.Instance;
        int bCode = gamePlay.craftingManager.GetCode();
        int typeCode = gamePlay.craftingManager.GetTypeCode();

        var building = gamePlay.spawnManager.SpawnStructure(typeCode, bCode);

        building.position = gamePlay.craftingManager.spawnPos.position;

        transform.gameObject.SetActive(false);
    }
}
