using UnityEngine;

public class Stat : MonoBehaviour
{
    public string idName;
    public float maxHealthValue = 0f;
    public float healthValue = 0f;
    public float damageValue = 0f;
    public float defenseValue = 0f;
    public float rangeValue = 0f;
    public float moveSpeed = 0f;
    public float atkSpeed = 0f;

    /// <summary>
    /// ������ �� ��
    /// </summary>
    /// <param name="target"></param>
    public virtual void DealDamage(GameObject target, float power = 0f)
    {
        Debug.Log(target.name + "���� " + transform.name + "��(��) ������ ��");
        target.GetComponent<Stat>().TakeDamage(damageValue, transform.gameObject, power);
    }

    /// <summary>
    /// ������ ����
    /// </summary>
    /// <param name="atk"></param>
    /// <param name="def"></param>
    /// <returns></returns>
    protected float GetDamage(float atk, float def)
    {
        if (def > 0)
            return atk * 100f / (100f + def);
        else
            return atk * (2 - 100f / (100f - def));
    }

    /// <summary>
    /// ������ �޾��� ��
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeDamage(float damage, GameObject attacker = null, float power = 0f)
    {
        Debug.Log(transform.name + "��(��) " + damage + "�� �������� ����");
        healthValue -= GetDamage(damage, defenseValue);

        if (healthValue < 0f)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + "���");
        transform.gameObject.SetActive(false);
        // ������ ��� Ǯ�� �ٽ� �־��ִ� ���� �ʿ�.
        // �ǹ� ���� ���������� �־��ֱ�
    }

    /// <summary>
    /// CC�� �������� �� ��
    /// Ÿ��, CC Ÿ��, ���ӽð�, CC�� ����(%)
    /// </summary>
    /// <param name="target"></param>
    public virtual void DealCrowdControl(GameObject target, GameDefine.CCType ccType, float duration, float percent = 0.01f)
    {
        Debug.Log(target.name + "���� " + ccType + " CC�⸦ ��");
        target.GetComponent<Stat>().TakeCrowdControl(ccType, duration, percent);
    }

    /// <summary>
    /// CC�� �޾��� ��
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeCrowdControl(GameDefine.CCType ccType, float duration, float percent = 0.01f)
    {
        Debug.Log(ccType + "�� CC�⸦ ����");

        if (ccType == GameDefine.CCType.Slow)
        {
            SlowStart(duration, percent);
        }
        else if (ccType == GameDefine.CCType.Stun)
        {
            StunStart(duration);
        }
        else
        {

        }
    }

    protected virtual void SlowStart(float duration, float percent)
    {
        Debug.LogError("CC�� ���� �ܰ迡�� ȣ��Ǿ�� ��. ���� �̱��� ����");
    }

    protected virtual void StunStart(float duration)
    {
        Debug.LogError("CC�� ���� �ܰ迡�� ȣ��Ǿ�� ��. ���� �̱��� ����");
    }
}
