using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;
using System.Globalization;

public class SkillManager : MonoBehaviour
{
    #region 0001

    private Player_0001 player0001;
    private AI_Structure targetStructure;
    public GameObject btn_Skill;
    public SkillAttack_0001 unit_Skill_0001;
    public Skill_0001_Detect unit_Skill_Detect;
    public GameObject buttonGroup;

    /// <summary>
    /// 처음에 좌측 유닛 스킬 버튼 누르면 작동하는 온클릭 버튼 함수
    /// </summary>
    public void OnClick_Skill0001()
    {
        Debug.Log("1001");
        GamePlay.Instance.gameStateManager.SetSkillStateType(GameDefine.SkillStateType.Player0001Skill);
        ReadySkill_0001();
    }

    private void ReadySkill_0001()
    {
        player0001 = GamePlay.Instance.unitManager.player0001;

        // 여기서 버그나면 매니저랑 DB의 인덱스가 일치하지 않아서 그럼
        if (unit_Skill_Detect == null)
        {
            Debug.Log("아치 스킬 오브젝트 Null. 스킬매니저 인스펙터 확인");
            return;
        }

        // 플레이어 앞에 설치하기 위한 준비
        Vector3 playerPos = GamePlay.Instance.playerManager.GetPlayer().transform.position;
        playerPos.z += 2.0f;
        playerPos.y = 0;

        unit_Skill_Detect.transform.position = playerPos;

        // 스폰된 건물의 위치에 맞게 UI도 위치 변경
        Vector3 uiPos = Camera.main.WorldToScreenPoint(playerPos);

        double truncateX = Math.Truncate(uiPos.x);
        double truncateY = Math.Truncate(uiPos.y);
        var defaultValueX = truncateX > 0f ? 0.5f : -0.5f;
        var defaultValueY = truncateY > 0f ? 0.5f : -0.5f;

        float x = Convert.ToSingle(defaultValueX + truncateX, CultureInfo.InvariantCulture);
        float y = Convert.ToSingle(defaultValueY + truncateY, CultureInfo.InvariantCulture);

        buttonGroup.transform.position = new Vector3(x, y, 0);
        ManageActive(true);

        unit_Skill_Detect.CraftingModeStart();
    }

    // 아치 스킬 버튼 UI 온오프
    private void ManageActive(bool isOn)
    {
        buttonGroup.SetActive(isOn);
    }

    // 버튼 그룹에서 위치 정하고 누르는 버튼
    public void OnClick_LocateCompleteSkill_0001()
    {
        GamePlay.Instance.gameStateManager.SetSkillStateType(GameDefine.SkillStateType.None);
        unit_Skill_0001.InitSkill_0001(unit_Skill_Detect.transform.position);
        unit_Skill_Detect.transform.position = new Vector3(0, 50, 0);
        buttonGroup.SetActive(false);
    }

    public void OnClick_CancelSkill_0001()
    {
        GamePlay.Instance.gameStateManager.SetSkillStateType(GameDefine.SkillStateType.None);
        buttonGroup.SetActive(false);
    }

    #region 구0001

    /// <summary>
    /// 스킬 취소 버튼
    /// </summary>
    public void DeActive_Skill0001()
    {
        GamePlay.Instance.gameStateManager.SetSkillStateType(GameDefine.SkillStateType.None);
        btn_Skill.SetActive(false);
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


    #region 스킬 버튼 1~4

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


    #region 스킬 버튼 타이머

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
