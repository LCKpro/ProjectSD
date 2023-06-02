using System;
using UnityEngine;

public partial class Core : MonoBehaviour
{
    #region SingleTon

    private static Core _instance = null;

    public static Core Instance
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
            GameObject go = GameObject.Find("Core");
            if (go != null)
            {
                _instance = go.GetComponent<Core>();
                if (_instance == null)
                {
                    Debug.Log("Core Instance Null");
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

    #region Common

    public GameSceneManager gameSceneManager;
    public BuildingManager buildingManager;
    public ItemManager itemManager;
    public UIPopUpManager uiPopUpManager;
    public AudioManager audioManager;

    /*public RootCheckManager rootCheckManager;
    public AppLovinMaxManager appLovinMaxManager;
    public HashCodeManager hashCodeManager;
    public AchievementManager achievementManager;
    public GoogleAchievement googleAchievement;
    public AdsMediation adsMediation;
    public AdmobManager admobManager;
    public NetworkManager networkManager;
    public UserSecureDataManager userSecureDataManager;
    public InstantDeviceID instantDeviceID;
    public NetworkDataList networkDataList;
    public AppSwitch appSwitch;
    public StaticSet staticSet;
    public LocalizationDataManager localizationDataManager;
    public TextMeshFontAssetManager textMeshFontAssetManager;
    public IRVManager iRVManager;
    public GameAssetBundleManager gameAssetBundleManager;
    public AddressableAssetManager addressableAssetManager;
    public StartDownloadManager startDownloadManager;
    public CookAppsIOSManager cookAppsIOSManager;
    public SessionManager sessionManager;
    public NetworkError networkError;
    public NetworkLoadingUI networkLoadingUI;
    public FacebookManager facebookManager;
    public GooglePlayManager googlePlayManager;

    public GameCenterManager gameCenterManager;
    public AppleLoginManager appleLoginManager;


    #region InAppManager Part

    *//*private InAppManager _inappManager = null;

    public InAppManager inappManager
    {
        get
        {
            if (_inappManager == null)
            {
                _inappManager = GameObject.Find("InAppManager").GetComponent<InAppManager>();
            }

            return _inappManager;
        }
    }*//*

    #endregion

    public TutorialManager tutorialManager;
    public GameSoundManager gameSoundManager;
    public LocalNotificationManager localNotificationManager;
    public ApplicationLifecycletManager applicationLifecycletManager;
    public GDEDataSubManager gDEDataSubManager;
    public FCMManager fcmManager;
    public BackKeyManager backKeyManager;
    public ZFManager zfManager;
    public FixedStatusManager fixedStatusManager;
    public SupporterDataManager supporterDataManager;
    public TemporaryStorageManager temporaryStorageManager;

    // Only use Main Core
    // Uses UI that penetrates all scenes only
    // Prevent memory overuse with excessive pop-up usage!!
    public UIPopUpManager uiPopUpManager;
    public UIEffectManager uiEffectManager;

    public ChattingManager chattingManager;
    public TicketManager ticketManager;
    public InGamePools inGamePools;

    public AutoTestManager autoTestManager;

    #endregion

    #region GamePlay Logic

    public GameDataManager gameDataManager;

    public ItemDropMemoryTrigger itemDropMemoryTrigger;
    public ShotSelectManager shotSelectManager;*/

    #endregion

    /// <summary>
    /// Init Logic
    /// </summary>
    public void Init()
    {
        SetInstance();
    }


    [NonSerialized] public bool managedStartCompleted = false;

    // Init after Start
    public void ManagedStart(int part)
    {
        Debug.Log($"Core / ManagedStart() / Part {part} / {Time.realtimeSinceStartup}");

        if (part == 0)
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep; // Don't turn off phone

            //localNotificationManager.Init();
        }
        else if (part == 1)
        {
            //inappManager.Init();
        }
        else if (part == 2)
        {
            //ES3.Init();
        }
        else if (part == 3)
        {
            /*staticSet.SetDatas();
            gameDataManager.Init();
            userSecureDataManager.Init();
            instantDeviceID.Init();
            networkManager.Init();
            // gameAssetBundleManager.Init();
            // gameAssetBundleManager.Initialize();
            // admobManager.InitRewardVideo();
            // adsMediation.Init();
            appLovinMaxManager.Init();
            // achievementManager.Init();
            // googleAchievement.Init();
            chattingManager.Init();

            // Play BGM
            gameSoundManager.Play_BGM(GameSoundManager.BGM.UIBGM);*/
        }
        else if (part == 4)
        {
            // User Selected Language
            /*SystemLanguage? userLanguage = userSecureDataManager.GetUserLanguage();
            if (userLanguage != null)
            {
                localizationDataManager.language = userLanguage.Value;
                localizationDataManager.FilterLanguage();
            }

            // Addressable Asset Init
            addressableAssetManager.Initialize();

            // TextMesh FontAsset Init
            textMeshFontAssetManager.Initialize();*/
        }
        else if (part == 5)
        {
            /*// Asset Download Require
            startDownloadManager.Initialize();

            // Complete Trigger. Must located on Bottom
            managedStartCompleted = true;*/
        }
    }
}