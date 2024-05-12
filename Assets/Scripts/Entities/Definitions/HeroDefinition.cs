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
    public int attack;
    public float attackCooldown;
    public int critChance;
    public int critPower;
    public float threatRatio;

    [Header("Abilities and Passives")] public TargetSelector attackTargetSelector;
    public PassiveDefinition attackPassive;
    public AbilityDefinition baseAbility;
    public List<PassiveDefinition> passives;
    public List<AbilityDefinition> availableAbilities;
    public List<SkillDefinition> availableSkills;
    
    public Color cooldownBarColor;
    public bool IsSupport => type == HeroType.Heal;
}
