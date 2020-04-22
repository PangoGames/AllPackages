using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SdkState { Integration, Test, Release }



public class AdsManager_A : MonoBehaviour {

    public string currentRw = "";
    public static AdsManager_A Instance { get; private set; }

    [SerializeField]
    private string sdkKey = "RLKKzOvbhqsDoAubshkfnIr_dJBMHUporkIZ0L8zRBAe9erkaTeuea7TOrDmeAzQ6fRTii-HObaOnATi8RDOJ7";
    [SerializeField] private SdkState sdkState = SdkState.Integration;

    // MAX SDK 
    string interstitialAdUnitId = "1471327460932040";
    string rewardedAdUnitId = "1c9faf240a40f6bc";
    string bannerAdUnitId = "e90a184f5f1ca87d";

    float RemoteDelayTime;
    float RemoteMinLevel;

    public float adsTimer;

    public void LetsGo()
    {
       

        RemoteDelayTime = 3000;
        RemoteMinLevel = 0;

        DontDestroyOnLoad(this.gameObject);

        Debug.Log("AdsManager_A is ready.");
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
        {
            if (sdkState == SdkState.Integration) return;
            // AppLovin SDK is initialized, start loading ads
            InitializeRewardedAds();

            InitializeInterstitialAds();
            InitializeBannerAds();


            if (sdkState == SdkState.Test)
            {
                MaxSdk.ShowMediationDebugger();
            }
        };

        if (Application.internetReachability != NetworkReachability.NotReachable)
        {


            MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration config) => {
                // Get value of a variable to use variableValue to alter your business logic
                var interstitial_timer = MaxSdk.VariableService.GetString("interstitial_timer", "0");
                //RemoteDelayTime = int.Parse(interstitial_delay);
                RemoteDelayTime = Convert.ToInt32(interstitial_timer);
                GameConst.kacsaniyedeReklam = (int)RemoteDelayTime;
                Debug.Log("interstitial_delay" + RemoteDelayTime);

                var min_level = MaxSdk.VariableService.GetString("min_level_interstitial", "0");
                //  RemoteMinLevel = int.Parse(min_level);
                RemoteMinLevel = Convert.ToInt32(min_level);
                Debug.Log("min_level" + RemoteMinLevel);


                var character_prize = MaxSdk.VariableService.GetString("skin_popup_freq", "0");
                var character = Convert.ToInt32(character_prize);
                SlugAnalytics.ABTestCharacter(Convert.ToInt32(character_prize));
                SlugAnalytics.ABTestAds((int)RemoteDelayTime);

                GameConst.kacOyundaKarakter = Convert.ToInt32(character_prize);

                //GameObject.Find("TestText").GetComponent<Text>().text = RemoteDelayTime + "_" + RemoteMinLevel + "_" + character;
            };
        }

        MaxSdk.SetSdkKey(sdkKey);
        MaxSdk.InitializeSdk();

    }
    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);

        }
        else
        {
            Instance = this;

        }
        LetsGo();




    }

    public void Getir()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {


            MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration config) => {
                // Get value of a variable to use variableValue to alter your business logic
                var interstitial_timer = MaxSdk.VariableService.GetString("interstitial_timer", "0");
                //RemoteDelayTime = int.Parse(interstitial_delay);
                RemoteDelayTime = Convert.ToInt32(interstitial_timer);
                GameConst.kacsaniyedeReklam = (int)RemoteDelayTime;
                Debug.Log("interstitial_delay" + RemoteDelayTime);

                var min_level = MaxSdk.VariableService.GetString("min_level_interstitial", "0");
                //  RemoteMinLevel = int.Parse(min_level);
                RemoteMinLevel = Convert.ToInt32(min_level);
                Debug.Log("min_level" + RemoteMinLevel);
                GameConst.KacLeveldenSonraReklam = (int)RemoteMinLevel;

                var character_prize = MaxSdk.VariableService.GetString("skin_popup_freq", "0");
                var character = Convert.ToInt32(character_prize);
                SlugAnalytics.ABTestCharacter(Convert.ToInt32(character_prize));
                SlugAnalytics.ABTestAds((int)RemoteDelayTime);
                GameConst.kacOyundaKarakter = Convert.ToInt32(character_prize);

                //GameObject.Find("TestText").GetComponent<Text>().text = RemoteDelayTime + "_" + RemoteMinLevel + "_" + character;
            };





        }
    }

    public void Update()
    {
        adsTimer += Time.deltaTime;
        if (RemoteDelayTime == 0)
        {


        }
    }


    public void InitializeBannerAds()
    {
        
        MaxSdk.CreateBanner(bannerAdUnitId, MaxSdkBase.BannerPosition.TopCenter);
        // Set background or background color for banners to be fully functional
        MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, new Color(0, 0, 0, 0));
        
    }

    public void InitializeInterstitialAds()
    {
        
        // Attach callback
        MaxSdkCallbacks.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.OnInterstitialLoadFailedEvent += OnInterstitialFailedEvent;
        MaxSdkCallbacks.OnInterstitialAdFailedToDisplayEvent += InterstitialFailedToDisplayEvent;
        MaxSdkCallbacks.OnInterstitialDisplayedEvent += InterstitialDisplayed;
        MaxSdkCallbacks.OnInterstitialHiddenEvent += OnInterstitialDismissedEvent;

        // Load the first interstitial
        LoadInterstitial();
        
    }

    private void InterstitialDisplayed(string obj)
    {
        //GameManager.gameManager.reklamGosteriliyor=true;
    }

    private void LoadInterstitial()
    {
        
        MaxSdk.LoadInterstitial(interstitialAdUnitId);
    }

    private void OnInterstitialLoadedEvent(string adUnitId)
    {
        // Interstitial ad is ready to be shown. MaxSdk.IsInterstitialReady(interstitialAdUnitId) will now return 'true'
    }

    private void OnInterstitialFailedEvent(string adUnitId, int errorCode)
    {
        
        // Interstitial ad failed to load. We recommend re-trying in 2 seconds.
        Invoke("LoadInterstitial", 2);
        
    }

    private void InterstitialFailedToDisplayEvent(string adUnitId, int errorCode)
    {
        
        // Interstitial ad failed to display. We recommend loading the next ad
        LoadInterstitial();
        
    }

    private void OnInterstitialDismissedEvent(string adUnitId)
    {
        SlugAnalytics.OnShowinterstitial();
       // GameManager.gameManager.reklamGosteriliyor = false;
       // StartCoroutine(GameManager.gameManager.GetPrizePlayer());
        //SceneManager.LoadScene(0);
        // Interstitial ad is hidden. Pre-load the next ad
        // Setting delegates and load again
        //InitializeInterstitialAds();
        LoadInterstitial();
        
    }



    public void InitializeRewardedAds()
    {
        
        // Attach callback
        MaxSdkCallbacks.OnRewardedAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.OnRewardedAdLoadFailedEvent += OnRewardedAdFailedEvent;
        MaxSdkCallbacks.OnRewardedAdFailedToDisplayEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.OnRewardedAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.OnRewardedAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.OnRewardedAdHiddenEvent += OnRewardedAdDismissedEvent;
        MaxSdkCallbacks.OnRewardedAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        // Load the first RewardedAd
        LoadRewardedAd();
       
    }

    private void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(rewardedAdUnitId);
    }

    private void OnRewardedAdLoadedEvent(string adUnitId)
    {
        // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(rewardedAdUnitId) will now return 'true'
    }

    private void OnRewardedAdFailedEvent(string adUnitId, int errorCode)
    {
        
        // Rewarded ad failed to load. We recommend re-trying in 2 seconds.
        Invoke("LoadRewardedAd", 2);
        
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, int errorCode)
    {
        
        // Rewarded ad failed to display. We recommend loading the next ad
        //rewardCallingObject.SendMessage(rewardVideoFailMessage);
        LoadRewardedAd();
        
    }


    private void OnRewardedAdDisplayedEvent(string adUnitId)
    {
        LoadRewardedAd();

        if (currentRw == "upgradeSpeed")
        {
            UpgradeManager.upgradeManager.UpgradeSpeedAfterRW();
            SlugAnalytics.OnRWComplete("speedUpgrade");
        }
        else if (currentRw == "upgradePower")
        {
            UpgradeManager.upgradeManager.UpgradePowerAfterRW();
            SlugAnalytics.OnRWComplete("powerUpgrade");
        }
        else if (currentRw == "upgradeHealt")
        {
            UpgradeManager.upgradeManager.UpgradeHealtAfterRW();
            SlugAnalytics.OnRWComplete("healthUpgrade");
        }
        else if (currentRw == "3xCoin")
        {
            if (TotalKazancController.totalKazancController != null)
                TotalKazancController.totalKazancController.AfterRW3X();

            SlugAnalytics.OnRWComplete("3xCoin");
        }
        else if (currentRw == "OyunSonrasiCanReklam")
        {
            GameConst.ShowRwAfterGame = 0;
            SlugAnalytics.OnRWComplete("ExtraHealth");
        }
        else if (currentRw == "prizePlayer")
        {
            GameManager.gameManager.CompletePrizePlayer();
            SlugAnalytics.OnRWComplete("unlockPlayer");
        }

        currentRw = "";

    }

    private void OnRewardedAdClickedEvent(string adUnitId) { }

    private void OnRewardedAdDismissedEvent(string adUnitId)
    {

        // Rewarded ad is hidden. Pre-load the next ad
        //OnRewardedAdReceivedRewardEvent currently is not works for all networks so give prize here
        //rewardCallingObject.SendMessage(rewardVideoCompleteMessage);
       

        LoadRewardedAd();
        
    }

    public delegate void ReceiveReward();
    public static event ReceiveReward OnReceiveReward;

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward)
    {


       


        // Rewarded ad was displayed and user should receive the reward
        // rewardCallingObject.SendMessage(rewardVideoCompleteMessage);
        OnReceiveReward?.Invoke();
    }

    public void InitializeAndLoadBanner()
    {
        
        InitializeBannerAds();
        
    }
    public void ShowBanner()
    {
        
        MaxSdk.ShowBanner(bannerAdUnitId);
        
    }

    public void HideBanner()
    {
        
        MaxSdk.HideBanner(bannerAdUnitId);
        
    }

    public bool IsIntersitialReady()
    {
        
        return MaxSdk.IsInterstitialReady(interstitialAdUnitId);
        
    }
    
    public void LoadIntersitial()
    {
        
        MaxSdk.LoadInterstitial(interstitialAdUnitId);
        
    }

    IEnumerator AfterIntersitial()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
    }
    private IEnumerator coroutine;
    public void ShowIntersitial()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        if (IsIntersitialReady())
        {
                 MaxSdk.ShowInterstitial(interstitialAdUnitId);
                    //StartCoroutine(AfterIntersitial());
        }
        else
        {
            Debug.Log("Interstitial is not ready.");
            coroutine = CheckInterstitial(); 
            StartCoroutine(coroutine);
           // SceneManager.LoadScene(0);
        }
    }

    private IEnumerator CheckInterstitial()
    {
        while (true)
        {
            MaxSdk.LoadInterstitial(interstitialAdUnitId);
            yield return new WaitForSeconds(2f);
            if (MaxSdk.IsInterstitialReady(interstitialAdUnitId))
            {
                break;
            }
        }
    }

    public void ShowRewarded( string _currentRw)
    {
        if (_currentRw == "upgradeSpeed")
        {
            SlugAnalytics.OnRWStart("speedUpgrade");
             
        }
        else if (_currentRw == "upgradePower")
        {
            SlugAnalytics.OnRWStart("powerUpgrade");
             
        }
        else if (_currentRw == "upgradeHealt")
        {
            SlugAnalytics.OnRWStart("healthUpgrade");

        }
        else if (_currentRw == "3xCoin")
        {
            SlugAnalytics.OnRWStart("3xCoin");
             
        }
        else if (_currentRw == "OyunSonrasiCanReklam")
        {

            SlugAnalytics.OnRWStart("ExtraHealth");
        }
        else if (_currentRw == "prizePlayer")
        {

            SlugAnalytics.OnRWStart("unlockPlayer");
        }
       
        currentRw = _currentRw;
        
        if (MaxSdk.IsRewardedAdReady(rewardedAdUnitId))
        {

            MaxSdk.ShowRewardedAd(rewardedAdUnitId);
        }
        else
        {
            Debug.Log("Rewarded is not ready.");
            MaxSdk.LoadRewardedAd(rewardedAdUnitId);
        }
    }

    public bool IsRewardedAdReady()
    {
       
       return MaxSdk.IsRewardedAdReady(rewardedAdUnitId);
      
    }
    
}
