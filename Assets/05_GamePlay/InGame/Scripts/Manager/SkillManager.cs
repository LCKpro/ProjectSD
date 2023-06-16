using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

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


    #region ��ų ��ư 1~4

    public void OnClick_First()
    {
        if(_isCoolTime[0] == true)
        {
            return;
        }

        _isCoolTime[0] = true;

        OnClick_Skill0001();
        SetCoolTime(0);
    }

    public void OnClick_Second()
    {
        if (_isCoolTime[1] == true)
        {
            return;
        }

        _isCoolTime[1] = true;

        OnClick_Skill0002();
        SetCoolTime(1);
    }

    public void OnClick_Third()
    {
        if (_isCoolTime[2] == true)
        {
            return;
        }

        _isCoolTime[2] = true;

        OnClick_Skill0003();
        SetCoolTime(2);
    }

    public void OnClick_Fourth()
    {
        if (_isCoolTime[3] == true)
        {
            return;
        }

        _isCoolTime[3] = true;

        OnClick_Skill0004();
        SetCoolTime(3);
    }

    #endregion


    #region ��ų ��ư Ÿ�̸�

    public Image[] coolTime = new Image[4];

    private IDisposable[] _coolTimeChecker = new IDisposable[4] { Disposable.Empty, Disposable.Empty, Disposable.Empty, Disposable.Empty };
    private bool[] _isCoolTime = new bool[4];

    private void StopCoolDown(int index)
    {
        _coolTimeChecker[index].Dispose();
        _coolTimeChecker[index] = Disposable.Empty;
    }

    private void SetCoolTime(int index)
    {
        coolTime[index].fillAmount = 1;
        
        StopCoolDown(index);

        _coolTimeChecker[index] = Observable.EveryUpdate().TakeUntilDisable(gameObject).
            TakeUntilDestroy(gameObject).
            Subscribe(_ =>
            {
                coolTime[index].fillAmount -= Time.deltaTime / 10;

                if(coolTime[index].fillAmount <= 0f)
                {
                    _isCoolTime[index] = false;
                    StopCoolDown(index);
                }
            });
    }

    #endregion


}
