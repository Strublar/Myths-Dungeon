using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHeroDefininition", menuName = "Hero/HeroDefinition")]
public class HeroDefinition : ScriptableObject
{
    public string heroName;
    public Sprite heroImage;
    public GameObject model;
    public HeroType type;
    [Header("Stats")]
    public float hp;
    public float hpPerLevel;
    public float armor;
    public float armorPerLevel;
    public List<PassiveDefinition> passives;
    public float attack;
    public float attackPerLevel;
    public float attackCooldown;
    public bool isHealer;
    public float threatRatio;
    public float critPower;
    

}
