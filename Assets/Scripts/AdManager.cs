using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
public class AdManager : MonoBehaviour, IUnityAdsListener 
{

    [SerializeField] string _iOSGameId;
    [SerializeField] string _androidGameId;

    [SerializeField] bool _testMode = false;
    [SerializeField] private string _gameId;


    [SerializeField] string _androidAdUnitId = "Interstitial_Android";
    [SerializeField] string _iOsAdUnitId = "Interstitial_iOS";
    [SerializeField] string _adInterstitialUnitId;

    [SerializeField] string _androidRewardAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSRewardAdUnitId = "Rewarded_iOS";
    [SerializeField] string _adRewardUnitId = null; // This will remain null for unsupported platforms


    [SerializeField] string _androidBannerAdUnitId = "Banner_Android";
    [SerializeField] string _iOSBannerAdUnitId = "Banner_iOS";
    string _adBannerUnitId = null; // This will remain null for unsupported platforms.


    //[SerializeField] Button rewardedAdBtn;

    public static AdManager instance;

    public Action RewardedVideoWatched;

    [SerializeField] int loadInterstitialAdAfter = 2;
    [SerializeField] int currentAdLoad = 0;
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (instance!=null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        InitializeAds();
    }
 
    void OnSceneLoaded(Scene scene, LoadSceneMode load)
    {
        //do stuff
        //if (SceneManager.GetActiveScene().buildIndex!=0)
        //{
            //try
            //{
            //    rewardedAdBtn = FindObjectOfType<UiManager>().rewardButton.GetComponent<Button>();
            //    rewardedAdBtn.interactable = true;
            //    rewardedAdBtn.onClick.AddListener(ShowRewardedAd);
            //}
            //catch (System.Exception e)
            //{
            //    Debug.Log("button error :" + e);
            //}
        Debug.Log("scene load ***");
        if (PlayerPrefs.HasKey("ADS")==false)
        {

            if (SceneManager.GetActiveScene().buildIndex > 0)
            {
            Debug.Log("Haskey Ads  *** "+PlayerPrefs.HasKey("ADS"));
                currentAdLoad++;
                if (currentAdLoad >= loadInterstitialAdAfter)
                {
                    currentAdLoad = 0;
                    ShowInterstitialAd();
                }
            }
        }

    }

    void InitializeAds()
    {
    #if UNITY_IOS
            _gameId = _iOSGameId;
    #else
     _gameId = _androidGameId;
    #endif

        Advertisement.Initialize(_gameId, _testMode, this);

        Advertisement.AddListener(this);

        //intestial video ad init
#if UNITY_IOS
            _adInterstitialUnitId = _iOsAdUnitId;
#else
        _adInterstitialUnitId = _androidAdUnitId;
    #endif
        //rewarded video init
    #if UNITY_IOS
            _adRewardUnitId = _iOSRewardAdUnitId;
    #else
                     _adRewardUnitId = _androidRewardAdUnitId;
#endif
        //banner
#if UNITY_IOS
            _adBannerUnitId = _iOSBannerAdUnitId;
#elif UNITY_ANDROID
        _adBannerUnitId = _androidBannerAdUnitId;
    #endif

        Advertisement.Load(_adInterstitialUnitId);
        Advertisement.Load(_adRewardUnitId);

        //Invoke("ShowRewardedAd", 2f);
    }

     void ShowInterstitialAd()
    {
        // Note that if the ad content wasn't previously loaded, this method will fail
        if (Advertisement.IsReady(_adInterstitialUnitId))
        {
          Advertisement.Show(_adInterstitialUnitId);
        }
        else
        {
            Advertisement.Load(_adInterstitialUnitId);
        }
    }
    //public REWARD rewardType = REWARD.none;

    public void ShowRewardedAd()
    {

        //rewardType = reward;
        // Disable the button: 
        //_showAdButton.interactable = false;
        // Then show the ad:
        //Debug.Log("Show Ad");
        if (Advertisement.IsReady(_adRewardUnitId))
            Advertisement.Show(_adRewardUnitId);
        //if(rewardedAdBtn)
        //  rewardedAdBtn.interactable = false;
    }

    public void ShowBannerAd()
    {

        Debug.Log("Show banner");
        if (Advertisement.IsReady(_adBannerUnitId))
        {
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_LEFT);
            Advertisement.Banner.Show(_adBannerUnitId);
        }
        else
        {
            StartCoroutine(RepeatShowBanner());
        }
    }

    IEnumerator RepeatShowBanner()
    {
        Debug.Log("repeat banner");
        Advertisement.Banner.Load(_adBannerUnitId);

        yield return new WaitForSeconds(1);
        ShowBannerAd();
    }
    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log("AD Ready : " + placementId);
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log("AdError : " + message);
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("AD Start : " + placementId);
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if(placementId==_adRewardUnitId && showResult == ShowResult.Finished)
        {
            Debug.Log("rewarded ad watched *** ");
            //reward 
            RewardedVideoWatched.Invoke();
            //rewardType = REWARD.none;
            //if (rewardedAdBtn)
            //{
            //    rewardedAdBtn.interactable = false;
            //    rewardedAdBtn.gameObject.SetActive(false);
            //}
        }
        if (placementId == _adRewardUnitId)
        {
            Debug.Log("rewarded ad *** " + showResult);
            Advertisement.Load(_adRewardUnitId);
       
        }

        if (placementId==_adInterstitialUnitId && showResult == ShowResult.Finished)
        {
            Debug.Log("interstial ad watched ***");
        }
        if (placementId == _adInterstitialUnitId)
        {
            Advertisement.Load(_adInterstitialUnitId);
            Debug.Log("interstial ad *** " +showResult);
        }
        if (placementId == _adBannerUnitId)
        {
            Advertisement.Load(_adInterstitialUnitId);
            Debug.Log("banner ad *** " + showResult);
        }
    }
}