using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    // 재화, 유닛 보유 수, 건물, 아이템 ETC, 일자

    ///////////////////////////////////////////////////////////////////////////////////////
    // 재화 : 골드, 포인트
    // Gold, int - Point, int
    ///////////////////////////////////////////////////////////////////////////////////////
    // 유닛 보유 수 : 종류 하나당 최대 한개 소유 그래서 각 캐릭터에 bool로 처리
    // Unit_0001~0010
    ///////////////////////////////////////////////////////////////////////////////////////
    // 건물 : 건물 지을 때 포스 받아서 이차원 배열 형태로 저장.
    // 예시 - PlayerPref.GetInt("000000~000000", 건물 인덱스)
    // 앞 3자리 - 0~1 : 음수/양수 00~30 x좌표 뒤 3자리 - 0~1 : 음수/양수 00~30 z좌표
    ///////////////////////////////////////////////////////////////////////////////////////
    // 아이템 ETC : 아이템 키값, 개수int
    ///////////////////////////////////////////////////////////////////////////////////////
    // 일자 : Stage, 날자 int
    ///////////////////////////////////////////////////////////////////////////////////////


    public int Player_Gold { get => player_Gold; private set => player_Gold = value; }
    private int player_Gold;

    public int Player_Point { get => player_Point; private set => player_Point = value; }
    private int player_Point;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        player_Gold = PlayerPrefs.GetInt("Gold", 0);        // 골드, 포인트 불러오기
        player_Point = PlayerPrefs.GetInt("Point", 0);

        GamePlay.Instance.architectureManager.Init();   // 건물 정보 불러오기

        GamePlay.Instance.unitManager.Init();       // 유닛 정보 불러오기
        Core.Instance.itemManager.InitItemData();   // 아이템 불러오기
        GamePlay.Instance.stageManager.Init();      //  스테이지 정보 불러오기
    }

    private bool isSaveClick = false;
    public void SaveData()
    {
        if(isSaveClick == true)
        {
            return;
        }

        isSaveClick = true;

        PlayerPrefs.SetInt("Gold", player_Gold);        // 골드, 포인트 저장
        PlayerPrefs.SetInt("Point", player_Point);

        GamePlay.Instance.unitManager.SaveUnitData();       // 유닛 정보 저장

        // 건물은 실시간 저장이라 빽

        Core.Instance.itemManager.SaveItemData();   // 아이템 저장
        GamePlay.Instance.stageManager.SaveStage();     // 스테이지 저장Init

        Invoke("SaveCoolTime", 5f);
    }

    public void SaveCoolTime()
    {
        isSaveClick = false;
    }

    
    /// <summary>
    /// 죽으면 데이터 전부 삭제
    /// </summary>
    public void DeleteAllData()
    {
        PlayerPrefs.DeleteAll();
    }
}
