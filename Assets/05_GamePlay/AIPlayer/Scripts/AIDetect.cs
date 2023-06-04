using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDetect : MonoBehaviour
{
    public AIState state;

    private void OnTriggerEnter(Collider other)
    {
        // �ε��� ������Ʈ�� �μ� �� �ִ°���
        if(other.tag == "Breakable")
        {
            // ���� ���°� Ÿ�̸Ӱ� ���� ���� �μ��� �ܰ�� �ε����°� �ƹ��ų� �μ���
            if (state.stateType == GameDefine.AIStateType.Breakable)
            {
                state.MeleeAttackStart(other);
                return;
            }

            // ĹŸ�� ���� ���� + �ε����� ĹŸ����
            if (state.stateType == GameDefine.AIStateType.Chase_CatTower && other.gameObject.layer == 11)   // 11 = ĹŸ��
            {
                state.MeleeAttackStart(other);
                return;
            }
        }

        // ���� �����ڿ� �ε��� + �����ڸ� �߰��ϴ� ���¸�
        if(other.tag == "Unit" && state.stateType == GameDefine.AIStateType.Chase_Attacker)
        {
            state.MeleeAttackStart(other);
        }
    }
}
