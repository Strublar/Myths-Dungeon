
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbility", menuName = "Ability")] 
public class AbilityDefinition : ScriptableObject
{
    public string abilityName;
    public List<Condition> castConditions;
    public float cooldown;
    public List<PassiveDefinition> linkedPassives;
}