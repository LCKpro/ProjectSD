using UnityEngine;
using Redcode.Pools;

public class GamePlay : MonoBehaviour
{
    #region SingleTon

    private static GamePlay _instance = null;

    public static GamePlay Instance
    {
        get
        {
            SetInstance();
            return _instance;
        }
    }

    public static void SetInstance()
    {
        if (_instance == null)
        {
            GameObject go = GameObject.Find("GamePlay");
            if (go != null)
            {
                _instance = go.GetComponent<GamePlay>();
                if (_instance == null)
                {
                    Debug.Log("GamePlay Instance Null");
                }
            }
        }
    }

    void OnDestroy()
    {
        _instance = null;
    }

    void OnDisable()
    {
        _instance = null;
    }

    #endregion

    public PlayerManager playerManager;
    public NatureManager natureManager;
    public CraftingManager craftingManager;
    public UI_System ui_System;
    public InGamePools inGamePools;
    public PoolManager poolManager_Monster;
    public PoolManager poolManager_Projectile;
    public SpawnManager spawnManager;
    public UI_ClockSystem uI_ClockSystem;

    public PoolManager[] poolManager_StructureArray;
    public PoolManager[] poolManager_IncompleteStructureArray;

    public GameStateManager gameStateManager;
    public SkillManager skillManager;
    public UnitManager unitManager;

    #region 버튼

    public void OnClick_ShowCraftSystem()
    {
        if(gameStateManager.CheckStateType(GameDefine.StateType.Build) == true)
        {
            return;
        }

        ui_System.gameObject.SetActive(true);
        ui_System.OnClick_StartCraftSystem();
    }

    public void OnClick_OpenInventory()
    {
        if(gameStateManager.CheckStateType(GameDefine.StateType.OpenInventory) == true)
        {
            gameStateManager.SetStateType(GameDefine.StateType.None);
            GameCreator.Inventory.InventoryUIManager.CloseInventory();
        }
        else if(gameStateManager.CheckStateType(GameDefine.StateType.None) == true)
        {
            gameStateManager.SetStateType(GameDefine.StateType.OpenInventory);
            GameCreator.Inventory.InventoryUIManager.OpenInventory();
        }
        else
        {
            Debug.Log("건축중이나 건물 파밍중에는 인벤토리를 열 수 없음");
        }
    }

    #endregion
}
