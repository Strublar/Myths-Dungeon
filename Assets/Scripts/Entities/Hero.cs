using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum HeroType
{
    Tank,
    Heal,
    DPS,
    none
}

public class Hero : Entity
{
    #region Stats

    [Header("Definition")] public HeroDefinition definition;
    public Item item;
    public SkillDefinition skill;
    public AbilityDefinition ability;

    [Header("FightStats")] [HideInInspector]
    public float currentAttackCooldown = 0f;
    [HideInInspector] public float currentAbilityCooldown = 0f;
    public float threat;
    public int power;
    public int critChance;
    public int critPower;
    public Entity currentTarget;

    [Header("Component objects")] public GameObject model;
    public GameObject healthBar;
    private bool isDragging;
    private Vector3 modelInitialPos;

    private Context selfContext;

    #endregion

    public void Update()
    {
        currentAttackCooldown -= Time.deltaTime * Mathf.Max((100+haste) / 100, 0);
        if (CanAttack())
        {
            currentTarget = definition.attackTargetSelector.GetTargets(selfContext)[0];
            Attack(currentTarget);
        }
        currentAbilityCooldown -= Time.deltaTime * Mathf.Max((100+haste) / 100, 0);
        if (Input.touchCount > 0)
        {
            if (isDragging)
            {
                Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                newPos.z = 0;
                model.transform.position = newPos;
            }
        }
        else
        {
            isDragging = false;
            model.transform.localPosition = modelInitialPos;
        }

        if (!CanCast() && RunManager.instance.fightStarted)
            model.GetComponent<SpriteRenderer>().color = Color.gray;
        else
            model.GetComponent<SpriteRenderer>().color = Color.white;
    }

    


    public void LoadDefinition()
    {
        if (model != null)
            Destroy(model);
        model = Instantiate(definition.model, transform);
        model.transform.localPosition = new Vector3(.15f, -.5f);
        modelInitialPos = model.transform.localPosition;
        isAlive = true;
        model.SetActive(true);
        healthBar.SetActive(true);
        ClearPassives();

        maxHp = definition.hp;
        armor = definition.armor;
        power = definition.power;
        haste = 0;
        percentPower = 0;
        
        threat = 0;
        critChance = definition.critChance;
        critPower = definition.critPower;
        if(FightManager.instance != null)
            currentTarget = FightManager.instance.boss;

        var passives = new List<PassiveDefinition>(definition.passives);
        passives.Add(definition.attackPassive);
        foreach (var passive in skill.passives)
        {
            passives.Add(passive);
        }
        foreach (var passive in ability.linkedPassives)
        {
            passives.Add(passive);
        }

        foreach (PassiveDefinition passive in passives)
        {
            GameObject newPassive = Instantiate(passivePrefab, this.transform);
            Passive pass = newPassive.GetComponent<Passive>();
            pass.holder = this;
            pass.definition = passive;
            passiveObjects.Add(pass);
        }

        if (item.definition != null)
        {
            //Stats
            maxHp += item.definition.hp + item.definition.hpPerLevel * item.level;
            armor += item.definition.armor + item.definition.armorPerLevel * item.level;
            haste += item.definition.haste + item.definition.hastePerLevel * item.level;
            percentPower += item.definition.damageModifier + item.definition.damageModifierPerLevel * item.level;
            //Passives
            foreach (PassiveDefinition passive in item.definition.passives)
            {
                GameObject newPassive = Instantiate(passivePrefab, transform);
                Passive pass = newPassive.GetComponent<Passive>();
                pass.holder = this;
                pass.definition = passive;
                pass.level = item.level;
                passiveObjects.Add(pass);
            }
        }

        selfContext = new Context
        {
            passiveHolder = this, source = this
        };

        currentAbilityCooldown = 0f;
        currentHp = maxHp;
    }

    public void OnTap()
    {
        isDragging = false;
        if (!RunManager.instance.fightStarted)
        {
            HeroTooltipManager.instance.InitHeroTooltip(this);
            HeroTooltipManager.instance.ShowToolTip();
        }
    }

    public void OnDrag(GameObject target)
    {
        if (isAlive && CanCast())
        {
            if (!definition.IsSupport)
            {
                if (target.CompareTag("Boss"))
                {
                    CastAbility(target.GetComponent<Entity>());
                }
            }
            else
            {
                if (target.CompareTag("Hero") && RunManager.instance.fightStarted)
                {
                    CastAbility(target.GetComponent<Entity>());
                }
            }

            if (FightManager.instance.mostThreatHero == null)
            {
                FightManager.instance.mostThreatHero = this;
            }

            if (threat >= FightManager.instance.mostThreatHero.threat)
            {
                FightManager.instance.mostThreatHero = this;
            }
        }

        isDragging = false;
    }

    public void Pull(Entity target)
    {
        if (!RunManager.instance.fightStarted)
        {
            FightManager.instance.mostThreatHero = this;
            RunManager.instance.fightStarted = true;
            FightManager.instance.bossTimer = Time.time;
            Context context = new Context
            {
                source = this,
                target = target,
                passiveHolder = null
            };
            TriggerManager.triggerMap[Trigger.OnPull].Invoke(context);
        }
    }

    public void OnStartDragging()
    {
        if (CanCast())
            isDragging = true;
    }

    public void OnStayedHovered()
    {
        HeroTooltipManager.instance.InitHeroTooltip(this);
        HeroTooltipManager.instance.ShowToolTip();
        isDragging = false;
    }

    public bool CanCast()
    {
        bool canCast = currentAbilityCooldown <= 0;
        foreach (var condition in ability.castConditions)
        {
            canCast &= condition.ShouldTrigger(selfContext);
        }

        return canCast;
    }
    private bool CanAttack()
    {
        return currentAttackCooldown < 0 && RunManager.instance.fightStarted;
    }


    private void Attack(Entity target)
    {
        currentAttackCooldown = definition.attackCooldown;
        Context context = new Context
        {
            source = this,
            target = target,
            passiveHolder = null,
            isCritical = Random.Range(0, 100) <= critChance,
        };
        TriggerManager.triggerMap[Trigger.OnAttack].Invoke(context);
    }

    private void CastAbility(Entity target)
    {
        currentAbilityCooldown = ability.cooldown;
        Pull(target);
        Context context = new Context
        {
            source = this,
            target = target,
            passiveHolder = null,
            isCritical = Random.Range(0, 100) <= critChance,
        };
        TriggerManager.triggerMap[Trigger.OnUseAbility].Invoke(context);
        FightManager.instance.lastAbilityName = ability.abilityName;
    }
    public override void Die()
    {
        base.Die();
        model.SetActive(false);
        healthBar.SetActive(false);
        FightManager.instance.GetNewThreatHero();
        FightManager.instance.HeroDies();
    }
    
    private void FindHealerTarget()
    {
    }
}