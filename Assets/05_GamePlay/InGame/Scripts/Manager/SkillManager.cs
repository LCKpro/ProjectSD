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
    /// 처음에 좌측 유닛 스킬 버튼 누르면 작동하는 온클릭 버튼 함수
    /// </summary>
    public void OnClick_Skill0001()
    {
        Debug.Log("1001");
        GamePlay.Instance.gameStateManager.SetSkillStateType(GameDefine.SkillStateType.Player0001Skill);
        ReadySkill0001(GamePlay.Instance.unitManager.player0001);
    }

    /// <summary>
    /// 스킬 취소 버튼
    /// </summary>
    public void DeActive_Skill0001()
    {
        GamePlay.Instance.gameStateManager.SetSkillStateType(GameDefine.SkillStateType.None);
        btn_Skill.SetActive(false);
    }
    
    /// <summary>
    /// 유닛 정보 가져와서 저장
    /// </summary>
    /// <param name="player"></param>
    public void ReadySkill0001(Player_0001 player)
    {
        player0001 = player;
    }

    /// <summary>
    /// 건물 클릭하면 건물 위에 버튼 띄우기
    /// </summary>
    /// <param name="structure"></param>
    public void SetTargetStructure(AI_Structure structure)
    {
        Debug.Log("여기까지 왔는지?");
        targetStructure = structure;

        if (targetStructure != null)
        {
            Vector3 mousePos = Camera.main.WorldToScreenPoint(targetStructure.transform.position);
            btn_Skill.transform.position = mousePos + new Vector3(0, 100, 0);
            btn_Skill.SetActive(true);
        }
        else
        {
            Debug.Log("버그");
        }
    }

    /// <summary>
    /// 건물에 띄운 수리 버튼을 클릭했을 때 유닛이 스킬 발동
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
            Debug.Log("스킬 버그");
        }
    }

    #endregion

    #region 0002

    /// <summary>
    /// 처음에 좌측 유닛 스킬 버튼 누르면 작동하는 온클릭 버튼 함수
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
