using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item")]
public class ItemDefinition : ScriptableObject
{
    public string itemName;
    public string description;
    public Rarity rarity;
    public Sprite itemImage;

    public List<PassiveDefinition> passives;

    [Header("Serialization")]
    public List<DynamicValue> values;
}

