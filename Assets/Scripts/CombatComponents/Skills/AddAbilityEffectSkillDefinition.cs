using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AddAbilityEffectSkill", menuName = "Skill/AddAbilityEffects")]
public class AddAbilityEffectsSkillDefinition : SkillDefinition
{
    public AbilityDefinition linkedAbility;
    public List<Effect> effects;
}