using UnityEngine;

[CreateAssetMenu(fileName = "CooldownModificationSkill", menuName = "Skill/PassiveSkill")]
public class CooldownModificationSkillDefinition : SkillDefinition
{
    public AbilityDefinition linkedAbility;
    public float cooldownModification;
}