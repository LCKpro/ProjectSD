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

    // 엑셀 파일 받아서 리소스 패스 링크 받아올 예정

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

    // 매니저에서 데이터 가져오기
    private void SetData()
    {
        var manager = GamePlay.Instance.craftingManager;
        _marker = manager.marker;
        _marker_LookAt = manager.marker_LookAt;
        _spawnPos = manager.spawnPos;
        _buttonGroup = manager.buttonGroup;
    }

    // 오브젝트 액티브 관리
    private void ManageActive(bool isOn)
    {
        //_clickTrigger.SetActive(isOn);
        _buttonGroup.SetActive(isOn);
    }

    /// <summary>
    /// 타이머 세팅. 매 업데이트마다 좌표를 계산함
    /// </summary>
    public void CraftingModeStart()
    {
        craftingModeTimer.Dispose();
        craftingModeTimer = Disposable.Empty;
        CheckCoordinates(); // 마우스 위치에 따른 좌표 계산
        MoveObjectByMousePosition();    // 오브젝트를 이동시킬 때 마우스 위치에 따라 딱딱 움직이도록
        MoveUIByMousePosition();    // UI 위치 움직이기
        ManageActive(true); // 오브젝트 관리 - 전부 켜기
        craftingModeTimer = Observable.EveryUpdate().TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                CheckCoordinates(); // 마우스 위치에 따른 좌표 계산
                MoveObjectByMousePosition();    // 오브젝트를 이동시킬 때 마우스 위치에 따라 딱딱 움직이도록
                MoveUIByMousePosition();    // UI 위치 움직이기
            });
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
        //transform.position = _marker.transform.position = newPos;
        transform.position = newPos;
        _spawnPos.position = newPos;
        SetMarkerPos(newPos);
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

    private void SetMarkerPos(Vector3 targetPos)
    {
        var vec = GamePlay.Instance.playerManager.GetPlayer().transform.position - targetPos;

        _marker.transform.position = targetPos + (vec.normalized * 3.5f);
        _marker_LookAt.transform.position = targetPos;
    }

    private void OnMouseDrag()
    {
        CheckCoordinates(); // 마우스 위치에 따른 좌표 계산
        MoveObjectByMousePosition();    // 오브젝트를 이동시킬 때 마우스 위치에 따라 딱딱 움직이도록
        MoveUIByMousePosition();    // UI 위치 움직이기
    }
}
