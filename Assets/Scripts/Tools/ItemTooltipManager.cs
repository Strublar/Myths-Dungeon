using System.Collections;
using System.Collections.Generic;
using Misc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltipManager : MonoBehaviour
{
    public static ItemTooltipManager instance;
    public Collider2D tmCollider;
    public GameObject itemTooltip;
    [Header("ItemTooltip")] public Image itemImage;
    public Image rarityFrame;
    public Image background;
    public TMP_Text itemDescription;
    [Header("Colors")] public Color commonColor;
    public Color uncommonColor;
    public Color rareColor;
    public Color epicColor;
    public Color legendaryColor;

    public Dictionary<Rarity, Color> rarityMap;

    public void Awake()
    {
        instance = this;

        rarityMap = new Dictionary<Rarity, Color>
        {
            {Rarity.common, commonColor},
            {Rarity.uncommon, uncommonColor},
            {Rarity.rare, rareColor},
            {Rarity.epic, epicColor},
            {Rarity.legendary, legendaryColor}
        };
    }

    /*public void InitItemTooltip(Item item)
    {
        var context = new Context
        {
        };
        itemImage.sprite = item != null ? item.definition.itemImage : null;
        string[] formatListItem = new string[item.definition.values.Count];
        for (int i = 0; i < item.definition.values.Count; i++)
        {
            formatListItem[i] = item.definition.values[i].computeValue(context).ToString();
        }

        itemDescription.text = item != null
            ? item.definition.itemName + "\n\n" + string.Format(item.definition.description, formatListItem)
            : "";
        rarityFrame.color = rarityMap[item.definition.rarity];
        background.color = instance.normalColor;
    }*/

    public string LootedItemDescription(LootedItem lootedItem)
    {
        var context = new Context
        {
        };
        string[] formatListItem = new string[lootedItem.item.values.Count];
        for (int i = 0; i < lootedItem.item.values.Count; i++)
        {
            formatListItem[i] = lootedItem.item.values[i].computeValue(context).ToString();
        }

        return string.Format(lootedItem.item.description, formatListItem);
    }

    public void ShowToolTip()
    {
        itemTooltip.SetActive(true);
        tmCollider.enabled = true;
    }

    public void HideToolTip()
    {
        itemTooltip.SetActive(false);
        tmCollider.enabled = false;
    }

    public void OnTap()
    {
        HideToolTip();
    }
}