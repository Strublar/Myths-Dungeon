using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltipManager : MonoBehaviour
{
    public static ItemTooltipManager instance;
    public Collider2D tmCollider;
    public GameObject itemTooltip;
    [Header("ItemTooltip")]
    public Image itemImage;
    public Image rarityFrame;
    public Image background;
    public TMP_Text itemDescription;
    [Header("Colors")]
    public Color commonColor;
    public Color rareColor;
    public Color epicColor;
    public Color legendaryColor;
    public Color normalColor;
    public Color enhancedColor;
    public Color masterworkColor;
    public Color legacyColor;
    public Dictionary<ItemQuality, Color> qualityMap;
    public Dictionary<ItemRarity, Color> rarityMap;

    public void Awake()
    {
        instance = this;

        qualityMap = new Dictionary<ItemQuality, Color>
        {
            { ItemQuality.normal, normalColor },
            { ItemQuality.enhanced, enhancedColor },
            { ItemQuality.masterwork, masterworkColor },
            { ItemQuality.legacy, legacyColor }
        };

        rarityMap = new Dictionary<ItemRarity, Color>
        {
            { ItemRarity.common, commonColor },
            { ItemRarity.rare, rareColor },
            { ItemRarity.epic, epicColor },
            { ItemRarity.legendary, legendaryColor }
        };

    }
    public void InitItemTooltip(Item item)
    {
        itemImage.sprite = item != null ? item.definition.itemImage : null;
        string[] formatListItem = new string[item.definition.values.Count];
        for (int i = 0; i < item.definition.values.Count; i++)
        {
            formatListItem[i] = (item.definition.values[i] + item.level *
                item.definition.scaling[i]).ToString();
        }

        itemDescription.text = item != null ? 
            item.GetName()+"\n\n"+ string.Format(item.definition.description, formatListItem) : "";
        rarityFrame.color = rarityMap[item.definition.rarity];
        background.color = qualityMap[item.quality];
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
