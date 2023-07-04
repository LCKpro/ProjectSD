using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack_0003 : MonoBehaviour
{
    private Animator anim = null;

    public void SpawnCloud()
    {
        if(anim == null)
        {
            anim = GetComponent<Animator>();
        }

        anim.SetTrigger("Idle");
    }
}
