using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public Player_0001 player0001;
    public Player_0002 player0002;
    public Player_0003 player0003;
    public Player_0004 player0004;

    public Unit_Day[] player_Unit;

    private bool[] possession = new bool[10];

    public void ShowUnitAtNight(bool isNight)
    {
        player0001.gameObject.SetActive(isNight);
        player0002.gameObject.SetActive(isNight);
        player0003.gameObject.SetActive(isNight);
        player0004.gameObject.SetActive(isNight);

        foreach (var unit in player_Unit)
        {
            unit.gameObject.SetActive(!isNight);
        }
    }

    public void Init()
    {
        for (int i = 0; i < possession.Length; i++)
        {
            // Prefs가 1이면 보유중, 0이면 보유X
            var str = i.ToString();
            possession[i] = PlayerPrefs.GetInt("Unit_" + str) == 1 ? true : false;
        }
    }

    public void GetUnit(int index)
    {
        if(index >= possession.Length)
        {
            Debug.Log("인덱스 오류 return");
            return;
        }

        possession[index] = true;
        var str = index.ToString();
        PlayerPrefs.SetInt("Unit_" + str, 1);
    }

    public void SaveUnitData()
    {
        for (int i = 0; i < possession.Length; i++)
        {
            if(possession[i] == true)
            {
                var str = i.ToString();
                PlayerPrefs.SetInt("Unit_" + str, 1);
            }
        }
    }
}