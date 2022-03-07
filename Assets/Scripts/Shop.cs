using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using NameSpaceName;
public class Shop : MonoBehaviour
{
    [Space(5)]
    [Header("ASSIGN")]
    [SerializeField] ShopSo shopSo;
    [SerializeField] Transform characterShopParent;  //prefabs in shop
    [SerializeField] Transform characterGameParent;//prefas in the game
    [SerializeField] GameObject itemPrefab;

    [Space(5)]
    [Header("DEFAULT")]
    [SerializeField] Transform itemParent;
    [SerializeField] GameObject buyButtonGo;
    [SerializeField] TextMeshProUGUI buyButtonPriceTxt;
    [SerializeField] GameObject shopCanvasGo;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] GameObject rewardedVideoButton;

    [Space(5)]
    [Header("PRIVATE")]
    [SerializeField] Dictionary<ShopItemSo, GameObject> shopCharacterDict = new Dictionary<ShopItemSo, GameObject>();
    [SerializeField] Dictionary<ShopItemSo, GameObject> gameCharacterDict = new Dictionary<ShopItemSo, GameObject>();

    [SerializeField] GameObject currentActivatedShopCharacterPrefab = null;
    [SerializeField] GameObject currentActivatedGameCharacterPrefab = null;

    [SerializeField] ShopItem currentActivatedShopItem = null;   //current in selection 
    [SerializeField] ShopItem currentSelectedShopItem = null; //main shop item

    //Action
    public Action shopClosed;
    public Action shopOpened;

    private void Awake()
    {
        //if (characterParent == null)
        //    characterParent = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
        InitShop();
    }
    private void Start()
    {
        UpdateCoinUi();
    }

    private void OnEnable()
    {
            AdManager.instance.RewardedVideoWatched += RewardedVideoWatchd;
    }
    private void OnDisable()
    {
        AdManager.instance.RewardedVideoWatched -= RewardedVideoWatchd;
    }
    public void SelectItem(ShopItem item)
    {
        //if (item == currentActivatedShopItem) return;

        if (PlayerPrefs.GetInt(item.shopItemSo.nameString + "UNLOCKED", 0) == 1)
        {
            buyButtonGo.SetActive(false);
            PlayerPrefs.SetInt(currentSelectedShopItem.shopItemSo.nameString + "SELECTED", 0);
            PlayerPrefs.SetInt(item.shopItemSo.nameString + "SELECTED", 1);

            currentActivatedShopItem.UnHighlightShopItem();
            item.HighlightShopItem();
            currentActivatedShopItem = item;
            currentSelectedShopItem = item;

        }
        else
        {
            currentActivatedShopItem.UnHighlightShopItem();
            item.HighlightShopItem();
            currentActivatedShopItem = item;


            buyButtonGo.SetActive(true);
            buyButtonPriceTxt.text = item.shopItemSo.price.ToString();
        }
        //display character
    
        EnableCharacter(item);
    }

    void RewardedVideoWatchd()
    {
        rewardedVideoButton.SetActive(false);
        int coins = PlayerPrefs.GetInt("COINS") + 50;
        FindObjectOfType<GameManager>().coinCount = coins;
        FindObjectOfType<UiManager>().UpdateCoinTxt(coins);
        PlayerPrefs.SetInt("COINS",coins );
        coinText.text = coins.ToString();
    }


    public void Hide(GameObject go)
    {
        go.SetActive(false);
    }

    public void Show(GameObject go)
    {
        go.SetActive(true);
    }

    public void _RewardAd()
    {
        AdManager.instance.ShowRewardedAd();
    }

    public void _CloseShop()
    {
        currentActivatedShopItem.UnHighlightShopItem();
        currentSelectedShopItem.HighlightShopItem();


        currentActivatedShopItem = currentSelectedShopItem;
        EnableCharacter(currentSelectedShopItem);
        //LeanTween.moveY(outpanel.GetComponent<RectTransform>(), -1000f, shopTimer).setOnUpdate((float val) => {
        //    if (val > 0.9f)
        //    {
        //        shopCanvasGo.SetActive(false);
        //    }
        //});
        //LeanTween.moveY(coinPanel.GetComponent<RectTransform>(), 250f, shopTimer);
        shopClosed?.Invoke();
    }

    public void _OpenShop()
    {
        shopCanvasGo.SetActive(true);
        //LeanTween.moveY(outpanel.GetComponent<RectTransform>(),  0, shopTimer);
        //LeanTween.moveY(coinPanel.GetComponent<RectTransform>(), -250f, shopTimer);
        shopOpened?.Invoke();
    }

    public void BuyItem()
    {
        if (currentActivatedShopItem.shopItemSo.price < PlayerPrefs.GetInt("COINS", 0))
        {
            PlayerPrefs.SetInt("COINS", PlayerPrefs.GetInt("COINS") - currentActivatedShopItem.shopItemSo.price);
            UpdateCoinUi();
            PlayerPrefs.SetInt(currentSelectedShopItem.shopItemSo.nameString + "SELECTED", 0);
           currentSelectedShopItem = currentActivatedShopItem;
            PlayerPrefs.SetInt(currentActivatedShopItem.shopItemSo.nameString + "UNLOCKED", 1);
            PlayerPrefs.SetInt(currentActivatedShopItem.shopItemSo.nameString + "SELECTED", 1);
            buyButtonGo.SetActive(false);
        }
    }

    void InitShop()
    {
        //bool isDefaultFound=false;
        bool isSelectedFound = false;

        ShopItem defaultShopItem = null;
        GameObject defaultCharacterShopPrefab = null;
        GameObject defaultCharacterGamePrefab = null;
        foreach (var itemSo in shopSo.shopItems)
        {
            //shop item for content
            GameObject shopItemGo = Instantiate(itemPrefab, itemParent);
            shopItemGo.name = itemSo.nameString;

            //car model 
            GameObject character = Instantiate(itemSo.prefab, characterShopParent);
            GameObject characterGame = Instantiate(itemSo.prefab, characterGameParent);

            shopCharacterDict.Add(itemSo, character);
            character.SetActive(false);

            gameCharacterDict.Add(itemSo, characterGame);
            characterGame.SetActive(false);

            ShopItem currentShopItem = shopItemGo.GetComponent<ShopItem>().Init(itemSo, this);
            if (itemSo.isDefaultSelected)
            {
                defaultShopItem = currentShopItem;
                defaultCharacterShopPrefab = character;
                defaultCharacterGamePrefab = characterGame;
                PlayerPrefs.SetInt(itemSo.nameString + "UNLOCKED", 1);
            }

            if (PlayerPrefs.GetInt(itemSo.nameString + "SELECTED", 0) == 1)
            {
                currentShopItem.HighlightShopItem();
                PlayerPrefs.SetInt(currentShopItem.shopItemSo.nameString + "SELECTED", 1);
                isSelectedFound = true;
                currentActivatedShopItem =currentSelectedShopItem= currentShopItem;

                currentActivatedShopCharacterPrefab = character;
                currentActivatedGameCharacterPrefab = characterGame;
                currentActivatedShopCharacterPrefab.SetActive(true);
                currentActivatedGameCharacterPrefab.SetActive(true);
            }
            else
            {
                currentShopItem.UnHighlightShopItem();
                PlayerPrefs.SetInt(currentShopItem.shopItemSo.nameString + "SELECTED", 0);
            }
        }
        buyButtonGo.SetActive(false);

        if (!isSelectedFound)
        {
            defaultShopItem.HighlightShopItem();
            PlayerPrefs.SetInt(defaultShopItem.shopItemSo.nameString + "SELECTED", 1);

            currentActivatedShopItem =currentSelectedShopItem= defaultShopItem;

            currentActivatedShopCharacterPrefab = defaultCharacterShopPrefab;
            currentActivatedGameCharacterPrefab = defaultCharacterGamePrefab;
            currentActivatedShopCharacterPrefab.SetActive(true);
            currentActivatedGameCharacterPrefab.SetActive(true);

            buyButtonGo.SetActive(false);
        }
    }
 
    void UpdateCoinUi()
    {
        coinText.text = PlayerPrefs.GetInt("COINS").ToString();
        FindObjectOfType<GameManager>().coinCount = PlayerPrefs.GetInt("COINS");
        FindObjectOfType<UiManager>().UpdateCoinTxt(PlayerPrefs.GetInt("COINS"));
    }

    void EnableCharacter(ShopItem item)
    {
        currentActivatedShopCharacterPrefab.SetActive(false);
        currentActivatedShopCharacterPrefab = shopCharacterDict[item.shopItemSo];
        shopCharacterDict[item.shopItemSo].SetActive(true);

        currentActivatedGameCharacterPrefab.SetActive(false);
        currentActivatedGameCharacterPrefab = gameCharacterDict[item.shopItemSo];
        gameCharacterDict[item.shopItemSo].SetActive(true);
    }
}
