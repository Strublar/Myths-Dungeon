using System;
using System.Collections;
using System.Collections.Generic;
using Misc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroTooltipManager : MonoBehaviour
{
    public static HeroTooltipManager instance;
    public GameObject heroToolTip;
    [Header("HeroTooltip")] public TMP_Text heroName;
    public Image heroImage;
    //Stats
    public TMP_Text attack;
    public TMP_Text attackCooldown;
    public TMP_Text health;
    public TMP_Text armor;
    public TMP_Text critChance;
    public TMP_Text mastery
        ;
    public TMP_Text ability;
    public TMP_Text passive;
    /*public Image itemImage;
    public TMP_Text itemDescription;
    public Image rarityFrame;
    public Image background;*/

    public void Awake()
    {
        instance = this;
    }

    private void InitHeroTooltip(Hero hero)
    {
        var context = new Context
        {
            source = hero,
            passiveHolder = hero
        };
        
        heroName.text = hero.definition.heroName;
        heroImage.sprite = hero.definition.heroImage;
        attack.text = "<b><color=#CC6600>Attack\n\n" + hero.GetCarac(Carac.attack) + "</color></b>";
        attackCooldown.text = "<b><color=#008B8B>At.CD\n\n" + hero.definition.attackCooldown*100/hero.GetCarac(Carac.attackSpeed) + "s</color></b>";
        health.text = "<b><color=#006600>Health\n\n" + hero.GetCarac(Carac.maxHp) + "</color></b>";
        armor.text = "<b><color=#333333>Armor\n\n" + hero.GetCarac(Carac.armor) + "</color></b>";
        critChance.text = "<b><color=#990000>Crit%\n\n" + hero.GetCarac(Carac.critChance) + "</color></b>";
        mastery.text = "<b><color=#660066>Mast.\n\n" + hero.GetCarac(Carac.mastery) + "</color></b>";
        
        var formatListAbility = new string[hero.ability.linkedPassives[0].values.Count];
        for (int i = 0; i < hero.ability.linkedPassives[0].values.Count; i++)
        {
            formatListAbility[i] = hero.ability.linkedPassives[0].values[i].computeValue(context).ToString();
        }

        ability.text = "Ability :\n\n"+string.Format(hero.ability.linkedPassives[0].description.Replace("\\n", "\n"), formatListAbility);

        var passiveDefinition = hero.definition.passives[0];

        string[] formatListPassive = new string[passiveDefinition.values.Count];
        for (int i = 0; i < hero.definition.passives[0].values.Count; i++)
        {
            formatListPassive[i] = passiveDefinition.values[i].computeValue(context).ToString();
        }

        passive.text = "Passive :\n\n"+string.Format(passiveDefinition.description.Replace("\\n", "\n"), formatListPassive);

        /*string[] formatListItem = Array.Empty<string>();
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
            ? hero.item.definition.itemName + "\n\n" + string.Format(hero.item.definition.description, formatListItem)
            : "";
        rarityFrame.color = hero.item.definition != null
            ? ItemTooltipManager.instance.rarityMap[hero.item.definition.rarity]
            : ItemTooltipManager.instance.commonColor;
        background.color = ItemTooltipManager.instance.normalColor;*/
    }

    public void ShowToolTip(Hero hero)
    {
        InitHeroTooltip(hero);
        heroToolTip.SetActive(true);
    }

    public void HideToolTip()
    {
        heroToolTip.SetActive(false);
    }

    public void OnTap()
    {
        HideToolTip();
    }
}