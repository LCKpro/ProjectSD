using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class AI_Structure
{
    public override void TakeDamage(float damage, GameObject attacker = null, float power = 0)
    {
        base.TakeDamage(damage, attacker, power);

        aI_Structure_HpBar.SetHpBar(maxHealthValue, healthValue);
        SoundManager.instance.PlaySound("CraftHit");
    }


    protected override void Die()
    {
        transform.gameObject.SetActive(false);
        GamePlay.Instance.spawnManager.GetFromPool("Explosion", transform.position);
        SoundManager.instance.PlaySound("Break");
        //GamePlay.Instance.spawnManager.ReturnStructurePool((int)craftType, idName, this.transform);
    }
}
