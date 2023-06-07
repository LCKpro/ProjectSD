using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private GameDefine.StateType stateType = GameDefine.StateType.None;

    /// <summary>
    /// Ÿ�� ��. �Ű������� ���� Ÿ�԰� ��ġ�� ��� true
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool CheckStateType(GameDefine.StateType type)
    {
        return (stateType == type);
    }

    public void SetStateType(GameDefine.StateType type)
    {
        stateType = type;
    }

    public GameDefine.StateType GetStateType()
    {
        return stateType;
    }
}
