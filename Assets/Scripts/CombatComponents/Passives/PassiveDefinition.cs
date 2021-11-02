using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive")]
public class PassiveDefinition : ScriptableObject
{
    public string descrpition;
    public Trigger trigger;
    public int triggerCount;
    public Trigger endTrigger;
    public int endTriggerCount;
    public TargetSelector targets;
    public List<Condition> conditions;
    public List<Effect> effects;
    [Header("Serialization")]
    public List<float> values;
    public List<float> scaling;


    
}
