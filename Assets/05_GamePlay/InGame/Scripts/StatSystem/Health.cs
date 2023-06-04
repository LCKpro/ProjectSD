using UnityEngine;

public abstract class Health : MonoBehaviour
{
    public float healthValue = 0f;

    public float damageValue = 0f;

    public float defenseValue = 0f;

    /// <summary>
    /// ������ �� ��
    /// </summary>
    /// <param name="target"></param>
    public virtual void DealDamage(GameObject target)
    {
        Debug.Log(target.name + "���� "+ transform.name + "��(��) ������ ��");
        target.GetComponent<Health>().TakeDamage(damageValue);
    }

    /// <summary>
    /// ������ ����
    /// </summary>
    /// <param name="atk"></param>
    /// <param name="def"></param>
    /// <returns></returns>
    protected float GetDamage(float atk, float def)
    {
        if(def > 0)
            return atk * 100f / (100f + def);
        else
            return atk * (2 - 100f / (100f - def));
    }

    /// <summary>
    /// ������ �޾��� ��
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeDamage(float damage, GameObject attacker = null)
    {
        Debug.Log(transform.name + "��(��) " + damage +"�� �������� ����");
        healthValue -= GetDamage(damage, defenseValue);

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
