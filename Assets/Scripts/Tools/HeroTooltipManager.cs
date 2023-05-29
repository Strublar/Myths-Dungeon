using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroTooltipManager : MonoBehaviour
{
    public static HeroTooltipManager instance;
    public Collider2D tmCollider;
    public GameObject heroToolTip;
    [Header("HeroTooltip")] public TMP_Text heroName;
    public Image heroImage;
    public TMP_Text stats;
    public TMP_Text resourceName;
    public TMP_Text resource;
    public TMP_Text abilityName;
    public TMP_Text ability;
    public TMP_Text passive;
    public Image itemImage;
    public TMP_Text itemDescription;
    public Image rarityFrame;
    public Image background;

    public void Awake()
    {
        instance = this;
    }

    public void InitHeroTooltip(Hero hero)
    {
        var context = new Context
        {
            source = hero,
            level = hero.level
        };
        heroName.text = hero.definition.heroName;
        heroImage.sprite = hero.definition.heroImage;
        stats.text = "Level :\t" + hero.level +
                     "\nHp :\t" + hero.maxHp +
                     "\nArmor :\t" + hero.armor +
                     "\nCrit% :\t" + hero.critChance + "%" +
                     "\nCritPow :\t" + hero.critPower + "%" +
                     "\nThreat :\tx" + hero.definition.threatRatio;

        string[] formatList = new string[hero.ability.linkedPassive.values.Count];
        for (int i = 0; i < hero.ability.linkedPassive.values.Count; i++)
        {
            formatList[i] = hero.ability.linkedPassive.values[i].computeValue(context).ToString();
        }

        resourceName.text = hero.definition.resourceName + " (" + hero.definition.maxResources + ")";
        
        var formatListResource = new string[hero.definition.resourceValues.Count];
        for (int i = 0; i < hero.definition.resourceValues.Count; i++)
        {
            formatListResource[i] = hero.definition.resourceValues[i].computeValue(context).ToString();
        }
        resource.text = string.Format(hero.definition.resourceDescription, formatListResource);

        abilityName.text = hero.ability.abilityName;

        var formatListAbility = new string[hero.ability.linkedPassive.values.Count];
        for (int i = 0; i < hero.ability.linkedPassive.values.Count; i++)
        {
            formatListAbility[i] = hero.ability.linkedPassive.values[i].computeValue(context).ToString();
        }

        ability.text = string.Format(hero.ability.linkedPassive.description, formatListAbility);
        passive.text = string.Format(hero.ability.linkedPassive.description, formatList);

        string[] formatListItem = Array.Empty<string>();
        if (hero.item.definition != null)
        {
            formatListItem = new string[hero.item.definition.values.Count];
            for (int i = 0; i < hero.item.definition.values.Count; i++)
            {
                formatListItem[i] = hero.item.definition.values[i].computeValue(context).ToString();
            }
        }


        itemImage.sprite = hero.item.definition != null ? hero.item.definition.itemImage : null;
        itemDescription.text = hero.item.definition != null
            ? hero.item.GetName() + "\n\n" + string.Format(hero.item.definition.description, formatListItem)
            : "";
        rarityFrame.color = hero.item.definition != null
            ? ItemTooltipManager.instance.rarityMap[hero.item.definition.rarity]
            : ItemTooltipManager.instance.commonColor;
        background.color = hero.item.definition != null
            ? ItemTooltipManager.instance.qualityMap[hero.item.quality]
            : ItemTooltipManager.instance.normalColor;
    }

    public void ShowToolTip()
    {
        heroToolTip.SetActive(true);
        tmCollider.enabled = true;
    }

    public void HideToolTip()
    {
        heroToolTip.SetActive(false);
        tmCollider.enabled = false;
    }

    public void OnTap()
    {
        HideToolTip();
    }
}