using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum HeroType
{
    Tank,
    Heal,
    DPS,
    none
}

public class Hero : Entity, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    #region Stats

    [Header("Definition")] public HeroDefinition definition;
    public ItemDefinition item;
    public List<SkillDefinition> skills;
    public AbilityDefinition ability;
    public Dictionary<SkillTag, int> skillTags;
    [Header("Bonus Stats")] 
    public Dictionary<Carac, int> baseCaracs;
    public Dictionary<Carac, int> caracBonus;

    [Header("Fight Stats")] public Entity currentTarget;
    public float threat;
    [HideInInspector] public float currentAttackCooldown = 0f;
    [HideInInspector] public float currentAbilityCooldown = 0f;
    [Header("Component objects")] public GameObject model;
    public GameObject healthBar;
    private bool isDragging;
    private Vector3 modelInitialPos;

    private Context selfContext;

    #endregion

    public void Update()
    {
        currentAttackCooldown -= Time.deltaTime * Mathf.Max((100 + GetCarac(Carac.attackSpeed)) / 100, 0);
        if (CanAttack())
        {
            currentTarget = definition.attackTargetSelector.GetTargets(selfContext)[0];
            Attack(currentTarget);
        }

        currentAbilityCooldown -= Time.deltaTime * Mathf.Max((100 + GetCarac(Carac.abilityHaste)) / 100, 0);
        if (Input.touchCount > 0)
        {
            if (isDragging)
            {
                Vector2 newPos = Input.GetTouch(0).position;
                model.transform.position = newPos;
            }
        }
        else
        {
            isDragging = false;
            model.transform.localPosition = modelInitialPos;
        }

        if (!CanCast() && RunManager.instance.fightStarted)
            model.GetComponent<Image>().color = Color.gray;
        else
            model.GetComponent<Image>().color = Color.white;
    }

    #region DefinitionLoading

    public void LoadDefinition()
    {
        LoadGraphics();
        ClearPassives();

        if (FightManager.instance != null)
            currentTarget = FightManager.instance.boss;

        LoadPassives();
        LoadCaracs();
        ComputeStats();
        RefreshSkillTags();
        
        selfContext = new Context
        {
            passiveHolder = this, source = this
        };
    }

    private void LoadGraphics()
    {
        if (model != null)
            Destroy(model);
        model = Instantiate(definition.model, transform);
        model.transform.localPosition = new Vector3(.15f, -.5f);
        modelInitialPos = model.transform.localPosition;
        isAlive = true;
        model.SetActive(true);
        healthBar.SetActive(true);
    }

    private void LoadPassives()
    {
        
        var passives = new List<PassiveDefinition>(definition.passives);
        passives.Add(definition.attackPassive);
        foreach (var skill in skills)
        {
            foreach (var passive in skill.passives)
            {
                passives.Add(passive);
            }
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
    }

    public void LoadCaracs()
    {
        baseCaracs = new();
        foreach (var caracData in definition.caracs)
        {
            baseCaracs.Add(caracData.carac,caracData.value);
        }

        caracBonus = new(); 
    }

    private void ComputeStats()
    {
        
        threat = 0;

        foreach (Carac carac in Enum.GetValues(typeof(Carac)))
        {
            caracs[carac] = 0;
            if (baseCaracs.TryGetValue(carac, out int baseValue))
            {
                caracs[carac] = baseValue;
            }
            if (caracBonus.TryGetValue(carac, out int bonusValue))
            {
                caracs[carac]*= (100 + bonusValue) / 100;
            }
        }
        
        /*caracs[Carac.maxHp] = baseCaracs[Carac.maxHp] * (100 + caracBonus[Carac.maxHp]) / 100;
        caracs[Carac.armor] = baseCaracs[Carac.armor] * (100 + caracBonus[Carac.armor]) / 100;
        caracs[Carac.attack] = baseCaracs[Carac.attack] * (100 + caracBonus[Carac.attack]) / 100;
        caracs[Carac.critChance] = baseCaracs[Carac.critChance] * (100 + caracBonus[Carac.critChance]) / 100;
        caracs[Carac.critPower] = baseCaracs[Carac.critPower] * (100 + caracBonus[Carac.critPower]) / 100;*/


        /*if (item.definition != null)
        {
            //Stats
            maxHp += item.definition.hp + item.definition.hpPerLevel * item.qualityLevel;
            armor += item.definition.armor + item.definition.armorPerLevel * item.qualityLevel;
            haste += item.definition.haste + item.definition.hastePerLevel * item.qualityLevel;
            percentPower += item.definition.damageModifier + item.definition.damageModifierPerLevel * item.qualityLevel;
            //Passives
            foreach (PassiveDefinition passive in item.definition.passives)
            {
                GameObject newPassive = Instantiate(passivePrefab, transform);
                Passive pass = newPassive.GetComponent<Passive>();
                pass.holder = this;
                pass.definition = passive;
                pass.level = item.qualityLevel;
                passiveObjects.Add(pass);
            }
        }*/

        currentAbilityCooldown = 0f;
        currentAttackCooldown = 0f;
        caracs[Carac.currentHp] = GetCarac(Carac.maxHp);
    }

    #endregion

    #region Controls

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

    #endregion

    #region Casting&Attacks

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
        PlayPunch();
        
        Context context = new Context
        {
            source = this,
            target = target,
            passiveHolder = null,
            isCritical = Random.Range(0, 100) <= GetCarac(Carac.critChance),
        };
        TriggerManager.triggerMap[Trigger.OnAttack].Invoke(context);
    }
    
    public void PlayPunch()
    {
        model.transform.DOKill(); // Stoppe les tweens en cours pour éviter le spam
        model.transform.localScale = Vector3.one;
        model.transform.DOScale(Vector3.one * 1.05f, .05f)
            .SetEase(Ease.OutBack)
            .OnComplete(() => model.transform.DOScale(Vector3.one, .05f).SetEase(Ease.InOutQuad));
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
            isCritical = Random.Range(0, 100) <= GetCarac(Carac.critChance),
        };
        TriggerManager.triggerMap[Trigger.OnUseAbility].Invoke(context);
        FightManager.instance.lastAbilityName = ability.abilityName;
    }

    public override void DealDamage(Context context)
    {
        PlayWobble();
        base.DealDamage(context);
    }

    #endregion


    public override void Die()
    {
        base.Die();
        model.SetActive(false);
        healthBar.SetActive(false);
        FightManager.instance.GetNewThreatHero();
        FightManager.instance.HeroDies();
    }

    #region DragHandler

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (CanCast())
            isDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        if (!isAlive || !CanCast()) return;


        Entity target = PlayerController.GetTarget<Entity>(eventData);

        if (target == null) return;

        if (!definition.IsSupport)
        {
            if (target is Enemy)
            {
                CastAbility(target.GetComponent<Entity>());
            }
        }
        else
        {
            if (target is Hero && RunManager.instance.fightStarted)
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

    public void OnDrag(PointerEventData eventData)
    {
    }

    #endregion


    public void RefreshSkillTags()
    {
        skillTags = new();
        skillTags.Add(definition.skillTag, 1);
        
        foreach (var skillDefinition in skills)
        {
            foreach (var tagData in skillDefinition.tags)
            {
                if (skillTags.ContainsKey(tagData.tag))
                {
                    skillTags[tagData.tag] += tagData.weight;
                }
                else
                {
                    skillTags[tagData.tag] = tagData.weight;
                }
            }
        }
    }

    public bool CanEquipSkill(SkillDefinition skillDefinition)
    {
        return skillDefinition.holderRequiredTags.All(skillTag => skillTags.ContainsKey(skillTag));
    }
    public void AddSkill(SkillDefinition skillDefinition)
    {
        foreach (var kvp in skillDefinition.caracs)
        {
            if (caracs.TryGetValue(kvp.Key, out int value))
            {
                caracs[kvp.Key] = value + kvp.Value;
            }
            else
            {
                caracs[kvp.Key] = kvp.Value;
            }
        }
        skills.Add(skillDefinition);
    }

    #region Feedbacks

    
    public void PlayWobble()
    {
        model.transform.DOKill();
        model.transform.rotation = Quaternion.identity;

        model.transform.DORotate(new Vector3(0, 0, 15), .01f)
            .SetEase(Ease.InOutSine)
            .SetLoops(8, LoopType.Yoyo)
            .OnComplete(() => transform.rotation = Quaternion.identity);
    }

    #endregion
}