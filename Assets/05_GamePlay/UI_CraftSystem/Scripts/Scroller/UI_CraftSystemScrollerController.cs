using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftSystemScrollerController : MonoBehaviour, IEnhancedScrollerDelegate
{
    // Enhanced Scroller
    public EnhancedScroller scroller;

    // ȭ�鿡 ��Ÿ���� ���� ������
    public UI_CraftSystemCellView craftSystemCellViewPrefab;

    // �� �ϳ��� �� ������ ��
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
        // ������ �ִ� ������ ���ġ
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
