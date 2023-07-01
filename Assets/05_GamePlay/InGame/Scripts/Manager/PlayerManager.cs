using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCreator.Characters;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private Character player;

    [SerializeField]
    private CatTower catTower;

    public Character GetPlayer()
    {
        return player;
    }

    public CatTower GetCatTower()
    {
        return catTower;
    }
}
