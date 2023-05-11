using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField]
    private List<GameCreator.Core.Actions> actionList;


    private void Awake()
    {
    }

}

public enum BuildingSort
{
    Apartment = 0,
    ConvenienceStore = 1,
    Restaurant = 2,
    Factory = 3,
    Hospital = 4,
    MilitaryBase = 5,
    GasStation = 6,
    SuperMarket = 7,
    GunStore = 8,
}
