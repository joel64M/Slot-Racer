using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Unity.Advertisement.IosSupport.Components;
#if UNITY_IOS
using UnityEngine.iOS;
using Unity.Advertisement.IosSupport;
#endif
using UnityEngine.UI;
using UnityEngine.Analytics;
public class ConsentScreenScript : MonoBehaviour
{
  
    int userContent = -1;  //-1 is not set yet , 0 is yes, 1 is no

    [SerializeField] GameObject consentObject;
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject loadingTextGo;
    [SerializeField] GameObject acceptButton;
    [SerializeField] GameObject nextButton;
 

    private void Awake()
    {
        canvas.enabled = false;
    }
    private void Start()
    {
        userContent = PlayerPrefs.GetInt("UserConsent", -1);

        #if UNITY_IOS
                var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
        Version currentVersion = new Version(Device.systemVersion);
                Version ios14 = new Version("14.5");
                Debug.Log("current Version ***" + Device.systemVersion);
                Debug.Log("OS Version ***" + SystemInfo.operatingSystem);
                 Debug.Log("*** userConsent=" + userContent);
                if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED && currentVersion >= ios14)
                {
                    Debug.Log("IOS>14.5 *** ");
                     //  var contextScreen = Instantiate(contextScreenPrefab).GetComponent<ContextScreenView>();
                     //after the Continue button is pressed, and the tracking request
                     //        has been sent, automatically destroy the popup to conserve memory
                     //       contextScreen.sentTrackingAuthorizationRequest += () => Destroy(contextScreen.gameObject);
                    nextButton.SetActive(true);
                    acceptButton.SetActive(false);
                    canvas.enabled = true;
                }
                else
                {
                    Debug.Log("IOS<14.5 *** ");
                    nextButton.SetActive(false);
                    acceptButton.SetActive(true);

                    if (userContent == -1)
                    {
                        Debug.Log("consent = - 1  *** ");
                        canvas.enabled = true;
                        loadingTextGo.SetActive(false);
                        consentObject.SetActive(true);
                    }
                    else if (userContent == 0)
                    {
                        Debug.Log("consent = 0 *** ");
                        consentObject.SetActive(false);
                        LoadNextLevel();
                    }
                }
#else
                   nextButton.SetActive(false);
                    acceptButton.SetActive(true);

                    if (userContent == -1)
                    {
                        Debug.Log("consent = - 1  *** ");
                        canvas.enabled = true;
                        loadingTextGo.SetActive(false);
                        consentObject.SetActive(true);
                    }
                    else if (userContent == 0)
                    {
                        Debug.Log("consent = 0 *** ");
                        consentObject.SetActive(false);
                        LoadNextLevel();
                    }
#endif
    }


    public void _AcceptButton()
    {
        Debug.Log("Accept Button clicked *** ");
        Debug.Log(AnalyticsResult.AnalyticsDisabled);
        Debug.Log(AnalyticsResult.InvalidData);
        Debug.Log(AnalyticsResult.NotInitialized);
        Analytics.CustomEvent("UserConsent", new Dictionary<string, object>
                {
                   {"Accept ? ", true}

                });
        Debug.Log(AnalyticsResult.AnalyticsDisabled);
        Debug.Log(AnalyticsResult.InvalidData);
        Debug.Log(AnalyticsResult.NotInitialized);

        PlayerPrefs.SetInt("UserConsent", 0);
        LoadNextLevel();
    }

    public void _NextButton()
    {
        Debug.Log("Next Button clicked *** ");
        //PlayerPrefs.SetInt("UserConsent", 0);
        canvas.enabled = false;
#if UNITY_IOS
        ATTrackingStatusBinding.RequestAuthorizationTracking(AuthorizationTrackingReceived);
#endif

    }

    private void AuthorizationTrackingReceived(int status)
    {
        Debug.LogFormat("*** Tracking status received: {0}", status);

        if (status == 3)
        {
            PlayerPrefs.SetInt("UserConsent", 0);  //accept

                Analytics.CustomEvent("UserConsent", new Dictionary<string, object>
                {
                   {"Accept ATT ? ", true}

                });

        }
        else if (status == 2)
        {
            PlayerPrefs.SetInt("UserConsent", 1);  //reject
            Analytics.CustomEvent("UserConsent", new Dictionary<string, object>
            {
               {"Accept ATT ? ", false}

            });
        }

        LoadNextLevel();

    }


    public void _PrivacyPolicy()
    {
        Application.OpenURL("https://joel64fernandes.wixsite.com/joel64/privacy-policy");
    }

    void LoadNextLevel()
    {
        if (PlayerPrefs.GetInt("LEVELS", 1) != SceneManager.GetActiveScene().buildIndex)
        {
            if (Application.CanStreamedLevelBeLoaded(PlayerPrefs.GetInt("LEVELS", 1)))
            {
                //SceneManager.LoadScene(PlayerPrefs.GetInt("LEVELS", 1));
                SceneTransitionScript.instance.StartransitionTo(PlayerPrefs.GetInt("LEVELS", 1));
            }
            else
            {
                //SceneManager.LoadScene("Final");
                SceneTransitionScript.instance.StartransitionTo("Final");
            }
        }
    }

}
