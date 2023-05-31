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
        // ������ ��� Ǯ�� �ٽ� �־��ִ� ���� �ʿ�.
        // �ǹ� ���� ���������� �־��ֱ�
    }
}
