using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDetect : MonoBehaviour
{
    public AIPlayer state;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Breakable")
        {
            return;
        }

        if (state.atkType == GameDefine.AIAttackType.Melee)
        {
            state.MeleeAttackStart(other);
        }
        else if (state.atkType == GameDefine.AIAttackType.Range)
        {
            state.RangeAttackStart(other);
        }
        else if (state.atkType == GameDefine.AIAttackType.Suicide)
        {
            state.SuicideAttackStart();
        }
    }
}
