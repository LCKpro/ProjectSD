using System.Collections.Generic;
using UnityEngine;

// 리소스 폴더에서 UI를 관리하는 클래스
public class InGamePools : MonoBehaviour
{
    private Dictionary<string, List<Transform>> spawnPoolList = new Dictionary<string, List<Transform>>();
    private Dictionary<string, Queue<Transform>> despawnPoolList = new Dictionary<string, Queue<Transform>>();

    private Dictionary<Transform, List<Transform>> linkedDespawnPrefabList = new Dictionary<Transform, List<Transform>>();

    public void Awake()
    {
        spawnPoolList.Clear();
        despawnPoolList.Clear();
    }

    #region action

    /// <summary>
    /// 디스폰 풀에 없는 경우 풀을 만들고 넣음.
    /// 이미 있는 경우 아무것도 하지 않음.
    /// </summary>
    public void PoolingAndAddToDespawnPool(string itemObjName, int cnt, Transform parent = null)
    {
        if (despawnPoolList.ContainsKey(itemObjName) == false || despawnPoolList[itemObjName].Count == 0)
        {
            GameObject resourceItem = Resources.Load<GameObject>($"InGamePools/{itemObjName}");
            if (resourceItem == null)
            {
                GameUtils.Error("InGamePools Spawn Fail : " + itemObjName);
                return;
            }

            for (int i = 0; i < cnt; i++)
            {
                var item = Instantiate(resourceItem).transform;
                if (item != null)
                {
                    if (parent == null)
                        item.SetParent(transform);
                    else
                        item.SetParent(parent);

                    item.localPosition = new Vector3(0, 0, 0);
                    item.localEulerAngles = new Vector3(0, 0, 0);
                    item.localScale = new Vector3(1, 1, 1);
                    item.gameObject.SetActive(false);
                    AddToDespawnPool(itemObjName, item);
                }
            }
        }
    }

    public void AddToSpawnPool(string itemObjName, Transform item)
    {
        if (spawnPoolList.ContainsKey(itemObjName) == false)
        {
            Queue<Transform> newQue = new Queue<Transform>();
            newQue.Enqueue(item);
            spawnPoolList.Add(itemObjName, new List<Transform>() { item });
        }
        else
        {
            spawnPoolList[itemObjName].Add(item);
        }
    }

    public void AddToDespawnPool(string itemName, Transform itemObj)
    {
        if (despawnPoolList.ContainsKey(itemName) == false)
        {
            Queue<Transform> newQueue = new Queue<Transform>();
            newQueue.Enqueue(itemObj);
            despawnPoolList.Add(itemName, newQueue);
        }
        else
        {
            despawnPoolList[itemName].Enqueue(itemObj);
        }
    }

    public void Despawn(Transform itemObj)
    {
        itemObj.gameObject.SetActive(false);
        if (gameObject.activeSelf)
            itemObj.SetParent(transform);
        else
            itemObj.SetParent(null);

        itemObj.localPosition = new Vector3(0, 0, 0);
        itemObj.localEulerAngles = new Vector3(0, 0, 0);
        itemObj.localScale = new Vector3(1, 1, 1);
        string itemName = "";
        if (spawnPoolList.ContainsValue(itemObj) == true)
        {
            itemName = spawnPoolList.GetKey(itemObj);
            spawnPoolList.RemoveItem(itemName, itemObj);
        }

        DespawnAllLinkedItems(itemObj);

        AddToDespawnPool(itemName, itemObj);
    }

    public void DespawnAllItems(string itemName)
    {
        List<Transform> items = new List<Transform>();
        foreach (var item in GetSpawnedItems(itemName))
        {
            items.Add(item);
        }

        foreach (var item in items)
        {
            Despawn(item);
        }
    }

    #endregion

    #region condition

    public bool IsSpawned(Transform itemObj)
    {
        bool isOK = spawnPoolList.ContainsValue(itemObj) == true;

        return isOK;
    }

    public bool IsSpawned(string itemName)
    {
        bool isOK = spawnPoolList.ContainsKey(itemName) == true;

        return isOK;
    }

    #endregion

    #region function

    public List<Transform> GetSpawnedItems(string itemName)
    {
        if (IsSpawned(itemName))
            return spawnPoolList[itemName];
        return new List<Transform>();
    }

    public Transform Spawn(string itemObjName, bool isView = true, Transform parent = null)
    {
        Transform item = null;
        //GameUtils.Log("InGamePools Name : " + itemObjName);

        if (despawnPoolList.ContainsKey(itemObjName) == false || despawnPoolList[itemObjName].Count == 0)
        {
            GameObject resourceItem = Resources.Load<GameObject>($"InGamePools/{itemObjName}");
            if (resourceItem == null)
            {
                GameUtils.Log("InGamePools Spawn Fail : " + itemObjName);
                return null;
            }

            item = Instantiate(resourceItem).transform;
            if (item != null)
            {
                if (parent == null)
                {
                    item.SetParent(transform);
                }
                else
                {
                    item.SetParent(parent);
                }

                item.localPosition = new Vector3(0, 0, 0);
                item.localEulerAngles = new Vector3(0, 0, 0);
                //item.localScale = new Vector3(1, 1, 1);
                AddToSpawnPool(itemObjName, item);
                //GameUtils.Log("InGamePools Spawn Success : " + itemObjName);
            }
        }
        else
        {
            item = despawnPoolList.DequeueItem(itemObjName);
            if (item != null)
            {
                if (parent == null)
                {
                    item.SetParent(transform);
                }
                else
                {
                    item.SetParent(parent);
                }

                item.localPosition = new Vector3(0, 0, 0);
                item.localEulerAngles = new Vector3(0, 0, 0);
                item.localScale = new Vector3(1, 1, 1);
                AddToSpawnPool(itemObjName, item);
            }
        }

        if (item != null)
        {
            item.gameObject.SetActive(isView);
        }

        return item;
    }

    public Transform SpawnWithoutPools(string itemObjName, bool isView = true, Transform parent = null)
    {
        Transform item = null;
        //GameUtils.Log("InGamePools Name : " + itemObjName);

        GameObject resourceItem = Resources.Load<GameObject>($"InGamePools/{itemObjName}");
        if (resourceItem == null)
        {
            GameUtils.Error("InGamePools Spawn Fail : " + itemObjName);
            return null;
        }

        item = Instantiate(resourceItem).transform;
        if (item != null)
        {
            if (parent == null)
            {
                item.SetParent(transform);
            }
            else
            {
                item.SetParent(parent);
            }

            item.localPosition = new Vector3(0, 0, 0);
            item.localEulerAngles = new Vector3(0, 0, 0);
            item.localScale = new Vector3(1, 1, 1);
        }
        if (item != null)
        {
            item.gameObject.SetActive(isView);
        }

        return item;
    }

    public T Spawn<T>(string itemObjName, bool isView = true, Transform parent = null)
    {
        return Spawn(itemObjName, isView, parent).GetComponent<T>();
    }

    public Transform OutGameSpawn(string itemObjName, bool isView = true, Transform parent = null)
    {
        Transform item = null;
        //GameUtils.Log("InGamePools Name : " + itemObjName);

        if (despawnPoolList.ContainsKey(itemObjName) == false || despawnPoolList[itemObjName].Count == 0)
        {
            GameObject resourceGO = Resources.Load<GameObject>(string.Format("PopUpPools/{0}", itemObjName));
            if (resourceGO == null)
            {
                GameUtils.Error("InGamePools Spawn Fail : " + itemObjName);
                return null;
            }

            item = Instantiate(resourceGO).transform;
            if (item != null)
            {
                if (parent == null)
                {
                    item.SetParent(transform);
                }
                else
                {
                    item.SetParent(parent);
                }

                item.localPosition = new Vector3(0, 0, 0);
                item.localEulerAngles = new Vector3(0, 0, 0);
                item.localScale = new Vector3(1, 1, 1);
                AddToSpawnPool(itemObjName, item);
                //GameUtils.Log("InGamePools Spawn Success : " + itemObjName);
            }
        }
        else
        {
            item = despawnPoolList.DequeueItem(itemObjName);
            if (item != null)
            {
                if (parent == null)
                {
                    item.SetParent(transform);
                }
                else
                {
                    item.SetParent(parent);
                }

                item.localPosition = new Vector3(0, 0, 0);
                item.localEulerAngles = new Vector3(0, 0, 0);
                item.localScale = new Vector3(1, 1, 1);
                AddToSpawnPool(itemObjName, item);
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

    public T OutGameSpawn<T>(string itemObjName, bool isView = true, Transform parent = null)
    {
        return OutGameSpawn(itemObjName, isView, parent).GetComponent<T>();
    }

    #endregion

    #region 연동

    public bool HaveLinkedDespawnItem(Transform from)
    {
        foreach (var item in linkedDespawnPrefabList)
        {
            if (ReferenceEquals(from, item.Key)) return true;
        }

        return false;
    }

    public List<Transform> GetLinkedDespawnItems(Transform from)
    {
        if (HaveLinkedDespawnItem(from) == false) return null;
        return linkedDespawnPrefabList[from];
    }

    public void DespawnAllLinkedItems(Transform from)
    {
        List<Transform> items = GetLinkedDespawnItems(from);
        if (items == null) return;
        foreach (var item in items)
        {
            Despawn(item);
        }

        linkedDespawnPrefabList.Remove(from);
    }

    #endregion
}

public static class InGamePoolsFunction
{
    public static void RemoveItem(this Dictionary<string, List<Transform>> itemList, string key, Transform value)
    {
        itemList[key].Remove(value);
        if (itemList[key].Count == 0) itemList.Remove(key);
    }

    public static Transform DequeueItem(this Dictionary<string, Queue<Transform>> itemList, string key)
    {
        Transform item = itemList[key].Dequeue();
        if (itemList[key].Count == 0)
        {
            itemList[key].Clear();
            itemList.Remove(key);
        }

        return item;
    }

    public static string GetKey(this Dictionary<string, List<Transform>> itemList, Transform value)
    {
        foreach (var item in itemList)
        {
            if (item.Value.ContainsValue(value)) return item.Key;
        }

        return null;
    }

    public static bool ContainsValue(this Dictionary<string, List<Transform>> itemList, Transform value)
    {
        foreach (var item in itemList)
        {
            if (item.Value.ContainsValue(value)) return true;
        }

        return false;
    }

    public static bool ContainsValue(this List<Transform> itemList, Transform value)
    {
        foreach (var item in itemList)
        {
            if (ReferenceEquals(item, value)) return true;
        }

        return false;
    }
}