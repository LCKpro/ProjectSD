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

    // 행 하나에 들어갈 프리팹 수
    public int numberOfCellsPerRow = 5;

    public void SetData()
    {
        scroller.Delegate = this;
        scroller.lookAheadAfter = 500;
        scroller.lookAheadBefore = 500;

        LoadFastRewardData();
    }

    public void LoadFastRewardData()
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
        return 160.0f;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        UI_CraftSystemCellView cellView = null;

        cellView = scroller.GetCellView(craftSystemCellViewPrefab) as UI_CraftSystemCellView;

        cellView.SetData();

        return cellView;
    }

    public int CellCount()
    {
        int count = 5;

        return count;
    }
}
