using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LootManager : MonoBehaviour
{
    public static LootManager instance;
    public List<ItemDefinition> lootTable;
    public List<LootedItem> lootedItems;
    public int commonProbability, rareProbability, epicProbability, legendaryProbability;
    public int normalProbability, enhancedProbability, masterworkProbability, legacyProbability;
    public LootedItem selectedItem;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    
    public Color selectedColor;
    public Color unselectedColor;
    public void Awake()
    {
        instance = this;
    }
    public List<ItemDefinition> GetLoots()
    {
        List<ItemDefinition> newList = new List<ItemDefinition>();
        for(int i=0;i<3;i++)
        {
            newList.Add(lootTable[Random.Range(0, lootTable.Count)]);
        }
        return newList;
    }

    public List<ItemRarity> GetRarities()
    {
        List<ItemRarity> newList = new();
        for(int i=0;i<3;i++)
        {
            newList.Add(GetRarity());
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

    public void Start()
    {
        selectedItem = lootedItems[0];
        List<ItemDefinition> loots = GetLoots();
        List<ItemRarity> rarities = GetRarities();
        
        //item
        lootedItems[0].item.definition = loots[0];
        lootedItems[1].item.definition = loots[1];
        lootedItems[2].item.definition = loots[2];
        
        lootedItems[0].item.rarity = rarities[0];
        lootedItems[1].item.rarity = rarities[1];
        lootedItems[2].item.rarity = rarities[2];


        lootedItems[0].Init();
        lootedItems[1].Init();
        lootedItems[2].Init();

        lootedItems[0].gameObject.SetActive(true);
        lootedItems[1].gameObject.SetActive(true);
        lootedItems[2].gameObject.SetActive(true);

        itemNameText.text = selectedItem.item.definition.itemName;
        itemDescriptionText.text = selectedItem.item.definition.description;
    }

    public void SelectItem(LootedItem newSelectedItem)
    {
        selectedItem = newSelectedItem;
        foreach (var lootedItem in lootedItems)
        {
            if (lootedItem == newSelectedItem)
            {
                lootedItem.selectionFrame.color = selectedColor;
            }
            else
            {
                lootedItem.selectionFrame.color = unselectedColor;
            }
        }
    }

    public void Choose(LootedItem chosenLootedItem, Hero hero)
    {
        if(hero.item.definition != null)
        {
            var chosenItemDefinition = chosenLootedItem.item.definition;
            var chosenItemRarity = chosenLootedItem.item.rarity;
            chosenLootedItem.item.definition = hero.item.definition;
            chosenLootedItem.item.rarity = hero.item.rarity;
            hero.item.definition = chosenItemDefinition;
            hero.item.rarity = chosenItemRarity;

            chosenLootedItem.Init();
            hero.LoadDefinition();
        }
        else
        {
            hero.item.definition = chosenLootedItem.item.definition;
            hero.item.rarity = chosenLootedItem.item.rarity;

            chosenLootedItem.gameObject.SetActive(false);

        }
        
        
        /*if(lootChosen == lootedItems.Count)
        {
            SceneManager.LoadScene("HubScene", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("LootScene");
        }*/
    }

    public void Continue()
    {
        //TODO Sell items
        SceneManager.LoadScene("HubScene", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("LootScene");
    }
}
