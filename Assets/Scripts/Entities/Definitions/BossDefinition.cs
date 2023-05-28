using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBossDefininition", menuName = "Boss/BossDefinition")]
public class BossDefinition : ScriptableObject
{
    public string bossName;
    public GameObject model;
    [Header("Stats")]
    public int hp;
    public int hpPerLevel;
    public int armor;
    public int armorPerLevel;
    public List<BossSpellDefinition> spells;
    public List<PassiveDefinition> passives;
}
