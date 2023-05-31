using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMonster : Health
{
    public AIPlayer aiPlayer;

    public override void DealDamage(GameObject target)
    {
        base.DealDamage(target);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    protected override void Die()
    {
        aiPlayer.Die();
        // 몬스터의 경우 풀에 다시 넣어주는 로직 필요.
        // 건물 역시 마찬가지로 넣어주기
    }
}
