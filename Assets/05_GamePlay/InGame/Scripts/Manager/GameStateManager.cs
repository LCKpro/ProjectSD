using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private GameDefine.StateType stateType = GameDefine.StateType.None;

    /// <summary>
    /// 타입 비교. 매개변수로 넣은 타입과 일치할 경우 true
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
