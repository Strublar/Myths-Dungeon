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
    public int armor;
    public float attackCooldown;
    public int critChance;
    public int critPower;
    public float threatRatio;

    [Header("Abilities and Passives")]
    public PassiveDefinition attackPassive;
    public List<PassiveDefinition> passives;
    public List<AbilityDefinition> availableAbilities;
    public List<SkillDefinition> availableSkills;
    
    public Color cooldownBarColor;
    public bool IsHealer => type == HeroType.Heal;
}
