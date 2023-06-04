using UnityEngine;

public class Stat : MonoBehaviour
{
    public string idName;
    public float healthValue = 0f;
    public float damageValue = 0f;
    public float defenseValue = 0f;
    public float rangeValue = 0f;
    public float moveSpeed = 0f;

    /// <summary>
    /// 데미지 줄 때
    /// </summary>
    /// <param name="target"></param>
    public virtual void DealDamage(GameObject target)
    {
        Debug.Log(target.name + "에게 " + transform.name + "이(가) 데미지 줌");
        target.GetComponent<Stat>().TakeDamage(damageValue);
    }

    /// <summary>
    /// 데미지 계산식
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
    /// 데미지 받았을 때
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeDamage(float damage, GameObject attacker = null)
    {
        Debug.Log(transform.name + "이(가) " + damage + "의 데미지를 받음");
        healthValue -= GetDamage(damage, defenseValue);

        if (healthValue < 0f)
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
