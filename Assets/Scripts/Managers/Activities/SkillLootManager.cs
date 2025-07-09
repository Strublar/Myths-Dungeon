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
    public static SkillLootManager instance;

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
        instance = this;
        SkillDefinition[] loadedSkills = Resources.LoadAll<SkillDefinition>("Data/Skills");
        availableSkills = new List<SkillDefinition>(loadedSkills);
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

        var heroesThatCanEquip = new List<Hero>();
        foreach (var hero in RunManager.instance.heroes)
        {
            bool canEquip = hero.CanEquipSkill(newSelectedSkill.definition);
            hero.SetModelActive(hero.CanEquipSkill(newSelectedSkill.definition));
            if (canEquip)
                heroesThatCanEquip.Add(hero);
        }

        var context = new Context
        {
        };
        if (heroesThatCanEquip.Count == 1)
        {
            context.passiveHolder = heroesThatCanEquip[0];
        }

        string[] formatListSkill = new string[selectedSkill.definition.values.Count];
        for (int i = 0; i < selectedSkill.definition.values.Count; i++)
        {
            formatListSkill[i] = selectedSkill.definition.values[i].ComputeString(context);
        }

        skillDescriptionText.text =
            string.Format(selectedSkill.definition.description.Replace("\\n", "\n"), formatListSkill);

        skillDescriptionText.text += selectedSkill.definition.BuildCaracsString();
        
        skillNameText.text = selectedSkill.definition.skillName;
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
            rarity = Rarity.Common;
        else if (randInt < commonProbability + uncommonProbability)
            rarity = Rarity.Uncommon;
        else if (randInt < commonProbability + uncommonProbability + rareProbability)
            rarity = Rarity.Rare;
        else if (randInt < commonProbability + uncommonProbability + rareProbability + epicProbability)
            rarity = Rarity.Epic;
        else
            rarity = Rarity.Legendary;

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

        if (filteredSkills.Count == 0)
        {
            filteredSkills = availableSkills
                .Where(
                    skill => skill.requiredTags.All(tagData => skillTags.ContainsKey(tagData)) &&
                             skill.rarity == Rarity.Common &&
                             SkillCanBeEquipped(skill)
                ).ToList();
        }

        var updatedFilteredSkills = new List<SkillDefinition>(filteredSkills);
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


        return updatedFilteredSkills.Count != 0
            ? updatedFilteredSkills[Random.Range(0, updatedFilteredSkills.Count())]
            : filteredSkills[Random.Range(0, filteredSkills.Count())];
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
        if (!target.CanEquipSkill(lootedSkill.definition)) return;

        target.AddSkill(lootedSkill.definition);
        RunManager.instance.StartNewBoss();
        SceneManager.UnloadSceneAsync("SkillScene");
    }
}