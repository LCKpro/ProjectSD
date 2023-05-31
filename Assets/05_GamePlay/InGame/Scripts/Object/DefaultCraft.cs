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

        var building = gamePlay.spawnManager.SpawnStructure(bCode);

        building.position = gamePlay.craftingManager.marker.transform.position;

        transform.gameObject.SetActive(false);
    }
}
