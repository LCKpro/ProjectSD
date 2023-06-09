using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private Player_0001 player0001;
    public void ReadySkill0001(Player_0001 player)
    {
        player0001 = player;
    }
    
    public void ActiveSkill0001(AI_Structure target)
    {
        if(player0001 != null)
        {
            player0001.ActiveSkill0001(target);
        }
    }

}
