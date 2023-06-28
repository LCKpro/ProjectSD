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

        var pos = gamePlay.craftingManager.spawnPos.position;
        building.position = pos;

        var index = (typeCode * 100) + bCode;
        int x = (int)pos.x;
        int z = (int)pos.z;
        GamePlay.Instance.architectureManager.SaveArchitecture(index, x, z);

        transform.gameObject.SetActive(false);

        GamePlay.Instance.gameStateManager.SetStateType(GameDefine.StateType.None);
    }
}
