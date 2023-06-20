using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStateItemBox : MonoBehaviour
{
    public void SetCloseState()
    {
        GamePlay.Instance.gameStateManager.SetStateType(GameDefine.StateType.None);
    }
}
