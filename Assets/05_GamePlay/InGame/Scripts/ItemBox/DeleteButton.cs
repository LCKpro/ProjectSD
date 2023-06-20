using UnityEngine;

public class DeleteButton : MonoBehaviour
{
	// 쓰레기통 버튼 스크립트. 클릭했으면 체크박스 체크 온오프 기능 + 매니저에 현재 상태 보냄
	private GameDefine.ItemBoxType itemBoxType = GameDefine.ItemBoxType.None;
	public GameObject checkObj;

    public void OnClick()
	{
		GamePlay.Instance.itemBoxManager.SendDeleteBtn(this);	// 매니저에 버튼 보내기
		GamePlay.Instance.itemBoxManager.OnClick_Delete();

		if(itemBoxType == GameDefine.ItemBoxType.None)
        {
			CheckDelete();
		}
		else
        {
			UnCheckDelete();
		}
	}

	private void CheckDelete()
    {
		itemBoxType = GameDefine.ItemBoxType.Delete;
		checkObj.SetActive(true);
	}

	public void UnCheckDelete()
	{
		itemBoxType = GameDefine.ItemBoxType.None;
		checkObj.SetActive(false);
	}

}
