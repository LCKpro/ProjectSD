using UnityEngine;
using UniRx;
using System;
using System.Globalization;
using GameCreator.Characters;
using GameCreator.Core;

public class CraftingManager : MonoBehaviour
{
    // ���� ũ������ ��尡 �����ִ���?
    public bool isOn;
    private IDisposable craftingModeTimer = Disposable.Empty;

    private Vector3 craftVector = Vector3.zero;
    private Vector3 newPos = Vector3.zero;

    public GameObject buttonGroup;
    public NavigationMarker marker;
    public NavigationMarker marker_LookAt;
    public Transform spawnPos;

    public GameObject loadingUIObj;

    public float changeValue = 12f;

    // �ӽÿ� �ڵ�. ���� ���� �޾Ƽ� ���ҽ� �н� ��ũ �޾ƿ� ����
    private string _buildingCode = "";
    private int _typeCode = 0;
    private int _code = 0;
    // �ǹ� ���� �� ���� ������Ʈ
    private GameObject preparatoryObj;

    public Actions playerMoveAction;


    /// <summary>
    /// ũ������ ��� ����
    /// </summary>
    public void FinalizeCraft()
    {
        ManageActive(false); // ������Ʈ ���� - ���� ����
        if(preparatoryObj != null)
            preparatoryObj.gameObject.SetActive(false);
        craftingModeTimer.Dispose();
        craftingModeTimer = Disposable.Empty;
    }

    /*private void OnDisable()
    {
        FinalizeCraft();
    }*/

    private void Start()
    {
        ManageActive(false);
        /*GetReadyToCraft("Appliances_Store");
        CraftingModeEnd();*/
    }

    // ������Ʈ ��Ƽ�� ����. ��ư ó���� �Ⱥ��̵���
    private void ManageActive(bool isOn)
    {
        buttonGroup.SetActive(isOn);
    }

    /// <summary>
    /// ũ������ ����. DB�� Ű���� �޾ƿͼ� �ش� �ǹ��� ����õ��� �� �ֵ��� ���
    /// </summary>
    /// <param name="key"></param>
    public void InitCraft(int key)
    {
        //GetReadyToCraft(key);       // Ű �ް� ������Ʈ �غ�
        //GetReadyToCraft("Appliances_Store");     // Ű �ް� ������Ʈ �غ�(���� �ڵ�)
        CraftingModeEnd();  // ������ �����ؼ� ũ������ �������� ����
    }

    /// <summary>
    /// ũ������ ����. DB�� Ű���� �޾ƿͼ� �ش� �ǹ��� ����õ��� �� �ֵ��� ���
    /// </summary>
    /// <param name="key"></param>
    public void InitCraft(string key)
    {
        //GetReadyToCraft(key);       // Ű �ް� ������Ʈ �غ�
        //GetReadyToCraft("Appliances_Store");     // Ű �ް� ������Ʈ �غ�(���� �ڵ�)
        CraftingModeEnd();  // ������ �����ؼ� ũ������ �������� ����
    }

    public void InitCraft(int type, int key)
    {
        GetReadyToCraft(type, key);       // Ű �ް� ������Ʈ �غ�
        //GetReadyToCraft("Appliances_Store");     // Ű �ް� ������Ʈ �غ�(���� �ڵ�)
        CraftingModeEnd();  // ������ �����ؼ� ũ������ �������� ����
    }

    public string GetBuildingCode()
    {
        return _buildingCode;
    }


    // Ÿ�� �ڵ�� �ǹ��� Ÿ��(��,��,���), �Ϲ� �ڵ�� �ǹ��� �ε��� ��ȣ
    public int GetTypeCode()
    {
        return _typeCode;
    }

    public int GetCode()
    {
        return _code;
    }

    public void GetReadyToCraft(int type, int code)
    {
        _typeCode = type;
        _code = code;

        // ���⼭ ���׳��� �Ŵ����� DB�� �ε����� ��ġ���� �ʾƼ� �׷�
        Debug.Log(_typeCode + ", " + _code);
        preparatoryObj = GamePlay.Instance.spawnManager.SpawnIncompleteStructure(_typeCode, _code).gameObject;

        // �÷��̾� �տ� ��ġ�ϱ� ���� �غ�
        Vector3 playerPos = GamePlay.Instance.playerManager.GetPlayer().transform.position;
        playerPos.z += 2.0f;
        playerPos.y = 0;

        preparatoryObj.transform.position = playerPos;

        // ������ �ǹ��� ��ġ�� �°� UI�� ��ġ ����
        Vector3 uiPos = Camera.main.WorldToScreenPoint(playerPos);

        double truncateX = Math.Truncate(uiPos.x);
        double truncateY = Math.Truncate(uiPos.y);
        var defaultValueX = truncateX > 0f ? 0.5f : -0.5f;
        var defaultValueY = truncateY > 0f ? 0.5f : -0.5f;

        float x = Convert.ToSingle(defaultValueX + truncateX, CultureInfo.InvariantCulture);
        float y = Convert.ToSingle(defaultValueY + truncateY, CultureInfo.InvariantCulture);

        buttonGroup.transform.position = new Vector3(x, y, 0);
        ManageActive(true);
    }

    

    /// <summary>
    /// ������ �� ũ������ ��� ����
    /// </summary>
    public void CraftingModeEnd()
    {
        craftingModeTimer.Dispose();
        craftingModeTimer = Disposable.Empty;
        craftingModeTimer = Observable.EveryUpdate().TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Where(x => Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            .Subscribe(_ =>
            {
                Debug.Log("����V : " + Input.GetAxis("Vertical"));
                Debug.Log("����H : " + Input.GetAxis("Horizontal"));
                FinalizeCraft();    // UniRx �ϰ� �ٸ� ��ư�� �� ����
            });
    }

    // ��ư Ŭ���� �̸� �Űܳ��� ��Ŀ�� ���� �÷��̾ ������
    // ��Ŀ�� ���� �����̴� ���̿� �÷��̾� Input�� �����Ǹ� ���ߴ� UniRx�� �߰��ؾ� �ҵ�
    // ��ư Ŭ��. �ǹ� ����. ĳ���� �����̱� + �ִϸ��̼��� �׼����� ��ü
    public void OnClick_ClickToCraft()
    {
        GamePlay.Instance.gameStateManager.SetStateType(GameDefine.StateType.Build);
        var idName = preparatoryObj.GetComponent<CraftObject>().idName;
        GamePlay.Instance.spawnManager.ReturnIncompleteStructurePool(_typeCode, idName, preparatoryObj.transform);
        preparatoryObj = null;
        FinalizeCraft();    // UniRx �ϰ� �ٸ� ��ư�� �� ����
        playerMoveAction.Execute();
    }

    /// <summary>
    /// ũ������ ��� ����
    /// </summary>
    public void OnClick_FinalizeCraft()
    {
        ManageActive(false); // ������Ʈ ���� - ���� ����
        if (preparatoryObj != null)
            preparatoryObj.gameObject.SetActive(false);
        craftingModeTimer.Dispose();
        craftingModeTimer = Disposable.Empty;
        GamePlay.Instance.gameStateManager.SetStateType(GameDefine.StateType.None);
    }

    public void SpawnLoadingUI()
    {
        loadingUIObj.SetActive(true);

        Vector3 playerPos = GamePlay.Instance.playerManager.GetPlayer().transform.position;
        Vector3 uiPos = Camera.main.WorldToScreenPoint(playerPos);

        loadingUIObj.transform.position = uiPos;
    }
}
