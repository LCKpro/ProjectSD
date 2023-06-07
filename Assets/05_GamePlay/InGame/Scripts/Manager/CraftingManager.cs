using UnityEngine;
using UniRx;
using System;
using System.Globalization;
using GameCreator.Characters;
using GameCreator.Core;

public class CraftingManager : MonoBehaviour
{
    // 현재 크래프팅 모드가 켜져있는지?
    public bool isOn;
    private IDisposable craftingModeTimer = Disposable.Empty;

    private Vector3 craftVector = Vector3.zero;
    private Vector3 newPos = Vector3.zero;

    public GameObject buttonGroup;
    public NavigationMarker marker;
    public Transform spawnPos;

    public GameObject loadingUIObj;

    public float changeValue = 12f;

    // 임시용 코드. 엑셀 파일 받아서 리소스 패스 링크 받아올 예정
    private string _buildingCode = "";
    private int _typeCode = 0;
    private int _code = 0;
    // 건물 짓기 전 예비 오브젝트
    private GameObject preparatoryObj;

    public Actions playerMoveAction;


    /// <summary>
    /// 크래프팅 모드 종료
    /// </summary>
    public void FinalizeCraft()
    {
        ManageActive(false); // 오브젝트 관리 - 전부 끄기
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

    // 오브젝트 액티브 관리. 버튼 처음에 안보이도록
    private void ManageActive(bool isOn)
    {
        buttonGroup.SetActive(isOn);
    }

    /// <summary>
    /// 크래프팅 시작. DB의 키값을 받아와서 해당 건물을 건축시도할 수 있도록 기능
    /// </summary>
    /// <param name="key"></param>
    public void InitCraft(int key)
    {
        //GetReadyToCraft(key);       // 키 받고 오브젝트 준비
        //GetReadyToCraft("Appliances_Store");     // 키 받고 오브젝트 준비(더미 코드)
        CraftingModeEnd();  // 움직임 감지해서 크래프팅 꺼지도록 수정
    }

    /// <summary>
    /// 크래프팅 시작. DB의 키값을 받아와서 해당 건물을 건축시도할 수 있도록 기능
    /// </summary>
    /// <param name="key"></param>
    public void InitCraft(string key)
    {
        //GetReadyToCraft(key);       // 키 받고 오브젝트 준비
        //GetReadyToCraft("Appliances_Store");     // 키 받고 오브젝트 준비(더미 코드)
        CraftingModeEnd();  // 움직임 감지해서 크래프팅 꺼지도록 수정
    }

    public void InitCraft(int type, int key)
    {
        GetReadyToCraft(type, key);       // 키 받고 오브젝트 준비
        //GetReadyToCraft("Appliances_Store");     // 키 받고 오브젝트 준비(더미 코드)
        CraftingModeEnd();  // 움직임 감지해서 크래프팅 꺼지도록 수정
    }

    public string GetBuildingCode()
    {
        return _buildingCode;
    }


    // 타입 코드는 건물의 타입(공,방,장식), 일반 코드는 건물의 인덱스 번호
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

        // 여기서 버그나면 매니저랑 DB의 인덱스가 일치하지 않아서 그럼
        preparatoryObj = GamePlay.Instance.spawnManager.SpawnIncompleteStructure(_typeCode, _code).gameObject;

        // 플레이어 앞에 설치하기 위한 준비
        Vector3 playerPos = GamePlay.Instance.playerManager.GetPlayer().transform.position;
        playerPos.z += 2.0f;
        playerPos.y = 0;

        preparatoryObj.transform.position = playerPos;

        // 스폰된 건물의 위치에 맞게 UI도 위치 변경
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
    /// 움직일 때 크래프팅 모드 종료
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
                Debug.Log("종료V : " + Input.GetAxis("Vertical"));
                Debug.Log("종료H : " + Input.GetAxis("Horizontal"));
                FinalizeCraft();    // UniRx 하고 다른 버튼들 다 끄기
            });
    }

    // 버튼 클릭시 미리 옮겨놓은 마커를 향해 플레이어가 움직임
    // 마커를 향해 움직이는 사이에 플레이어 Input이 감지되면 멈추는 UniRx를 추가해야 할듯
    // 버튼 클릭. 건물 짓기. 캐릭터 움직이기 + 애니메이션은 액션으로 대체
    public void OnClick_ClickToCraft()
    {
        FinalizeCraft();    // UniRx 하고 다른 버튼들 다 끄기
        playerMoveAction.Execute();
    }

    public void SpawnLoadingUI()
    {
        loadingUIObj.SetActive(true);

        Vector3 playerPos = GamePlay.Instance.playerManager.GetPlayer().transform.position;
        Vector3 uiPos = Camera.main.WorldToScreenPoint(playerPos);

        loadingUIObj.transform.position = uiPos;
    }

    #region 미사용 코드

    public void GetReadyToCraft(string code)
    {
        _buildingCode = code;
        
        //preparatoryObj = GamePlay.Instance.spawnManager.SpawnIncompleteStructure(0).gameObject;

        // 플레이어 앞에 설치하기 위한 준비
        Vector3 playerPos = GamePlay.Instance.playerManager.GetPlayer().transform.position;
        playerPos.z += 2.0f;
        playerPos.y = 0;

        preparatoryObj.transform.position = playerPos;

        // 스폰된 건물의 위치에 맞게 UI도 위치 변경
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
        Debug.Log("마우스 월드 좌표: " + craftVector);
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
        preparatoryObj.transform.position = marker.transform.position = newPos;

        Debug.Log("오브젝트 월드 좌표: " + preparatoryObj.transform.position);
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

        buttonGroup.transform.position = new Vector3(x, y, 0);
        Debug.Log("UI 좌표: " + buttonGroup.transform.position);
    }

    public void FinishCraft()
    {
        GameObject obj = Resources.Load<GameObject>("GameObject/Appliances_Store");
        obj.transform.position = marker.transform.position;
    }

    #endregion
}
