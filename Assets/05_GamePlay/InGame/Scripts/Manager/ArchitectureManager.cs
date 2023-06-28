using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchitectureManager : MonoBehaviour
{
    private Dictionary<KeyValuePair<int, int>, int> architectureData = new Dictionary<KeyValuePair<int, int>, int>();
    private GamePlay gamePlay = null;

    public void Init()
    {
        if (gamePlay == null)
        {
            gamePlay = GamePlay.Instance;
        }

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k <= 30; k++)
                {
                    for (int l = 0; l <= 30; l++)
                    {
                        var strI = i.ToString();
                        var strJ = j.ToString();
                        var strk = k > 10 ? k.ToString() : "0" + k.ToString();
                        var strL = l > 10 ? l.ToString() : "0" + l.ToString();
                        var totalStr = strI + strk + strJ + strL;

                        var info = PlayerPrefs.GetInt(totalStr);
                        if (info != 0)
                        {
                            var x = i == 0 ? (-k) : k;
                            var z = j == 0 ? (-l) : l;
                            BuildArchitecture(info, x, z);
                        }
                    }
                }
            }
        }
    }

    public void BuildArchitecture(int index, int x, int z)
    {
        var typeCode = index % 100;
        var bCode = index / 100;

        var building = gamePlay.spawnManager.SpawnStructure(typeCode, bCode);
        building.position = new Vector3(x, 0, z);
    }

    public void SaveArchitecture(int index, int x, int z)
    {
        var strI = x < 0 ? 0 : 1;
        var strJ = z < 0 ? 0 : 1;
        var strK = Mathf.Abs(x);
        var strL = Mathf.Abs(z);
        var total = strI.ToString() + strK.ToString() + strJ.ToString() + strL.ToString();
        Debug.Log("Prefs ÀúÀå : " + total);
        PlayerPrefs.SetInt(total, index);
    }
}
