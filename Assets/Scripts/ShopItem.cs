using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.UIElements;

using TMPro;
public class ShopItem : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] UnityEngine.UI.Image img;
    [SerializeField] UnityEngine.UI.Image highlightImg;
    //[SerializeField] GameObject priceTag;
    //[SerializeField] TextMeshProUGUI priceText;

    [Space(5)]
    [Header("Private")]
    public ShopItemSo shopItemSo;
    [SerializeField] Shop shop;


    public void _SelectItem()
    {
        shop.SelectItem(this);
    }

    public ShopItem Init(ShopItemSo item,Shop shopp)
    {
        img.sprite = item.image;
        //var texture =   AssetPreview.GetAssetPreview(item.prefab);
        //img.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        ////img = Sprite.Create(texture, new Rect(0, 0, 128, 128), new Vector2());


        img.color = item.tint;
        shopItemSo = item;
        shop = shopp;
        //priceText.text = item.price.ToString();
        return this;
    }


    //public void UnSelectShopItem()
    //{
    //    highlightImg.enabled = false;
    //    PlayerPrefs.SetInt(shopItemSo.nameString + "SELECTED", 0);
    //}
    //public void SelectShopItem()
    //{
    //    highlightImg.enabled = true;
    //    PlayerPrefs.SetInt(shopItemSo.nameString + "SELECTED", 1);
    //}

    public void HighlightShopItem()
    {
        highlightImg.enabled = true;
    }
    public void UnHighlightShopItem()
    {
        highlightImg.enabled = false;
    }
}
