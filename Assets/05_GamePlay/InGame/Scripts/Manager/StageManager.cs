using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Cysharp.Threading.Tasks;

[Serializable]
public class MonsterData : SerializableDictionary<int, int> {   }

[Serializable]
public class WaveData
{
    public GameDefine.WaveType waveType;
    public MonsterData monsterData;
}

[Serializable]
public class StageData
{
    public List<WaveData> stageData;
}

public class StageManager : MonoBehaviour
{
    private int currentStage = 1;
    private int currentWave = 0;

    // 인스펙터에서 스테이지 설정
    public List<StageData> stageDataList;

    public Transform[] spawnPos;

    private SpawnManager spawnMananer = null;
    private IDisposable _timer = Disposable.Empty;

    #region Data

    public void Init()
    {
        if (spawnMananer == null)
        {
            spawnMananer = GamePlay.Instance.spawnManager;
        }
        currentStage = PlayerPrefs.GetInt("Stage", 1);

        // 임시 코드
        StartSequence();
    }

    public void SaveStage()
    {
        PlayerPrefs.SetInt("Stage", currentStage);
    }

    public void ClearStage()
    {
        currentWave = 0;
        currentStage++;
        SaveStage();
    }

    #endregion

    private void StopTimer()
    {
        _timer.Dispose();
        _timer = Disposable.Empty;
    }

    public void StartSequence()
    {
        if (currentStage >= stageDataList.Count)
        {
            return;
        }

        StopTimer();
        PrepareSpawn();
        SoundManager.instance.PlayBGM("Battle");
        SoundManager.instance.PlaySound("BattleStart");
        int count = 0;
        _timer = Observable.Interval(TimeSpan.FromSeconds(60f)).TakeUntilDisable(gameObject)
           .TakeUntilDestroy(gameObject)
           .Subscribe(_ =>
           {
               count++;

               if(count >= 5)
               {
                   count = 0;
                   StopTimer();
               }
               else
               {
                   PrepareSpawn();
               }
           });
    }

    private void PrepareSpawn()
    {
        var waveData = stageDataList[currentStage].stageData[currentWave];

        switch (waveData.waveType)
        {
            case GameDefine.WaveType.Linear:
                LinearSpawn();
                break;
            case GameDefine.WaveType.Circle:
                CircleSpawn();
                break;
            case GameDefine.WaveType.Double:
                DoubleSpawn();
                break;
            case GameDefine.WaveType.OnlyTop:
                OneDirectionSpawn(0);
                break;
            case GameDefine.WaveType.OnlyBottom:
                OneDirectionSpawn(6);
                break;
            case GameDefine.WaveType.OnlyLeft:
                OneDirectionSpawn(4);
                break;
            case GameDefine.WaveType.OnlyRight:
                OneDirectionSpawn(2);
                break;
            case GameDefine.WaveType.None:
                break;
            default:
                break;
        }
    }

    public void LinearSpawn()
    {
        var waveData = stageDataList[currentStage].stageData[currentWave].monsterData;

        UniTask.Create(async () =>
        {
            try
            {
                foreach (var data in waveData)
                {
                    spawnMananer.SpawnMonster(data.Key, data.Value / 4, spawnPos[0].position);
                    spawnMananer.SpawnMonster(data.Key, data.Value / 4, spawnPos[2].position);
                    spawnMananer.SpawnMonster(data.Key, data.Value / 4, spawnPos[4].position);
                    spawnMananer.SpawnMonster(data.Key, data.Value / 4, spawnPos[6].position);

                    await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
                }
            }
            catch (Exception ex)
            {
                GameUtils.Error(ex.ToString());
            }
        });
    }

    public void CircleSpawn()
    {
        var waveData = stageDataList[currentStage].stageData[currentWave].monsterData;

        UniTask.Create(async () =>
        {
            try
            {
                foreach (var data in waveData)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        spawnMananer.SpawnMonster(data.Key, data.Value / 8, spawnPos[i].position);
                    }

                    await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
                }
            }
            catch (Exception ex)
            {
                GameUtils.Error(ex.ToString());
            }
        });
    }

    public void DoubleSpawn()
    {
        
    }

    public void OneDirectionSpawn(int dir)
    {
        var waveData = stageDataList[currentStage].stageData[currentWave].monsterData;

        UniTask.Create(async () =>
        {
            try
            {
                foreach (var data in waveData)
                {
                    spawnMananer.SpawnMonster(data.Key, data.Value, spawnPos[dir].position);
                    await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
                }
            }
            catch (Exception ex)
            {
                GameUtils.Error(ex.ToString());
            }

        });
    }
}