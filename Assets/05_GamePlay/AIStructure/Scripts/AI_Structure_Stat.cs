using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class AI_Structure
{
    private AI_Structure_HpBar _hpBarObj = null;
    public override void TakeDamage(float damage, GameObject attacker = null, float power = 0)
    {
        base.TakeDamage(damage, attacker, power);

        if (_hpBarObj == null)
        {
            var hpBar = GamePlay.Instance.spawnManager.GetHpBarFromPool();
            _hpBarObj = hpBar;
        }

        _hpBarObj.SetTarget(transform);
        _hpBarObj.SetHpBar(maxHealthValue, healthValue);
        SoundManager.instance.PlaySound("CraftHit");
    }

    protected override void Die()
    {
        ReturnHpBar();
        transform.gameObject.SetActive(false);
        GamePlay.Instance.spawnManager.GetFromPool("Explosion", transform.position);
        SoundManager.instance.PlaySound("Break");
        //GamePlay.Instance.spawnManager.ReturnStructurePool((int)craftType, idName, this.transform);
    }
    public void ReturnHpBar()
    {
        _hpBarObj.ResetActiveTimer();
        _hpBarObj = null;
    }
}
