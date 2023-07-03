using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack_0004 : Stat
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster") == true)
        {
            DealDamage(other.gameObject);
            GamePlay.Instance.spawnManager.ReturnProjectilePool("P0004", this.transform);
        }
    }
}
