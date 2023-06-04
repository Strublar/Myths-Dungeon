using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewHeroDefininition", menuName = "Hero/HeroDefinition")]
public class HeroDefinition : ScriptableObject
{
    public string heroName;
    public Sprite heroImage;
    public GameObject model;
    public HeroType type;
    [Header("Stats")]
    public int hp;
    public int hpPerLevel;
    public int armor;
    public int armorPerLevel;
    public float threatRatio;
    public int critChance;
    public int critChancePerLevel;
    public int critPower;
    public int critPowerPerLevel;
    [Header("Abilities and Passives")]
    public List<PassiveDefinition> passives;
    public List<AbilityDefinition> availableAbilities;
    public List<SkillDefinition> availableSkills;
    
    public Color cooldownBarColor;
    public bool IsHealer => type == HeroType.Heal;
}
