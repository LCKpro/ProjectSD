using System;
using UnityEngine;
using UniRx;

public class AI_Structure_Detect : MonoBehaviour
{
    public AI_Structure structure;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Monster")
        {
            return;
        }

        structure.RangeAttackStart(other);
    }

    
}
