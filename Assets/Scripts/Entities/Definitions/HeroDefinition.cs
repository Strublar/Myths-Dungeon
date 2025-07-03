using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewHeroDefininition", menuName = "Hero/HeroDefinition")]
public class HeroDefinition : ScriptableObject
{
    public string heroName;
    public Sprite heroImage;
    public GameObject model;
    public HeroType type;
    public HeroClass heroClass;
    public bool isVagabond;

    [Header("Stats")] public float attackCooldown;
    public List<CaracData> caracs;

    [Header("Passives")]
    public AbilityDefinition baseAbility;
    public List<PassiveDefinition> passives;

    public SkillTag skillTag;
    public Color cooldownBarColor;
    public bool IsSupport => type == HeroType.Heal;
    public float ThreatRatio => type == HeroType.Tank ? 5 : 1;
}
