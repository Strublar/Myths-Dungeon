
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skill")] 
public class SkillDefinition : ScriptableObject
{
    public string description;
    public List<PassiveDefinition> passives;
    
    [Header("Serialization")]
    public List<DynamicValue> values;
}