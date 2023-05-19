using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCreator.Characters;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private Character player;

    public Character GetPlayer()
    {
        return player;
    }
}
