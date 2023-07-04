using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatTower : AI_Structure
{
    protected override void Die()
    {
        base.Die();

        GamePlay.Instance.gameDataManager.DeleteAllData();
        GamePlay.Instance.gameOverManager.GameOver();
    }
}
