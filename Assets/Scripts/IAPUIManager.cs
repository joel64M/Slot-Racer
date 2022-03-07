using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IAPUIManager : MonoBehaviour
{
    public static IAPUIManager instance;

    //[SerializeField] TextMeshProUGUI coinsTxt;
    //[SerializeField] int coins;

    //[SerializeField] TextMeshProUGUI localCoinTxt;
    [SerializeField] GameObject removeAdsButton;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }

    //private void Start()
    //{
    //    StartCoroutine(LoadPricesRoutine());
    //}

    //public void _BuyCoin100Button()
    //{
    //    IAPManager.instance.BuyCoin100();
    //}

    public void _BuyRemoveAdsButton()
    {
        IAPManager.instance.BuyRemoveAds();
    }

    public void _RestoreButton()
    {
        IAPManager.instance.RestorePurchases();
    }

    //called after successfully buying coins
    //public void AddCoins(int amount)
    //{
    //    coins += amount;
    //    coinsTxt.text = coins.ToString();
    //}

    //called after successfully buying remove ads
    public void RemoveAdsComplete()
    {
        PlayerPrefs.SetInt("ADS", 0);
        removeAdsButton.SetActive(false);
    }

    //IEnumerator LoadPricesRoutine()
    //{
    //    while (!IAPManager.instance.IsInitialized())
    //        yield return null;
    //    string loadPrice = "";
    //    loadPrice = IAPManager.instance.GetProductPriceFromStore(IAPManager.instance.COINS_100);
    //    localCoinTxt.text = loadPrice;
    //}
}
