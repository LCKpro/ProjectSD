using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField]
    private int uuid_Gold;
    [SerializeField]
    private int uuid_Point;
    [SerializeField]
    private int uuid_Wood;
    [SerializeField]
    private int uuid_Stone;
    [SerializeField]
    private int uuid_Diamond;
    [SerializeField]
    private int uuid_Oil;

    public List<int> uuidList;

    public int UUID_Gold => uuid_Gold;
    public int UUID_Point => uuid_Point;
    public int UUID_Wood => uuid_Wood;
    public int UUID_Stone => uuid_Stone;
    public int UUID_Diamond => uuid_Diamond;
    public int UUID_Oil => uuid_Oil;

    private void Awake()
    {
        uuidList = new List<int>();

        uuidList.Add(uuid_Gold);
        uuidList.Add(uuid_Point);
        uuidList.Add(uuid_Wood);
        uuidList.Add(uuid_Stone);
        uuidList.Add(uuid_Diamond);
        uuidList.Add(uuid_Oil);
    }
}
