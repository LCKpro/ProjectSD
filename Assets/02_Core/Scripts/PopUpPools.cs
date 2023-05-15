using System.Collections.Generic;
using UnityEngine;

// 리소스 폴더에서 UI를 관리하는 클래스
public class PopUpPools : MonoBehaviour
{
    private Dictionary<string, Transform> spawnPopUpList;

    public void Awake()
    {
        spawnPopUpList = new Dictionary<string, Transform>();
        spawnPopUpList.Clear();
    }

    public Transform Spawn(string popupName, bool isView = true)
    {
        Transform item = null;
        GameUtils.Log("PopUpPools popupName : " + popupName);

        if (spawnPopUpList.ContainsKey(popupName) == false)
        {
            GameObject resourceGO = Resources.Load<GameObject>(string.Format("PopUpPools/{0}", popupName));
            if (resourceGO == null)
            {
                GameUtils.Error("PopUpPools Spawn Fail : " + popupName);
                return null;
            }

            item = Instantiate(resourceGO).transform;

            if (item != null)
            {
                item.SetParent(transform);
                item.localPosition = new Vector3(0, 0, 0);
                item.localEulerAngles = new Vector3(0, 0, 0);
                item.localScale = new Vector3(1, 1, 1);
                spawnPopUpList.Add(popupName, item);
                GameUtils.Log("PopUpPools Spawn Success : " + popupName);
            }
        }
        else
        {
            item = spawnPopUpList[popupName];

            // Null 예외처리
            if (item == null)
            {
                spawnPopUpList.Remove(popupName);
            }
        }

        if (item != null)
        {
            if (isView == true)
            {
                item.gameObject.SetActive(true);
            }
        }

        return item;
    }

    public void Despawn(Transform popUpTransform)
    {
        popUpTransform.gameObject.SetActive(false);
        popUpTransform.SetParent(transform);
        popUpTransform.localPosition = new Vector3(0, 0, 0);
        popUpTransform.localEulerAngles = new Vector3(0, 0, 0);
        popUpTransform.localScale = new Vector3(1, 1, 1);
    }

    public bool IsSpawned(Transform popUpTransform)
    {
        bool isOK = false;

        if (spawnPopUpList.ContainsValue(popUpTransform) == true)
        {
            isOK = true;
        }

        return isOK;
    }

    public void Dispose(string popupName)
    {
        if (spawnPopUpList.ContainsKey(popupName))
        {
            Transform target = spawnPopUpList[popupName];

            if (target != null)
            {
                Destroy(target.gameObject);
            }

            spawnPopUpList.Remove(popupName);
        }
    }
}