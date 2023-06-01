using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftSystemScrollerController : MonoBehaviour, IEnhancedScrollerDelegate
{
    

    // Enhanced Scroller
    public EnhancedScroller scroller;

    // 화면에 나타나는 셀뷰 프리팹
    public UI_CraftSystemCellView craftSystemCellViewPrefab;

    private List<Dictionary<string, object>> _data_Building_ATK;
    private List<Dictionary<string, object>> _data_Building_DFS;
    private List<Dictionary<string, object>> _data_Building_PD;
    private List<Dictionary<string, object>> _data_Building_OA;
    private List<Dictionary<string, object>> _data_Building;

    private bool isStart = false;

    private BuildingType _type = BuildingType.None;

    public void SetData(BuildingType type)
    {
        _type = type;

        if (isStart == false)
        {
            scroller.Delegate = this;
            scroller.lookAheadAfter = 500;
            scroller.lookAheadBefore = 500;

            SetCSV();
            isStart = true;
        }
        else
        {
            SetType();
        }

        LoadCraftSystemData();
    }

    /// <summary>
    /// CSV 파일 리딩
    /// </summary>
    private void SetCSV()
    {
        _data_Building_ATK = CSVReader.Read("CTS_BuildingATK_DB");
        _data_Building_DFS = CSVReader.Read("CTS_BuildingDFS_DB");
        _data_Building_PD = CSVReader.Read("CTS_BuildingPD_DB");
        _data_Building_OA = CSVReader.Read("CTS_BuildingOA_DB");

        _data_Building = _data_Building_ATK;
    }

    private void SetType()
    {
        switch (_type)
        {
            case BuildingType.ATK:
                _data_Building = _data_Building_ATK;
                break;
            case BuildingType.DFS:
                _data_Building = _data_Building_DFS;
                break;
            case BuildingType.PD:
                _data_Building = _data_Building_PD;
                break;
            case BuildingType.OA:
                _data_Building = _data_Building_OA;
                break;
            case BuildingType.None:
                Debug.Log("버그");
                _data_Building = _data_Building_ATK;
                break;
            default:
                break;
        }
    }

    public void LoadCraftSystemData()
    {
        // 가지고 있는 데이터 재배치
        scroller.ReloadData();
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return CellCount();
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 180.0f;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        UI_CraftSystemCellView cellView = null;

        cellView = scroller.GetCellView(craftSystemCellViewPrefab) as UI_CraftSystemCellView;

        cellView.SetData(_data_Building[dataIndex]);

        return cellView;
    }

    public int CellCount()
    {
        if(_data_Building != null)
        {
            return _data_Building.Count;
        }

        return 0;
    }
}

public enum BuildingType
{
    ATK = 0,
    DFS = 1,
    PD = 2,
    OA = 3,
    None = 4,
}
