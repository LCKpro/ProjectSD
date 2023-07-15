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
    // 엑셀 파일 받아서 리소스 패스 링크 받아올 예정

    public void CraftingModeStart()
    {
        _buttonGroup = GamePlay.Instance.skillManager.buttonGroup;
    }

    // 마우스 위치에 따른 좌표 계산
    private void CheckCoordinates()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = changeValue; // 카메라와의 거리 설정

        //buttonGroup.transform.position = Input.mousePosition;
        craftVector = Camera.main.ScreenToWorldPoint(mousePosition);
    }

    // 오브젝트를 이동시킬 때 마우스 위치에 따라 딱딱 움직이도록 수정
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

    // UI는 오브젝트랑 좌표가 다르므로 따로 처리
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
        CheckCoordinates(); // 마우스 위치에 따른 좌표 계산
        MoveObjectByMousePosition();    // 오브젝트를 이동시킬 때 마우스 위치에 따라 딱딱 움직이도록
        MoveUIByMousePosition();    // UI 위치 움직이기
    }
}
