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
        // ������ ��� Ǯ�� �ٽ� �־��ִ� ���� �ʿ�.
        // �ǹ� ���� ���������� �־��ֱ�
    }
}
