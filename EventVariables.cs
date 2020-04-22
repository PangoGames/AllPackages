using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using GameAnalyticsSDK;
public class EventVariables : MonoBehaviour
{


    private void Start()
    {
        GameAnalytics.Initialize();

        if (!FB.IsInitialized)
        {
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            FB.ActivateApp();
        }


        /* Mandatory - set your AppsFlyer’s Developer key. */
        AppsFlyer.setAppsFlyerKey("1509259545");//Burayı değiştirmiyoruz
        /* For detailed logging */
        /* AppsFlyer.setIsDebug (true); */
        #if UNITY_IOS
        /* Mandatory - set your apple app ID
        NOTE: You should enter the number only and not the "ID" prefix */

        AppsFlyer.setAppID("1500867328"); //Buraya appstore id girilecek !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        AppsFlyer.getConversionData();
        AppsFlyer.trackAppLaunch();
        #elif UNITY_ANDROID
         /* For getting the conversion data in Android, you need to add the "AppsFlyerTrackerCallbacks" listener.*/
         AppsFlyer.init ("YOUR_APPSFLYER_DEV_KEY","AppsFlyerTrackerCallbacks");
        #endif


        //Start kısmına kod yazmayalım
        StartCoroutine(WaitInitialize());
    }

    public string version;
    IEnumerator WaitInitialize()
    {
        version = Application.version;

        yield return new WaitForSeconds(1);

       
        LevelStart();

        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, version, PlayerPrefs.GetInt("LevelCount").ToString(),/*Score*/ 0);
    }

    public void LevelStart()
    {
        FB.LogAppEvent(
            "LevelStart",
            1,
            new Dictionary<string, object>()
            {
                    { AppEventParameterName.Level, PlayerPrefs.GetInt("LevelCount")}
            });

        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "LevelStart", PlayerPrefs.GetInt("LevelCount").ToString(),/*Score*/ 1500);
    }

    public void LevelComplete()
    {
        FB.LogAppEvent(
            "LevelComplete",
            1,
            new Dictionary<string, object>()
            {
                { AppEventParameterName.Level, PlayerPrefs.GetInt("LevelCount")},
                { AppEventParameterName.Description, "Test"}

            });

        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "LevelComplete", PlayerPrefs.GetInt("LevelCount").ToString(),/*Score*/ 1500);
    }

    public void LevelFail()
    {
        FB.LogAppEvent(
            "LevelFail",
            1,
            new Dictionary<string, object>()
            {
                { AppEventParameterName.Level, PlayerPrefs.GetInt("LevelCount")},
                { AppEventParameterName.Description, "Test"}
            });

        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "LevelFail", PlayerPrefs.GetInt("LevelCount").ToString(),/*Score*/ 1500);
    }


    public static void RWStart(int giftValue)
    {
        FB.LogAppEvent(
            "RWStart",
            1,
            new Dictionary<string, object>()
            {
                { AppEventParameterName.Level, PlayerPrefs.GetInt("LevelCount")},
                { AppEventParameterName.Description, "Test"}
            });

        GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "RWStart", 0, PlayerPrefs.GetInt("LevelCount").ToString(), "Test");
    }

    public static void RWComplete(int giftValue)
    {
        FB.LogAppEvent(
            "RWComplete",
            1,
            new Dictionary<string, object>()
            {
                { AppEventParameterName.Level, PlayerPrefs.GetInt("LevelCount")},
                { AppEventParameterName.Description, giftValue}
            });

        GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "RWComplete", 0, PlayerPrefs.GetInt("LevelCount").ToString(), "Test");
    }


    public static void IntersititalTimer(int giftValue)
    {
        FB.LogAppEvent(
            "IntersititalTimer",
            1,
            new Dictionary<string, object>()
            {
                { AppEventParameterName.Level, giftValue}

            });

        GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "IntersititalTimer", 0, giftValue.ToString(), "Test");
    }


    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            Debug.Log("Failed to Initialize Facebook SDK.");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        Time.timeScale = !isGameShown ? 0 : 1;
    }

    string valueForXX;
    public void RemoteValueGA()
    {
        if (GameAnalytics.IsRemoteConfigsReady())
             valueForXX = GameAnalytics.GetRemoteConfigsValueAsString("VALUE", "1");
    }
}
