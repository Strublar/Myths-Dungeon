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
    public int hp;
    public int hpPerLevel;
    public int armor;
    public int armorPerLevel;
    public int critPower;
    public int haste;
    public int hastePerLevel;
    public int damageModifier;
    public int damageModifierPerLevel;

    public List<PassiveDefinition> passives;

    [Header("Serialization")]
    public List<DynamicValue> values;
}

public enum ItemRarity
{
    common,
    rare,
    epic,
    legendary
}