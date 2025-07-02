using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum HeroType
{
    Tank,
    Heal,
    DPS
}

public enum HeroClass
{
    Warrior,
    Paladin,
    Archer,
    BladeMaster,
    Mage,
    Druid,
    Priest
}

public class Hero : Entity, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    #region Stats

    [Header("Definition")] public HeroDefinition definition;
    public ItemDefinition item;
    public List<SkillDefinition> skills;
    public AbilityDefinition ability;
    public Dictionary<SkillTag, int> SkillTags;
    private Dictionary<Carac, int> baseCaracs;
    private Dictionary<Carac, int> caracBonus;

    [Header("Fight Stats")] public Entity currentTarget;
    public float threat;
    [HideInInspector] public float currentAttackCooldown = 0f;
    [HideInInspector] public float currentAbilityCooldown = 0f;

    [Header("Component objects")] public GameObject model;
    public GameObject healthBar;
    public GameObject cooldownBar;
    private bool _isDragging;
    private Vector3 _modelInitialPos;
    public OrbitSpawner orbitSpawner;

    private bool isModelActive;
    private Image modelImage;
    private Image colorFilter;
    private Context _selfContext;

    #endregion

    public void Update()
    {
        base.Update();
        currentAttackCooldown -= Time.deltaTime * Mathf.Max((GetCarac(Carac.AttackSpeed)) / 100, 0);
        currentAbilityCooldown -= Time.deltaTime * Mathf.Max((GetCarac(Carac.AbilityHaste)) / 100, 0);

        //Attack temporarily disabled
        /*if (CanAttack() && RunManager.instance.fightStarted)
        {
            currentTarget = definition.attackTargetSelector.GetTargets(_selfContext)[0];
            Attack(currentTarget);
        }*/

        if (Input.touchCount > 0)
        {
            if (_isDragging)
            {
                var uiCamera = GameManager.instance.mainCamera;

                Vector3 newPos = uiCamera.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x,
                    Input.GetTouch(0).position.y, 1f));
                model.transform.position = newPos;
            }
        }
        else
        {
            _isDragging = false;
            model.transform.localPosition = _modelInitialPos;
        }

        if (RunManager.instance.fightStarted)
        {
            if (CanCast())
                SetModelActive(CanCast());
            else
                UpdateModelCooldownFeedback();
        }
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

        _selfContext = new Context
        {
            passiveHolder = this, source = this
        };
    }

    private void LoadGraphics()
    {
        if (model != null && model != definition.model)
        {
            Destroy(model);
        }

        model = Instantiate(definition.model, transform);
        model.transform.localPosition = new Vector3(.15f, -.5f);
        modelImage = model.GetComponent<Image>();
        _modelInitialPos = model.transform.localPosition;

        var colorFilterModel = Instantiate(definition.model, model.transform);
        colorFilter = colorFilterModel.GetComponent<Image>();
        colorFilter.color = new Color(1, 1, 1, 0);
        isAlive = true;
        model.SetActive(true);
        healthBar.SetActive(true);
        cooldownBar.SetActive(true);
    }

    private void LoadPassives()
    {
        ability = definition.baseAbility;
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
            Passive newPassive = PassivePool.instance.GetObject(transform);
            newPassive.holder = this;
            newPassive.definition = passive;
            base.passives.Add(newPassive);
            newPassive.Init();
        }
    }

    public void LoadCaracs()
    {
        baseCaracs = new();
        foreach (var caracData in definition.caracs)
        {
            baseCaracs.Add(caracData.carac, caracData.value);
        }

        caracBonus = RunManager.instance.GetSkillCaracBonus();

        foreach (var skill in skills)
        {
            foreach (var caracData in skill.personalCaracs)
            {
                if (caracBonus.ContainsKey(caracData.carac))
                    caracBonus[caracData.carac] += caracData.value;
                else
                    caracBonus[caracData.carac] = caracData.value;
            }
        }
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
                switch (carac)
                {
                    //Percent
                    case Carac.MaxHp:
                    case Carac.CurrentHp:
                    case Carac.AbilityHaste:
                    case Carac.Mastery:
                    case Carac.AttackSpeed:
                    case Carac.Attack:
                        caracs[carac] = Mathf.RoundToInt(caracs[carac] * ((100f + bonusValue) / 100f));
                        break;
                    //Flat
                    case Carac.Armor:
                    case Carac.CritPower:
                    case Carac.CritChance:
                        caracs[carac] += bonusValue;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
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
        caracs[Carac.CurrentHp] = GetCarac(Carac.MaxHp);
    }

    #endregion

    #region Casting&Attacks

    public bool CanCast()
    {
        bool canCast = currentAbilityCooldown <= 0;
        foreach (var condition in ability.castConditions)
        {
            canCast &= condition.ShouldTrigger(_selfContext);
        }

        foreach (var passive in passives)
        {
            if (passive.definition.replacementAbility == null) continue;

            var castable = true;
            foreach (var condition in passive.definition.replacementAbility.castConditions)
            {
                castable &= condition.ShouldTrigger(_selfContext);
            }

            canCast |= castable;
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
        //PlayPunch();

        Context context = new Context
        {
            source = this,
            target = target,
            passiveHolder = null,
            isCritical = Random.Range(0, 100) <= GetCarac(Carac.CritChance),
        };
        TriggerManager.triggerMap[Trigger.OnAttack].Invoke(context);
        if (context.isCritical)
            TriggerManager.triggerMap[Trigger.OnCrit].Invoke(context);
    }

    public void PlayPunch()
    {
        model.transform.DOKill(); // Stoppe les tweens en cours pour éviter le spam
        model.transform.localScale = Vector3.one;
        model.transform.DOScale(Vector3.one * 1.05f, .05f)
            .SetEase(Ease.OutBack)
            .OnComplete(() => model.transform.DOScale(Vector3.one, .05f).SetEase(Ease.InOutQuad));
    }

    private void TryCastAbility(Entity target)
    {
        AbilityDefinition abilityToCast = null;
        Passive underlyingPassive = null;
        if (currentAbilityCooldown <= 0)
        {
            abilityToCast = ability;
        }
        else
        {
            foreach (var passive in passives)
            {
                if (passive.definition.replacementAbility != null)
                {
                    abilityToCast = passive.definition.replacementAbility;
                    underlyingPassive = passive;
                    break;
                }
            }
        }

        if (abilityToCast == null) return;

        bool validTarget;
        switch (abilityToCast.abilityTarget)
        {
            case AbilityTarget.Any:
                validTarget = true;
                break;
            case AbilityTarget.Hero:
                validTarget = target is Hero;
                break;
            case AbilityTarget.Self:
                validTarget = target == this;
                break;
            case AbilityTarget.Enemy:
                validTarget = target is Enemy;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (!validTarget || !target.isAlive) return;
        if (abilityToCast == definition.baseAbility) currentAbilityCooldown = abilityToCast.cooldown;
        if (target is Enemy) Pull(target);

        Context context = new Context
        {
            source = this,
            target = target,
            passiveHolder = null,
            isCritical = Random.Range(0, 100) <= GetCarac(Carac.CritChance),
            abilityCast = abilityToCast,
            underlyingPassive = underlyingPassive,
            replacementAbilityPassive = underlyingPassive
        };
        TriggerManager.triggerMap[Trigger.OnUseAbility].Invoke(context);
        if (context.isCritical)
            TriggerManager.triggerMap[Trigger.OnCrit].Invoke(context);
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

    #endregion


    public override void Die()
    {
        base.Die();
        model.SetActive(false);
        healthBar.SetActive(false);
        cooldownBar.SetActive(false);
        FightManager.instance.GetNewThreatHero();
        FightManager.instance.HeroDies();
    }

    #region DragHandler

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (CanCast())
            _isDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;

        if (!isAlive || !CanCast()) return;


        Entity target = PlayerController.GetTarget<Entity>(eventData);

        if (target == null) return;

        TryCastAbility(target);

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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isDragging)
            return;

        if (!RunManager.instance.fightStarted)
        {
            HeroTooltipManager.instance.ShowToolTip(this);
        }
        else
        {
            if (!isAlive || !CanCast()) return;
            TryCastAbility(this);
        }
    }

    #endregion


    public void RefreshSkillTags()
    {
        SkillTags = new();
        SkillTags.Add(definition.skillTag, 1);

        foreach (var skillDefinition in skills)
        {
            foreach (var tagData in skillDefinition.tags)
            {
                if (SkillTags.ContainsKey(tagData.tag))
                {
                    SkillTags[tagData.tag] += tagData.weight;
                }
                else
                {
                    SkillTags[tagData.tag] = tagData.weight;
                }
            }
        }
    }

    public bool CanEquipSkill(SkillDefinition skillDefinition)
    {
        return (!skills.Contains(skillDefinition) || skillDefinition.rarity == Rarity.Common) &&
               (skillDefinition.holderRequiredClass.Contains(definition.heroClass) ||
                skillDefinition.holderRequiredClass.Count == 0) &&
               skillDefinition.holderRequiredTags.All(skillTag => SkillTags.ContainsKey(skillTag));
    }

    public void AddSkill(SkillDefinition skillDefinition)
    {
        skills.Add(skillDefinition);
        RunManager.instance.ReloadHeroes();
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

    public void PlayPunchScale()
    {
        model.transform.DOKill(true);
        model.transform.localScale = Vector3.one;
        model.transform.DOScale(model.transform.localScale * 1.4f, .1f)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                model.transform.DOScale(Vector3.one, .2f)
                    .SetEase(Ease.InOutQuad);
            });
    }

    public void PlayDamageFeedback()
    {
        colorFilter.color = new Color(1, 0, 0, 0.8f); // rouge semi-transparent
        colorFilter.DOFade(0f, 1f).SetEase(Ease.OutQuad);
    }

    #endregion

    public override void DealDamage(Context context)
    {
        base.DealDamage(context);
        PlayDamageFeedback();
    }

    public void SetModelActive(bool isActive)
    {
        if (!isModelActive && isActive)
            PlayPunchScale();
        isModelActive = isActive;
        modelImage.color = isActive ? Color.white : Color.grey;
    }

    public void UpdateModelCooldownFeedback()
    {
        isModelActive = false;
        var progression = 1f - (currentAbilityCooldown / definition.baseAbility.cooldown);
        Color inactiveColor = new Color(.4f, .4f, .4f);
        Color activeColor = new Color(.6f, .6f, .6f);

        //model.transform.localScale = Vector3.Lerp(new Vector3(.8f, .8f), new Vector3(1, 1), progression);
        modelImage.color = Color.Lerp(inactiveColor, activeColor, progression);
    }
}