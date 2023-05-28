
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbility", menuName = "Ability")] 
public class AbilityDefinition : ScriptableObject
{
    public List<Condition> castConditions;
    public int resourceModification;
    public PassiveDefinition linkedPassive;
}