using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ModifyDynamicValueSkill", menuName = "Skill/ModifyDynamicValue")]
public class ModifyDynamicValueSkillDefinition : SkillDefinition
{
    public AbilityDefinition linkedAbility;
    public DynamicValue toReplace;
    public DynamicValue modification;
}


