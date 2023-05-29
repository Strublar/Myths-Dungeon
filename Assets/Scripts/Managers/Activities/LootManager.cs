using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LootManager : MonoBehaviour
{
    public static LootManager instance;
    public List<ItemDefinition> lootTable;
    public List<LootedItem> lootedItems;
    public List<ExperienceItem> experienceItems;
    public int commonProbability, rareProbability, epicProbability, legendaryProbability;
    public int normalProbability, enhancedProbability, masterworkProbability, legacyProbability;
    public int lootChosen = 0;

    public void Awake()
    {
        instance = this;
    }
    public List<ItemDefinition> GetLoots()
    {
        List<ItemDefinition> newList = new List<ItemDefinition>();
        for(int i=0;i<3;i++)
        {
            ItemRarity rarity = GetRarity();
            List<ItemDefinition> itemList = (from item in lootTable where item.rarity == rarity select item).ToList();
            newList.Add(itemList[Random.Range(0, itemList.Count)]);
        }
        return newList;
    }

    public ItemRarity GetRarity()
    {
        int randInt = Random.Range(0, (commonProbability + rareProbability + epicProbability + legendaryProbability));


        ItemRarity rarity;
        if (randInt < commonProbability)
            rarity = ItemRarity.common;
        else if (randInt < commonProbability + rareProbability)
            rarity = ItemRarity.rare;
        else if (randInt < commonProbability + rareProbability + epicProbability)
            rarity = ItemRarity.epic;
        else
            rarity = ItemRarity.legendary;

        return rarity;
    }

    public ItemQuality GetQuality()
    {
        int randInt = Random.Range(0, (normalProbability + enhancedProbability + masterworkProbability + legacyProbability));


        ItemQuality quality;
        if (randInt < normalProbability)
            quality = ItemQuality.normal;
        else if (randInt < normalProbability + enhancedProbability)
            quality = ItemQuality.enhanced;
        else if (randInt < normalProbability + enhancedProbability + masterworkProbability)
            quality = ItemQuality.masterwork;
        else
            quality = ItemQuality.legacy;

        return quality;
    }

    public int GetBonusFromQuality(ItemQuality quality)
    {
        switch(quality)
        {
            case ItemQuality.normal:
                return 0;
            case ItemQuality.enhanced:
                return 1;
            case ItemQuality.masterwork:
                return 3;
            case ItemQuality.legacy:
                return 0;
            default:
                return 0;
        }
    }

    public void Start()
    {
        lootChosen = 0;
        List<ItemDefinition> loots = GetLoots();

        //level
        lootedItems[0].item.level = RunManager.instance.bossBeaten;
        lootedItems[1].item.level = RunManager.instance.bossBeaten;
        lootedItems[2].item.level = RunManager.instance.bossBeaten;

        //item
        lootedItems[0].item.definition = loots[0];
        lootedItems[1].item.definition = loots[1];
        lootedItems[2].item.definition = loots[2];

        //Quality
        lootedItems[0].item.quality = GetQuality();
        lootedItems[1].item.quality = GetQuality();
        lootedItems[2].item.quality = GetQuality();
        lootedItems[0].item.level += GetBonusFromQuality(lootedItems[0].item.quality);
        lootedItems[1].item.level += GetBonusFromQuality(lootedItems[1].item.quality);
        lootedItems[2].item.level += GetBonusFromQuality(lootedItems[2].item.quality);


        lootedItems[0].Init();
        lootedItems[1].Init();
        lootedItems[2].Init();

        lootedItems[0].gameObject.SetActive(true);
        lootedItems[1].gameObject.SetActive(true);
        lootedItems[2].gameObject.SetActive(true);


        experienceItems[0].gameObject.SetActive(true);
        experienceItems[1].gameObject.SetActive(true);
        experienceItems[2].gameObject.SetActive(true);
    }

    public void Choose(int id)
    {
        lootChosen++;

        lootedItems[id].gameObject.SetActive(false);
        experienceItems[id].gameObject.SetActive(false);
        if(lootChosen == lootedItems.Count)
        {
            SceneManager.LoadScene("ActivityScene", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("LootScene");
        }
    }


}
