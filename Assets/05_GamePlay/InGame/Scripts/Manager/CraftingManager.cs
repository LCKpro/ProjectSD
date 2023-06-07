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

    private void OnDisable()
    {
        FinalizeCraft();
    }

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
        FinalizeCraft();    // UniRx �ϰ� �ٸ� ��ư�� �� ����
        playerMoveAction.Execute();
    }

    public void SpawnLoadingUI()
    {
        loadingUIObj.SetActive(true);

        Vector3 playerPos = GamePlay.Instance.playerManager.GetPlayer().transform.position;
        Vector3 uiPos = Camera.main.WorldToScreenPoint(playerPos);

        loadingUIObj.transform.position = uiPos;
    }

    #region �̻�� �ڵ�

    public void GetReadyToCraft(string code)
    {
        _buildingCode = code;
        
        //preparatoryObj = GamePlay.Instance.spawnManager.SpawnIncompleteStructure(0).gameObject;

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
    }

    /// <summary>
    /// Ÿ�̸� ����. �� ������Ʈ���� ��ǥ�� �����
    /// </summary>
    public void CraftingModeStart()
    {
        craftingModeTimer.Dispose();
        craftingModeTimer = Disposable.Empty;
        CheckCoordinates(); // ���콺 ��ġ�� ���� ��ǥ ���
        MoveObjectByMousePosition();    // ������Ʈ�� �̵���ų �� ���콺 ��ġ�� ���� ���� �����̵���
        MoveUIByMousePosition();    // UI ��ġ �����̱�
        ManageActive(true); // ������Ʈ ���� - ���� �ѱ�
        craftingModeTimer = Observable.EveryUpdate().TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                CheckCoordinates(); // ���콺 ��ġ�� ���� ��ǥ ���
                MoveObjectByMousePosition();    // ������Ʈ�� �̵���ų �� ���콺 ��ġ�� ���� ���� �����̵���
                MoveUIByMousePosition();    // UI ��ġ �����̱�
            });
    }

    // ���콺 ��ġ�� ���� ��ǥ ���
    private void CheckCoordinates()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = changeValue; // ī�޶���� �Ÿ� ����

        //buttonGroup.transform.position = Input.mousePosition;
        craftVector = Camera.main.ScreenToWorldPoint(mousePosition);
        Debug.Log("���콺 ���� ��ǥ: " + craftVector);
    }

    // ������Ʈ�� �̵���ų �� ���콺 ��ġ�� ���� ���� �����̵��� ����
    private void MoveObjectByMousePosition()
    {
        double truncateX = Math.Truncate(craftVector.x);
        double truncateZ = Math.Truncate(craftVector.z);
        var defaultValueX = truncateX > 0f ? 0.5f : -0.5f;
        var defaultValueZ = truncateZ > 0f ? 0.5f : -0.5f;

        float x = Convert.ToSingle(defaultValueX + truncateX, CultureInfo.InvariantCulture);
        float z = Convert.ToSingle(defaultValueZ + truncateZ, CultureInfo.InvariantCulture);

        newPos = new Vector3(x, 0, z);
        preparatoryObj.transform.position = marker.transform.position = newPos;

        Debug.Log("������Ʈ ���� ��ǥ: " + preparatoryObj.transform.position);
    }

    // UI�� ������Ʈ�� ��ǥ�� �ٸ��Ƿ� ���� ó��
    private void MoveUIByMousePosition()
    {
        Vector3 mousePos = Camera.main.WorldToScreenPoint(newPos);
        double truncateX = Math.Truncate(mousePos.x);
        double truncateY = Math.Truncate(mousePos.y);
        var defaultValueX = truncateX > 0f ? 0.5f : -0.5f;
        var defaultValueY = truncateY > 0f ? 0.5f : -0.5f;

        float x = Convert.ToSingle(defaultValueX + truncateX, CultureInfo.InvariantCulture);
        float y = Convert.ToSingle(defaultValueY + truncateY, CultureInfo.InvariantCulture);

        buttonGroup.transform.position = new Vector3(x, y, 0);
        Debug.Log("UI ��ǥ: " + buttonGroup.transform.position);
    }

    public void FinishCraft()
    {
        GameObject obj = Resources.Load<GameObject>("GameObject/Appliances_Store");
        obj.transform.position = marker.transform.position;
    }

    #endregion
}
