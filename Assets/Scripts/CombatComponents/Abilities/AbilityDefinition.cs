
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Ability")] 
public class AbilityDefinition : ScriptableObject
{
    public string abilityName;
    public List<Condition> castConditions;
    public float cooldown;
    public List<PassiveDefinition> linkedPassives;
    public AbilityTarget abilityTarget;
}

public enum AbilityTarget : int
{
    Any,
    Hero,
    Self,
    Enemy
}