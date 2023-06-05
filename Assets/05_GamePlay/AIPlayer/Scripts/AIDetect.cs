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

        if (state.atkType == GameDefine.AttackType.Melee)
        {
            state.MeleeAttackStart(other);
        }
        else if (state.atkType == GameDefine.AttackType.Range)
        {
            state.RangeAttackStart(other);
        }
        else if (state.atkType == GameDefine.AttackType.Suicide)
        {
            state.SuicideAttackStart();
        }
    }
}
