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
    /// 데미지 줄 때
    /// </summary>
    /// <param name="target"></param>
    public virtual void DealDamage(GameObject target, float power = 0f)
    {
        Debug.Log(target.name + "에게 " + transform.name + "이(가) 데미지 줌");
        target.GetComponent<Stat>().TakeDamage(damageValue, transform.gameObject, power);
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
    public virtual void TakeDamage(float damage, GameObject attacker = null, float power = 0f)
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
        Debug.Log(gameObject.name + "사망");
        transform.gameObject.SetActive(false);
        // 몬스터의 경우 풀에 다시 넣어주는 로직 필요.
        // 건물 역시 마찬가지로 넣어주기
    }

    /// <summary>
    /// CC기 상대방한테 줄 때
    /// 타겟, CC 타입, 지속시간, CC기 강도(%)
    /// </summary>
    /// <param name="target"></param>
    public virtual void DealCrowdControl(GameObject target, GameDefine.CCType ccType, float duration, float percent = 0.01f)
    {
        Debug.Log(target.name + "에게 " + ccType + " CC기를 줌");
        target.GetComponent<Stat>().TakeCrowdControl(ccType, duration, percent);
    }

    /// <summary>
    /// CC기 받았을 때
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeCrowdControl(GameDefine.CCType ccType, float duration, float percent = 0.01f)
    {
        Debug.Log(ccType + "의 CC기를 받음");

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
        Debug.LogError("CC는 하위 단계에서 호출되어야 함. 하위 미구현 상태");
    }

    protected virtual void StunStart(float duration)
    {
        Debug.LogError("CC는 하위 단계에서 호출되어야 함. 하위 미구현 상태");
    }
}
