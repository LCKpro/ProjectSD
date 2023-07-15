using System;
using UnityEngine;
using UniRx;
using TMPro;

public class SkillAttack_0001 : MonoBehaviour
{
    public SphereCollider skillCollider;
    private bool isCalled = false;

    public void InitSkill_0001(Vector3 pos)
    {
        transform.position = pos;

        skillCollider.enabled = true;
        isCalled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Breakable")
        {
            other.GetComponent<AI_Structure>().FullRepairStructure();

            if(isCalled == false)
            {
                Invoke("DisableCollider", 0.1f);
                isCalled = true;
            }
        }
    }

    public void DisableCollider()
    {
        skillCollider.enabled = false;
    }

}
