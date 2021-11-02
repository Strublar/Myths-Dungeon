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
    [Header("HeroTooltip")]
    public TMP_Text heroName;
    public Image heroImage;
    public TMP_Text stats;
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
        heroName.text = hero.definition.heroName;
        heroImage.sprite = hero.definition.heroImage;
        stats.text = "Level : " + hero.level +
                     "\nHp :  " + hero.maxHp +
                     "\nArmor :  " + hero.armor +
                     "\nAttack :  " + hero.attack*(hero.damageModifier/100) +
                     "\nCooldown :  " + string.Format("{0:0.#}",hero.definition.attackCooldown / (hero.haste / 100))+"s" +
                     "\nThreat :  x" + hero.definition.threatRatio;

        string[] formatList = new string[hero.definition.passives[0].values.Count];
        for(int i =0;i< hero.definition.passives[0].values.Count;i++)
        {
            formatList[i] = (hero.definition.passives[0].values[i] + hero.level *
                hero.definition.passives[0].scaling[i]).ToString();
        }
       
        passive.text = string.Format(hero.definition.passives[0].descrpition, formatList);

        string[] formatListItem = new string[0];
        if (hero.item.definition != null)
        {
            formatListItem = new string[hero.item.definition.values.Count];
            for (int i = 0; i < hero.item.definition.values.Count; i++)
            {
                formatListItem[i] = (hero.item.definition.values[i] + hero.item.level *
                    hero.item.definition.scaling[i]).ToString();
            }
        }
        

        itemImage.sprite = hero.item.definition != null ? hero.item.definition.itemImage : null;
        itemDescription.text = hero.item.definition != null ?
            hero.item.GetName()+ "\n\n"+ string.Format(hero.item.definition.description,formatListItem) : "";
        rarityFrame.color = hero.item.definition != null ? 
            ItemTooltipManager.instance.rarityMap[hero.item.definition.rarity] : 
            ItemTooltipManager.instance.commonColor;
        background.color = hero.item.definition != null ? 
            ItemTooltipManager.instance.qualityMap[hero.item.quality] :
            ItemTooltipManager.instance.normalColor;
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
