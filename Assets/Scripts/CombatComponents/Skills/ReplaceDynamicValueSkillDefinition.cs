using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReplaceDynamicValueSkill", menuName = "Skill/ReplaceDynamicValue")]
public class ReplaceDynamicValueSkillDefinition : SkillDefinition
{
    public AbilityDefinition linkedAbility;
    public DynamicValue toReplace;
    public DynamicValue replaceWith;
}