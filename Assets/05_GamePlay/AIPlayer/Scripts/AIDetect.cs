using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDetect : MonoBehaviour
{
    public AIState state;

    private void OnTriggerEnter(Collider other)
    {
        // 부딪힌 오브젝트가 부술 수 있는건지
        if(other.tag == "Breakable")
        {
            // 현재 상태가 타이머가 지난 모든걸 부수는 단계면 부딪히는거 아무거나 부수기
            if (state.stateType == GameDefine.AIStateType.Breakable)
            {
                state.AttackStart(other);
                return;
            }

            // 캣타워 추적 상태 + 부딪힌게 캣타워면
            if (state.stateType == GameDefine.AIStateType.Chase_CatTower && other.gameObject.layer == 11)   // 11 = 캣타워
            {
                state.AttackStart(other);
                return;
            }
        }

        // 만약 공격자와 부딪힘 + 공격자를 추격하는 상태면
        if(other.tag == "Unit" && state.stateType == GameDefine.AIStateType.Chase_Attacker)
        {
            state.AttackStart(other);
        }
    }
}
