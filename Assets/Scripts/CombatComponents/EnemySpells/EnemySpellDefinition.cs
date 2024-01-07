using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemySpell", menuName= "Enemy/EnemySpell")]
public class EnemySpellDefinition : ScriptableObject
{
    #region Stats

    [Header("Stats")]
    public int minLevel;
    public List<Effect> effects;
    public float coolDown;
    [HideInInspector]
    public float currentCooldown;
    public TargetSelector targets;
    #endregion
}
