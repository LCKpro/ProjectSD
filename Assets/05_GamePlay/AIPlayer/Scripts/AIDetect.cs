using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDetect : MonoBehaviour
{
    public AIPlayer player;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Breakable")
        {
            player.AttackStart(other);
        }
    }
}
