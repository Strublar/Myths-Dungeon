using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Misc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Entity
{
    public int level;
    public EnemyDefinition definition;
    [CanBeNull] public TextMeshPro enemyNameMesh;
    
    public GameObject model;
    private Dictionary<EnemySpellDefinition, float> spellCooldowns = new();
    public PassiveDefinition bossFrenzy;
    
    public void LoadDefinition()
    {
        ClearPassives();
        caracs[Carac.MaxHp] = definition.hp + definition.hpPerLevel*level;
        if(enemyNameMesh != null)
            enemyNameMesh.text = definition.enemyName;
        isAlive = true;

        model = Instantiate(definition.model, transform);
        
        foreach (PassiveDefinition passive in definition.passives)
        {
            Passive newPassive = PassivePool.instance.GetObject(transform);
            newPassive.holder = this;
            newPassive.definition = passive;
            newPassive.level = level;
            passives.Add(newPassive);
            newPassive.Init();
        }

        foreach (var spellDefinition in definition.spells)
        {
            spellCooldowns.Add(spellDefinition,spellDefinition.coolDown);
        }
        
        /*
        if(level>=10)//creep //TODO Cible, casting bar
        {
            caracs[Carac.maxHp] *= Mathf.RoundToInt(Mathf.Pow(1.1f, level - 10));
            Passive newPassive = Instantiate(passivePrefab, this.transform);
            newPassive.holder = this;
            newPassive.definition = bossFrenzy;
            newPassive.level = level;
            passives.Add(newPassive);
        }*/
        caracs[Carac.CurrentHp] = GetCarac(Carac.MaxHp);

    }


    public void Update()
    {
        base.Update();
        if (RunManager.instance.fightStarted && isAlive)
        {
            var spells = spellCooldowns.Keys.ToList();
            foreach(var spell in spells)
            {
                var cooldown = spellCooldowns.GetValueOrDefault(spell);
                if(spell.minLevel <= level)
                {
                    cooldown -= Time.deltaTime;
                    if (cooldown < 0)
                    {
                        Cast(spell);
                        cooldown = spell.coolDown;
                    }
                    spellCooldowns[spell] = cooldown;
                }
            }
        }
    }
    public void Cast(EnemySpellDefinition spell)
    {
        List<Entity> spellTargets = spell.targets.GetTargets(new Context());
        var context = new Context
        {
            passiveHolder = this,
            source = this,
            level = level
        };
        foreach(Entity ent in spellTargets)
        {
            if(ent != null)
            {
                context.target = ent;
                foreach (Effect eff in spell.effects)
                {
                    eff.Apply(context);
                }
            }
            
        }
    }

    public override void Die()
    {
        base.Die();
        Destroy(model);
        model = null;
        gameObject.SetActive(false);
        if(definition.isBoss)
            RunManager.instance.BossDefeated();
    }
}
