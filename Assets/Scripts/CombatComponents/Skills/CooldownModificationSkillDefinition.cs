using UnityEngine;

[CreateAssetMenu(fileName = "CooldownModificationSkill", menuName = "Skill/CooldownModification")]
public class CooldownModificationSkillDefinition : SkillDefinition
{
    public AbilityDefinition linkedAbility;
    public float cooldownModification;
}