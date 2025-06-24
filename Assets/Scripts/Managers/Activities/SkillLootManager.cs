using System;
using System.Collections;
using System.Collections.Generic;
using Misc;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using System.Linq;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SkillLootManager : MonoBehaviour
{
    public static SkillLootManager Instance;

    public List<SkillDefinition> availableSkills;
    public List<LootedSkill> lootedSkills;
    public LootedSkill selectedSkill;
    public int commonProbability, uncommonProbability, rareProbability, epicProbability, legendaryProbability;

    public TextMeshProUGUI skillNameText;
    public TextMeshProUGUI skillDescriptionText;

    public Color selectedColor;
    public Color unselectedColor;

    void Awake()
    {
        Instance = this;
    }

    public void SelectSkill(LootedSkill newSelectedSkill)
    {
        selectedSkill = newSelectedSkill;
        foreach (var lootedItem in lootedSkills)
        {
            if (lootedItem == newSelectedSkill)
            {
                lootedItem.selectionFrame.color = selectedColor;
            }
            else
            {
                lootedItem.selectionFrame.color = unselectedColor;
            }
        }

        skillNameText.text = selectedSkill.definition.skillName;
        skillDescriptionText.text = selectedSkill.definition.description;
    }

    public void Start()
    {
        var rarity = RollRarity();

        foreach (var lootedSkill in lootedSkills)
        {
            lootedSkill.definition = RollSkill(rarity);
            lootedSkill.Init();
        }

        SelectSkill(lootedSkills[0]);
    }

    private Rarity RollRarity()
    {
        int randInt = Random.Range(0,
            (commonProbability + uncommonProbability + rareProbability + epicProbability + legendaryProbability));

        Rarity rarity;
        if (randInt < commonProbability)
            rarity = Rarity.common;
        else if (randInt < commonProbability + uncommonProbability)
            rarity = Rarity.uncommon;
        else if (randInt < commonProbability + uncommonProbability + rareProbability)
            rarity = Rarity.rare;
        else if (randInt < commonProbability + uncommonProbability + rareProbability + epicProbability)
            rarity = Rarity.epic;
        else
            rarity = Rarity.legendary;

        return rarity;
    }

    private SkillDefinition RollSkill(Rarity rarity)
    {
        var skillTags = RunManager.instance.GetAllSkillTags();
        var filteredSkills = availableSkills
            .Where(
                skill => skill.requiredTags.All(tagData => skillTags.ContainsKey(tagData)) &&
                         skill.rarity == rarity &&
                         SkillCanBeEquipped(skill)
            ).ToList();
        var updatedFilteredSkills = new List(filteredSkills)//Faire une copie
        int totalWeight = 0;
        foreach (var entry in skillTags)
        {
            totalWeight += entry.Value;
        }

        int selector = Random.Range(0, totalWeight);


        foreach (var entry in skillTags)
        {
            selector -= entry.Value;
            if (selector < 0)
            {
                updatedFilteredSkills = updatedFilteredSkills
                    .Where(
                        skill => skill.tags.Exists(tagData => tagData.tag == entry.Key)
                    ).ToList();
                break;
            }
        }
        

        return updatedFilteredSkills.Count != 0 ? updatedFilteredSkills[Random.Range(0, updatedFilteredSkills.Count())] : filteredSkills[Random.Range(0, filteredSkills.Count())];
    }

    private bool SkillCanBeEquipped(SkillDefinition skillDefinition)
    {
        foreach (var hero in RunManager.instance.heroes)
        {
            if (hero.CanEquipSkill(skillDefinition))
                return true;
        }

        return false;
    }

    public void Choose(LootedSkill lootedSkill, Hero target)
    {
        target.AddSkill(lootedSkill.definition);
        RunManager.instance.StartNewBoss();
        SceneManager.UnloadSceneAsync("SkillScene");
    }
}
