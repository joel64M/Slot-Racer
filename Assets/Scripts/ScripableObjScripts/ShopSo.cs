using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shop", menuName = "SO/Shop")]
public class ShopSo : ScriptableObject
{
    public List<ShopItemSo> shopItems = new List<ShopItemSo>();
}
