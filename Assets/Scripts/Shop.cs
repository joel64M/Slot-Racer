using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
public class Shop : MonoBehaviour
{
    [Space(5)]
    [Header("ASSIGN")]
    //[SerializeField] float shopTimer = 0.5f;
    [SerializeField] ShopSo shopSo;
    [SerializeField] Transform characterShopParent;
    [SerializeField] Transform characterGameParent;
    [SerializeField] GameObject itemPrefab;

    [Space(5)]
    [Header("DEFAULT")]
    [SerializeField] Transform itemParent;
    [SerializeField] GameObject buyButtonGo;
    [SerializeField] TextMeshProUGUI buyButtonPriceTxt;
    [SerializeField] GameObject shopCanvasGo;
    [SerializeField] GameObject outpanel;
    [SerializeField] GameObject coinPanel;
    [SerializeField] TextMeshProUGUI coinText;

    [Space(5)]
    [Header("PRIVATE")]
    //[SerializeField] ShopItem currentShopItem;  //current in selection 
    //[SerializeField] ShopItem tempShopItem; //main shop item
    //[SerializeField] Image currentHighlightImg;
    [SerializeField] Dictionary<ShopItemSo, GameObject> shopCharacterDict = new Dictionary<ShopItemSo, GameObject>();
    [SerializeField] Dictionary<ShopItemSo, GameObject> gameCharacterDict = new Dictionary<ShopItemSo, GameObject>();

    //[SerializeField] GameObject currentActivatedPrefab;

    //Action
    public Action shopClosed;
    public Action shopOpened;

    private void Awake()
    {
        //if (characterParent == null)
        //    characterParent = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
        PlayerPrefs.GetInt("COINS", 100);
        //InitializeShop();
        InitShop();
    }

    void Start()
    {
        UpdateCoinUi();
    }

    //public void HighlightItem(ShopItem item)
    //{
    //    if (item == currentShopItem) return;

    //    if (PlayerPrefs.GetInt(item.ReturnShopItemSo().nameString + "UNLOCKED", 0) == 1)
    //    {
    //        buyButtonGo.SetActive(false);
    //        PlayerPrefs.SetInt(currentShopItem.ReturnShopItemSo().nameString + "SELECTED", 0);
    //        PlayerPrefs.SetInt(item.ReturnShopItemSo().nameString + "SELECTED", 1);
    //        tempShopItem = item;
    //    }
    //    else
    //    {
    //        buyButtonGo.SetActive(true);
    //    }
    //    currentShopItem = item;
    //    HighlightImg(item);
    //    EnableCharacter(item);
    //}


    public void SelectItem(ShopItem item)
    {
        //if (item == currentActivatedShopItem) return;

        if (PlayerPrefs.GetInt(item.shopItemSo.nameString + "UNLOCKED", 0) == 1)
        {
            buyButtonGo.SetActive(false);
            PlayerPrefs.SetInt(currentActivatedShopItem.shopItemSo.nameString + "SELECTED", 0);
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
    public void Hide(GameObject go)
    {
        go.SetActive(false);
    }
    public void Show(GameObject go)
    {
        go.SetActive(true);
    }
    public void _CloseShop()
    {

        currentActivatedShopItem.UnHighlightShopItem();
        currentSelectedShopItem.HighlightShopItem();
        //if(currentActivatedShopItem==tempShopItem)
        //HighlightImg(tempShopItem);
        //currentShopItem = tempShopItem;

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
            //tempShopItem = currentShopItem;
            //currentActivatedShopItem = tempShopItem;
            //tempShopItem = currentActivatedShopItem;

            PlayerPrefs.SetInt(currentActivatedShopItem.shopItemSo.nameString + "UNLOCKED", 1);
            PlayerPrefs.SetInt(currentActivatedShopItem.shopItemSo.nameString + "SELECTED", 1);
            //currentShopItem.ReturnPriceTagGO().SetActive(false);
            buyButtonGo.SetActive(false);
        }
        //if (currentShopItem.ReturnShopItemSo().price < PlayerPrefs.GetInt("COINS",0))
        //{
        //    PlayerPrefs.SetInt("COINS", PlayerPrefs.GetInt("COINS") - currentShopItem.ReturnShopItemSo().price);
        //    UpdateCoinUi();
        //    PlayerPrefs.SetInt(tempShopItem.ReturnShopItemSo().nameString + "SELECTED", 0);
        //    tempShopItem = currentShopItem;

        //    PlayerPrefs.SetInt(currentShopItem.ReturnShopItemSo().nameString + "UNLOCKED", 1);
        //    PlayerPrefs.SetInt(currentShopItem.ReturnShopItemSo().nameString + "SELECTED", 1);
        //    currentShopItem.ReturnPriceTagGO().SetActive(false);
        //    buyButtonGo.SetActive(false);
        //}
    }


   [SerializeField] GameObject currentActivatedShopCharacterPrefab = null;
    [SerializeField] GameObject currentActivatedGameCharacterPrefab = null;


    [SerializeField] ShopItem currentActivatedShopItem = null;
    [SerializeField] ShopItem currentSelectedShopItem = null;

    public void DDD()
    {
        Debug.Log("jjj");
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
                //HighlightImg(currentShopItem);
                currentShopItem.SelectShopItem();
                isSelectedFound = true;
                currentActivatedShopItem =currentSelectedShopItem= currentShopItem;
                Debug.Log(itemSo.nameString);
                //currentShopItem = tempShopItem = currentShopItemTemp;
                currentActivatedShopCharacterPrefab = character;
                currentActivatedGameCharacterPrefab = characterGame;
                currentActivatedShopCharacterPrefab.SetActive(true);
                currentActivatedGameCharacterPrefab.SetActive(true);


            }
            else
            {
                //currentShopItem.ReturnHighlightedImg().enabled = false;
                currentShopItem.UnSelectShopItem();
            }

            //if (PlayerPrefs.GetInt(itemSo.nameString + "UNLOCKED", 0) == 1)
            //{
            //    currentShopItem.ReturnPriceTagGO().SetActive(false);
            //}

        }
        buyButtonGo.SetActive(false);

        if (!isSelectedFound)
        {
            //PlayerPrefs.SetInt(defaultShopItem.nameString + "SELECTED", 1);
            defaultShopItem.SelectShopItem();
            //HighlightImg(defaultShopItem);
            currentActivatedShopItem =currentSelectedShopItem= defaultShopItem;
            currentActivatedShopCharacterPrefab = defaultCharacterShopPrefab;
            currentActivatedGameCharacterPrefab = defaultCharacterGamePrefab;
            currentActivatedShopCharacterPrefab.SetActive(true);
            currentActivatedGameCharacterPrefab.SetActive(true);
            //defaultCharacter.SetActive(true);
            //currentActivatedPrefab = defaultCharacter;
            //currentShopItem = tempShopItem = defaultShopItem;
            buyButtonGo.SetActive(false);
        }

    }

    /*
    void InitializeShop()
    {
        bool isFoundDefault = false;
        ShopItem defaultShopItem = null;
        GameObject defaultCharacter = null;

        foreach (var item in shopSo.shopItems)
        {
            GameObject go = Instantiate(itemPrefab, itemParent);
            go.name = item.nameString;

            GameObject goo = Instantiate(item.prefab, characterParent);
            dict.Add(item, goo);
            goo.SetActive(false);

            if (item.isDefaultSelected && !defaultCharacter)
            {
                defaultShopItem = go.GetComponent<ShopItem>().Init(item, this);
                defaultCharacter = goo;
            }
         
            ShopItem currentShopItemTemp = go.GetComponent<ShopItem>().Init(item, this);
            if (item.isDefaultSelected)
            {
                PlayerPrefs.SetInt(currentShopItemTemp.ReturnShopItemSo().nameString + "UNLOCKED", 1);
                currentShopItemTemp.ReturnPriceTagGO().SetActive(false);

            }
            if (PlayerPrefs.GetInt(item.nameString + "SELECTED", 0) == 1)
            {
                HighlightImg(currentShopItemTemp);
                isFoundDefault = true;
                currentShopItem = tempShopItem = currentShopItemTemp;

                currentActivatedPrefab = goo;
                goo.SetActive(true);
            }
            else
            {
                currentShopItemTemp.ReturnHighlightedImg().enabled = false;
            }

            if (PlayerPrefs.GetInt(item.nameString + "UNLOCKED", 0) == 1)
            {
                currentShopItemTemp.ReturnPriceTagGO().SetActive(false);
            }

            buyButtonGo.SetActive(false);
        }

        if (!isFoundDefault)
        {
            PlayerPrefs.SetInt(defaultShopItem.ReturnShopItemSo().nameString + "SELECTED", 1);
            HighlightImg(defaultShopItem);
            defaultCharacter.SetActive(true);
            currentActivatedPrefab = defaultCharacter;
            currentShopItem = tempShopItem = defaultShopItem;
            buyButtonGo.SetActive(false);
        }
    }
  
    //void HighlightImg(ShopItem shopItem)
    //{
    //    if (currentHighlightImg)
    //        currentHighlightImg.enabled = false;
    //    currentHighlightImg = shopItem.ReturnHighlightedImg();
    //    currentHighlightImg.enabled = true;
    //}
    */
    void UpdateCoinUi()
    {
        coinText.text = PlayerPrefs.GetInt("COINS").ToString();
    }

    void EnableCharacter(ShopItem item)
    {
        currentActivatedShopCharacterPrefab.SetActive(false);
        currentActivatedShopCharacterPrefab = shopCharacterDict[item.shopItemSo];
        shopCharacterDict[item.shopItemSo].SetActive(true);

        currentActivatedGameCharacterPrefab.SetActive(false);
        currentActivatedGameCharacterPrefab = gameCharacterDict[item.shopItemSo];
        gameCharacterDict[item.shopItemSo].SetActive(true);
        //currentActivatedPrefab.SetActive(false);
        //currentActivatedPrefab = dict[item.ReturnShopItemSo()];
        //dict[item.ReturnShopItemSo()].SetActive(true);
    }
}
