using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDefault : MonoBehaviour
{
    public Animator anim;
    public int index;

    public void UnitDefaultAnimation()
    {
        anim.SetInteger("animation", index);
    }
}
