using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopItem : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] Image img;
    [SerializeField] Image highlightImg;
    //[SerializeField] GameObject priceTag;
    //[SerializeField] TextMeshProUGUI priceText;

    [Space(5)]
    [Header("Private")]
    public ShopItemSo shopItemSo;
    [SerializeField] Shop shop;


    public void _SelectItem()
    {
        Debug.Log(this.shopItemSo.nameString);
        shop.SelectItem(this);
    }

    //public Image ReturnHighlightedImg()
    //{
    //    return highlightImg;
    //}

    //public ShopItemSo ReturnShopItemSo()
    //{
    //    return shopItemSo;
    //}
    //public GameObject ReturnPriceTagGO()
    //{
    //    return priceTag;
    //}

    public ShopItem Init(ShopItemSo item,Shop shopp)
    {
        img.sprite = item.image;
        img.color = item.tint;
        shopItemSo = item;
        shop = shopp;
        //priceText.text = item.price.ToString();
        return this;
    }

    //public ShopItem ReturnShopItem()
    //{
    //    return this;
    //}

    public void UnSelectShopItem()
    {
        highlightImg.enabled = false;
        PlayerPrefs.SetInt(shopItemSo.nameString + "SELECTED", 0);
    }
    public void SelectShopItem()
    {
        highlightImg.enabled = true;
        PlayerPrefs.SetInt(shopItemSo.nameString + "SELECTED", 1);
    }

    public void HighlightShopItem()
    {
        highlightImg.enabled = true;
    }
    public void UnHighlightShopItem()
    {
        highlightImg.enabled = false;
    }
}
