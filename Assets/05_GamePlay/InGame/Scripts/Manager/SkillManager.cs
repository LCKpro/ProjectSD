using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    #region 0001

    private Player_0001 player0001;
    private AI_Structure targetStructure;
    public GameObject btn_Skill;

    /// <summary>
    /// ó���� ���� ���� ��ų ��ư ������ �۵��ϴ� ��Ŭ�� ��ư �Լ�
    /// </summary>
    public void OnClick_Skill0001()
    {
        Debug.Log("1001");
        GamePlay.Instance.gameStateManager.SetSkillStateType(GameDefine.SkillStateType.Player0001Skill);
        ReadySkill0001(GamePlay.Instance.unitManager.player0001);
    }

    /// <summary>
    /// ��ų ��� ��ư
    /// </summary>
    public void DeActive_Skill0001()
    {
        GamePlay.Instance.gameStateManager.SetSkillStateType(GameDefine.SkillStateType.None);
        btn_Skill.SetActive(false);
    }
    
    /// <summary>
    /// ���� ���� �����ͼ� ����
    /// </summary>
    /// <param name="player"></param>
    public void ReadySkill0001(Player_0001 player)
    {
        player0001 = player;
    }

    /// <summary>
    /// �ǹ� Ŭ���ϸ� �ǹ� ���� ��ư ����
    /// </summary>
    /// <param name="structure"></param>
    public void SetTargetStructure(AI_Structure structure)
    {
        Debug.Log("������� �Դ���?");
        targetStructure = structure;

        if (targetStructure != null)
        {
            Vector3 mousePos = Camera.main.WorldToScreenPoint(targetStructure.transform.position);
            btn_Skill.transform.position = mousePos + new Vector3(0, 100, 0);
            btn_Skill.SetActive(true);
        }
        else
        {
            Debug.Log("����");
        }
    }

    /// <summary>
    /// �ǹ��� ��� ���� ��ư�� Ŭ������ �� ������ ��ų �ߵ�
    /// </summary>
    public void OnClick_Skill0001_ConfirmRepair()
    {
        if(targetStructure != null)
        {
            DeActive_Skill0001();
            player0001.ActiveSkill0001(targetStructure);
        }
        else
        {
            Debug.Log("��ų ����");
        }
    }

    #endregion

    #region 0002

    /// <summary>
    /// ó���� ���� ���� ��ų ��ư ������ �۵��ϴ� ��Ŭ�� ��ư �Լ�
    /// </summary>
    public void OnClick_Skill0002()
    {
        Debug.Log("0002");
        GamePlay.Instance.unitManager.player0002.SetSkill();
    }

    #endregion

    #region 0003

    public NormalAttack_0003 normalAtk_1003;
    public SkillAttack_0003 skillAtk_1003;

    public void OnClick_Skill0003()
    {
        Debug.Log("0003");
        GamePlay.Instance.unitManager.player0003.SetSkill();
    }

    #endregion

    #region 0004

    public void OnClick_Skill0004()
    {
        Debug.Log("0003");
        GamePlay.Instance.unitManager.player0004.SetSkill();
    }

    #endregion
}
