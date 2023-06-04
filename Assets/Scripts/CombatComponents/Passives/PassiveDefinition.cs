using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive")]
public class PassiveDefinition : ScriptableObject
{
    public string description;
    public Trigger trigger;
    public int triggerCount;
    public Trigger endTrigger;
    public int endTriggerCount;
    public TargetSelector targets;
    public List<Condition> conditions;
    public List<Effect> effects;
    public float internalCooldown;
    [Header("Serialization")]
    public List<DynamicValue> values;
}
