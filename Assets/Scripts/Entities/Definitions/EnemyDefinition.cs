using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "EnemyDefinition", menuName = "Boss/EnemyDefinition")]
public class EnemyDefinition : ScriptableObject
{
    public string enemyName;
    public GameObject model;
    public bool isBoss;
    [Header("Stats")]
    public int hp;
    public int hpPerLevel;
    public float hpLevelExponent;
    public List<EnemySpellDefinition> spells;
    public List<PassiveDefinition> passives;
}
