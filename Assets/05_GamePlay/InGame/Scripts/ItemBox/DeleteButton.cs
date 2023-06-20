using UnityEngine;

public class DeleteButton : MonoBehaviour
{
	// �������� ��ư ��ũ��Ʈ. Ŭ�������� üũ�ڽ� üũ �¿��� ��� + �Ŵ����� ���� ���� ����
	private GameDefine.ItemBoxType itemBoxType = GameDefine.ItemBoxType.None;
	public GameObject checkObj;

    public void OnClick()
	{
		GamePlay.Instance.itemBoxManager.SendDeleteBtn(this);	// �Ŵ����� ��ư ������
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
