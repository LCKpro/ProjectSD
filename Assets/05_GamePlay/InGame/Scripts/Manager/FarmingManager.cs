using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using GameCreator.Core;

public class FarmingManager : MonoBehaviour
{
    public Actions goToFarmAction;

    public Animator loadingAnim;

    public void OnClick_GoToFarm()
    {
        loadingAnim.SetTrigger("Loading");
        goToFarmAction.Execute();
    }
}
