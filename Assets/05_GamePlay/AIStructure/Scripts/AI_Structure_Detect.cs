using System;
using UnityEngine;
using UniRx;

public class AI_Structure_Detect : MonoBehaviour
{
    public AI_Structure structure;
    public float detectTime = 1f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Monster")
        {
            return;
        }

        structure.RangeAttackStart(other, detectTime);
    }
}
