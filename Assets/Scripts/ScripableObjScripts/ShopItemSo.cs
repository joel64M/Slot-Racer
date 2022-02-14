using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ShopItemName", menuName = "SO/ShopItem")]
public class ShopItemSo : ScriptableObject
{
    public Sprite image;
    public Color tint = Color.white;
    public int price;
    public string nameString;
    public GameObject prefab;
    public bool isDefaultSelected = false;
 
}
