using UnityEngine;
using UniRx;
using System;
using System.Globalization;

public class Skill_0001_Detect : MonoBehaviour
{
    private Vector3 craftVector = Vector3.zero;
    private Vector3 newPos = Vector3.zero;

    private GameObject _buttonGroup;

    public float changeValue = 21f;
    public string idName;
    // ���� ���� �޾Ƽ� ���ҽ� �н� ��ũ �޾ƿ� ����

    public void CraftingModeStart()
    {
        _buttonGroup = GamePlay.Instance.skillManager.buttonGroup;
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
        transform.position = newPos;
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

    private void OnMouseDrag()
    {
        CheckCoordinates(); // ���콺 ��ġ�� ���� ��ǥ ���
        MoveObjectByMousePosition();    // ������Ʈ�� �̵���ų �� ���콺 ��ġ�� ���� ���� �����̵���
        MoveUIByMousePosition();    // UI ��ġ �����̱�
    }
}
