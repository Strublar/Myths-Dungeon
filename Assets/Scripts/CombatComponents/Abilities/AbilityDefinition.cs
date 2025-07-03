
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Ability", menuName = "Ability")] 
public class AbilityDefinition : ScriptableObject
{
    public string abilityName;
    public string description;
    public List<Condition> castConditions;
    public float cooldown;
    public List<Effect> effects;
    public AbilityTarget abilityTarget;
    [Header("Serialization")]
    public List<DynamicValue> values;
}

public enum AbilityTarget : int
{
    Any,
    Hero,
    Self,
    Enemy
}