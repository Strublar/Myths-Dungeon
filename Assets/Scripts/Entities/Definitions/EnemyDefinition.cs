using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "EnemyDefinition", menuName = "Boss/EnemyDefinition")]
public class EnemyDefinition : ScriptableObject
{
    public string enemyName;
    public Sprite sprite;
    public AnimatorController animatorController;
    public bool isBoss;
    [Header("Stats")]
    public int hp;
    public int hpPerLevel;
    public int armor;
    public int armorPerLevel;
    public List<EnemySpellDefinition> spells;
    public List<PassiveDefinition> passives;
}
