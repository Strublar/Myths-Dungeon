using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Passive", menuName = "Passive")]
public class PassiveDefinition : ScriptableObject
{
    public string description;
    public Trigger trigger;
    public int triggerCount;
    public Trigger endTrigger;
    public int endTriggerCount;
    public TargetSelector targets;
    public List<Condition> conditions;
    public List<Condition> endTriggerConditions;
    public List<Effect> effects;
    public AbilityDefinition replacementAbility;
    public float internalCooldown;
    public GameObject model;
    public GameObject orbitalObjectModel;
    public List<PassiveType> passiveTypes = new();
    [Header("Serialization")]
    public List<DynamicValue> values;
}

public enum PassiveType
{
    HealingOverTime
}