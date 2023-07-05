using UnityEngine;
using UniRx;
using System;
using System.Globalization;
using GameCreator.Characters;

public class CraftObject : MonoBehaviour
{
    private IDisposable craftingModeTimer = Disposable.Empty;

    private Vector3 craftVector = Vector3.zero;
    private Vector3 newPos = Vector3.zero;

    //private GameObject _clickTrigger;
    private GameObject _buttonGroup;
    private NavigationMarker _marker;
    private NavigationMarker _marker_LookAt;
    private Transform _spawnPos;

    public float changeValue = 15f;

    // ���� ���� �޾Ƽ� ���ҽ� �н� ��ũ �޾ƿ� ����

    public void FinalizeCraft()
    {
        craftingModeTimer.Dispose();
        craftingModeTimer = Disposable.Empty;
    }

    private void OnDisable()
    {
        FinalizeCraft();
    }

    private void Start()
    {
        SetData();
        ManageActive(true);
    }

    // �Ŵ������� ������ ��������
    private void SetData()
    {
        var manager = GamePlay.Instance.craftingManager;
        _marker = manager.marker;
        _marker_LookAt = manager.marker_LookAt;
        _spawnPos = manager.spawnPos;
        _buttonGroup = manager.buttonGroup;
    }

    // ������Ʈ ��Ƽ�� ����
    private void ManageActive(bool isOn)
    {
        //_clickTrigger.SetActive(isOn);
        _buttonGroup.SetActive(isOn);
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
        //transform.position = _marker.transform.position = newPos;
        transform.position = newPos;
        _spawnPos.position = newPos;
        SetMarkerPos(newPos);
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

        _buttonGroup.transform.position = new Vector3(x, y, 0);
    }

    private void SetMarkerPos(Vector3 targetPos)
    {
        var vec = GamePlay.Instance.playerManager.GetPlayer().transform.position - targetPos;

        _marker.transform.position = targetPos + (vec.normalized * 3.5f);
        _marker_LookAt.transform.position = targetPos;
    }

    private void OnMouseDrag()
    {
        CheckCoordinates(); // ���콺 ��ġ�� ���� ��ǥ ���
        MoveObjectByMousePosition();    // ������Ʈ�� �̵���ų �� ���콺 ��ġ�� ���� ���� �����̵���
        MoveUIByMousePosition();    // UI ��ġ �����̱�
    }
}
