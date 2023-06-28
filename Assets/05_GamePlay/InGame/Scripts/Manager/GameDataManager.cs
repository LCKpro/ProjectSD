using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    // ��ȭ, ���� ���� ��, �ǹ�, ������ ETC, ����

    ///////////////////////////////////////////////////////////////////////////////////////
    // ��ȭ : ���, ����Ʈ
    // Gold, int - Point, int
    ///////////////////////////////////////////////////////////////////////////////////////
    // ���� ���� �� : ���� �ϳ��� �ִ� �Ѱ� ���� �׷��� �� ĳ���Ϳ� bool�� ó��
    // Unit_0001~0010
    ///////////////////////////////////////////////////////////////////////////////////////
    // �ǹ� : �ǹ� ���� �� ���� �޾Ƽ� ������ �迭 ���·� ����.
    // ���� - PlayerPref.GetInt("000000~000000", �ǹ� �ε���)
    // �� 3�ڸ� - 0~1 : ����/��� 00~30 x��ǥ �� 3�ڸ� - 0~1 : ����/��� 00~30 z��ǥ
    ///////////////////////////////////////////////////////////////////////////////////////
    // ������ ETC : ������ Ű��, ����int
    ///////////////////////////////////////////////////////////////////////////////////////
    // ���� : Stage, ���� int
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
        player_Gold = PlayerPrefs.GetInt("Gold", 0);        // ���, ����Ʈ �ҷ�����
        player_Point = PlayerPrefs.GetInt("Point", 0);

        GamePlay.Instance.architectureManager.Init();   // �ǹ� ���� �ҷ�����

        GamePlay.Instance.unitManager.Init();       // ���� ���� �ҷ�����
        Core.Instance.itemManager.InitItemData();   // ������ �ҷ�����
        GamePlay.Instance.stageManager.Init();      //  �������� ���� �ҷ�����
    }

    private bool isSaveClick = false;
    public void SaveData()
    {
        if(isSaveClick == true)
        {
            return;
        }

        isSaveClick = true;

        PlayerPrefs.SetInt("Gold", player_Gold);        // ���, ����Ʈ ����
        PlayerPrefs.SetInt("Point", player_Point);

        GamePlay.Instance.unitManager.SaveUnitData();       // ���� ���� ����

        // �ǹ��� �ǽð� �����̶� ��

        Core.Instance.itemManager.SaveItemData();   // ������ ����
        GamePlay.Instance.stageManager.SaveStage();     // �������� ����Init

        Invoke("SaveCoolTime", 5f);
    }

    public void SaveCoolTime()
    {
        isSaveClick = false;
    }

    
    /// <summary>
    /// ������ ������ ���� ����
    /// </summary>
    public void DeleteAllData()
    {
        PlayerPrefs.DeleteAll();
    }
}
