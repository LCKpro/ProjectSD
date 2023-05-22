using UnityEngine;
using UniRx;
using System;
using System.Globalization;
using GameCreator.Characters;

public class CraftingManager : MonoBehaviour
{
    // ���� ũ������ ��尡 �����ִ���?
    public bool isOn;
    private IDisposable craftingModeTimer = Disposable.Empty;

    private Vector3 craftVector = Vector3.zero;
    private Vector3 newPos = Vector3.zero;

    public GameObject clickTrigger;
    public GameObject buttonGroup;
    public NavigationMarker marker;

    public float changeValue = 12f;

    // �ӽÿ� �ڵ�. ���� ���� �޾Ƽ� ���ҽ� �н� ��ũ �޾ƿ� ����
    private string buildingCode = "";
    // �ǹ� ���� �� ���� ������Ʈ
    private GameObject preparatoryObj;

    /// <summary>
    /// ũ������ ��� ����
    /// </summary>
    public void FinalizeCraft()
    {
        ManageActive(false); // ������Ʈ ���� - ���� ����
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
        InitCrafting("");
        CraftingModeEnd();
        //CraftingModeStart();
    }

    // ������Ʈ ��Ƽ�� ����
    private void ManageActive(bool isOn)
    {
        clickTrigger.SetActive(isOn);
        buttonGroup.SetActive(isOn);
    }

    public void InitCrafting(string code)
    {
        buildingCode = code;
        //GameObject obj = Resources.Load<GameObject>("GameObject/" + buildingCode);
        GameObject obj = Resources.Load<GameObject>("GameObject/Craft_Appliances_Store");

        if (obj == null)
        {
            Debug.Log("ũ������ ������Ʈ Null");
            return;
        }

        preparatoryObj = Instantiate(obj, transform);

        Vector3 playerPos = GamePlay.Instance.playerManager.GetPlayer().transform.position;
        playerPos.z += 2.0f;
        playerPos.y = 0;

        preparatoryObj.transform.position = playerPos;

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
        Debug.Log("���� ������");
        craftingModeTimer.Dispose();
        craftingModeTimer = Disposable.Empty;
        craftingModeTimer = Observable.EveryUpdate().TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Where(x => Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            .Subscribe(_ =>
            {
                Debug.Log("����V : " + Input.GetAxis("Vertical"));
                Debug.Log("����H : " + Input.GetAxis("Horizontal"));
                ManageActive(false); // ������Ʈ ���� - ���� ����
                preparatoryObj.gameObject.SetActive(false);
            });
    }


    #region �̻�� �ڵ�

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

    // UI�� ������Ʈ�� ��ǥ�� �ٸ��Ƿ� ���� ó��
    private void MoveUIByFirstMousePosition()
    {

    }


    // ��ư Ŭ��. �ǹ� ����. ĳ���� �����̱� + �ִϸ��̼��� �׼����� ��ü
    public void OnClick_ClickToCraft()
    {
        marker.transform.position = craftVector;
    }

    #endregion
}
