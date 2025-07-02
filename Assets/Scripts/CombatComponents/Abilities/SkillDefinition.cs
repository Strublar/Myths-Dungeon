using System.Collections.Generic;
using Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skill")]
public class SkillDefinition : ScriptableObject
{
    public string skillName;
    public string description;
    public Sprite skillImage;
    public Rarity rarity;
    public List<PassiveDefinition> passives;
    public List<SkillTagData> tags;
    public List<SkillTag> requiredTags;
    public List<SkillTag> holderRequiredTags;
    public List<HeroClass> holderRequiredClass;

    [Header("Stats")] 
    public List<CaracData> personalCaracs = new();
    public List<CaracData> caracs = new();

    [Header("Serialization")] public List<DynamicValue> values;
}