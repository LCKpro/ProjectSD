using UnityEngine;

public abstract class Health
{
    public float healthValue = 0f;

    public float damageValue = 0f;

    public virtual void DealDamage(GameObject target)
    {
        target.GetComponent<Health>().TakeDamage(damageValue);
    }

    public virtual void TakeDamage(float damage)
    {
        healthValue -= damage;

        if(healthValue < 0f)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        // 몬스터의 경우 풀에 다시 넣어주는 로직 필요.
        // 건물 역시 마찬가지로 넣어주기
    }
}
