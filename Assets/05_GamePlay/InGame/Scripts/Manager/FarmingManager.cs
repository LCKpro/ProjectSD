using UnityEngine;
using GameCreator.Core;

public class FarmingManager : MonoBehaviour
{
    private GameDefine.FarmingType farmingType = GameDefine.FarmingType.OffFarming;

    public Actions goToFarmAction;
    public Actions goToStageAction;

    public Animator loadingAnim;

    public void OnClick_GoToFarm()
    {
        if(farmingType == GameDefine.FarmingType.OffFarming)
        {
            loadingAnim.SetTrigger("Loading");
            goToFarmAction.Execute();
            farmingType = GameDefine.FarmingType.OnFarming;
        }
        else if(farmingType == GameDefine.FarmingType.OnFarming)
        {
            loadingAnim.SetTrigger("Loading");
            goToStageAction.Execute();
            farmingType = GameDefine.FarmingType.OffFarming;
        }
    }
}
