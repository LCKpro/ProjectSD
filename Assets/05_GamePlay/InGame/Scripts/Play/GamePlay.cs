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
    public PoolManager poolManager_Effect;
    public SpawnManager spawnManager;

    public PoolManager[] poolManager_StructureArray;
    public PoolManager[] poolManager_IncompleteStructureArray;

    public GameStateManager gameStateManager;
    public SkillManager skillManager;
    public UnitManager unitManager;
    public ItemBoxManager itemBoxManager;
    public FarmingManager farmingManager;
    public CameraManager cameraManager;

    public GameDataManager gameDataManager;
    public StageManager stageManager;
    public ArchitectureManager architectureManager;
    public GameOverManager gameOverManager;

    public UI_DayNightSystem uI_DayNightSystem;

    #region ��ư

    public void Start()
    {
        SetResolution();
        InitGame();
    }

    private void SetResolution()
    {
        int setWidth = 960;
        int setHeight = 540;

        Screen.SetResolution(setWidth, setHeight, true);
    }

    private void InitGame()
    {
        gameDataManager.Init();
        /*gameDataManager.AddGold(999);
        gameDataManager.AddPoint(999);
        GameCreator.Inventory.InventoryManager.Instance.AddItemToInventory(25686138, 500);
        GameCreator.Inventory.InventoryManager.Instance.AddItemToInventory(816283316, 500);*/
    }

    public void OnClick_ShowCraftSystem()
    {
        if(gameStateManager.CheckStateType(GameDefine.StateType.Build) == true)
        {
            return;
        }

        if (Core.Instance.uiPopUpManager.CheckIsView("UI_Portal_CatTower") == true)
        {
            return;
        }

        SoundManager.instance.PlaySound("NormalClick");
        gameStateManager.SetStateType(GameDefine.StateType.Build);
        cameraManager.ChangeCam_Craft();
        ui_System.gameObject.SetActive(true);
        ui_System.OnClick_StartCraftSystem();
    }

    #endregion
}
