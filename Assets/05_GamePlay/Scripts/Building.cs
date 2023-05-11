using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField]
    private BuildingSort buildingSort;

    [SerializeField]
    private int buildingLevel;

    [SerializeField]
    private int maxItemValue;

    [SerializeField]
    private int minItemValue;

    [SerializeField]
    private bool isRootable;

    [SerializeField]
    private List<GameCreator.Inventory.Item> itemList;
}
