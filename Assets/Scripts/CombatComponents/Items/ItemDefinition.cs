using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item")]
public class ItemDefinition : ScriptableObject
{
    public string itemName;
    public ItemRarity rarity;
    public string description;
    public Sprite itemImage;
    [Header("Stats")]
    public float hp;
    public float hpPerLevel;
    public float armor;
    public float armorPerLevel;
    public float attack;
    public float attackPerLevel;
    public float attackCooldown;
    public bool isHealer;
    public float threatRatio;
    public float critPower;
    public float haste;
    public float hastePerLevel;
    public float damageModifier;
    public float damageModifierPerLevel;

    public List<PassiveDefinition> passives;

    [Header("Serialization")]
    public List<float> values;
    public List<float> scaling;
}

public enum ItemRarity
{
    common,
    rare,
    epic,
    legendary
}