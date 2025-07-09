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
    public Dictionary<SkillTag, int> skillTags;
    private Dictionary<Carac, int> baseCaracs;
    private Dictionary<Carac, int> caracBonus;

    [Header("Fight Stats")] public Entity currentTarget;
    public float threat;
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

    public new void Update()
    {
        base.Update();
        currentAbilityCooldown -= Time.deltaTime * Mathf.Max((GetCarac(Carac.AbilityHaste)) / 100, 0);

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
        foreach (var skill in skills)
        {
            if (!(skill is PassiveSkillDefinition passiveSkill)) continue;
            passives.AddRange(passiveSkill.passives);
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
                    case Carac.Power:
                    case Carac.CurrentHp:
                    case Carac.AbilityHaste:
                    case Carac.Mastery:
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

    public void PlayPunch()
    {
        model.transform.DOKill(); // Stoppe les tweens en cours pour éviter le spam
        model.transform.localScale = Vector3.one;
        model.transform.DOScale(Vector3.one * 1.05f, .05f)
            .SetEase(Ease.OutBack)
            .OnComplete(() => model.transform.DOScale(Vector3.one, .05f).SetEase(Ease.InOutQuad));
    }

    public void TryCastAbility(AbilityDefinition abilityToCast, Entity target)
    {
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
        if (abilityToCast == definition.baseAbility)
        {
            var updatedCooldown = abilityToCast.cooldown;
            foreach (var skill in skills)
            {
                if (skill is CooldownModificationSkillDefinition cdMod &&
                    cdMod.linkedAbility == abilityToCast)
                    updatedCooldown += cdMod.cooldownModification;
            }

            currentAbilityCooldown = updatedCooldown;
        }

        Pull(target);

        Context context = new Context
        {
            source = this,
            target = target,
            passiveHolder = this,
            isCritical = Random.Range(0, 100) <= GetCarac(Carac.CritChance),
            abilityCast = abilityToCast,
            underlyingPassive = underlyingPassive,
            replacementAbilityPassive = underlyingPassive,
            modifiedDynamicValues = GetModifiedDynamicValues(abilityToCast)
        };

        var effectsToExecute = new List<Effect>(abilityToCast.effects);

        foreach (var skill in skills)
        {
            if (skill is not AddAbilityEffectsSkillDefinition addEffect ||
                addEffect.linkedAbility != abilityToCast) continue;
            addEffect.effects.ForEach(e => effectsToExecute.Add(e));
        }

        if (abilityToCast.projectile == null || context.source == context.target)
        {
            effectsToExecute.ForEach(e => e.Apply(context));
        }
        else
        {
            Projectile projectile = ProjectileManager.instance.GetObject();
            projectile.Init(context, effectsToExecute, abilityToCast.projectile, abilityToCast.travelTime);
        }


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
        if (!CanCast())
            return;

        _isDragging = true;
        var position = eventData.position;
        /*Vector3 newPos = GameManager.instance.mainCamera.ScreenToWorldPoint(new Vector3(position.x,
            position.y, 1f));*/
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;

        if (!isAlive || !CanCast()) return;
        Vector3 newPos = GameManager.instance.mainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x,
            eventData.position.y, 1f));

        Entity target = PlayerController.GetTarget<Entity>(eventData);

        TryCastAbility(ability, target);

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
            TryCastAbility(ability, this);
        }
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
        return (!skills.Contains(skillDefinition) || skillDefinition.rarity == Rarity.Common) &&
               (skillDefinition.holderRequiredClass.Contains(definition.heroClass) ||
                skillDefinition.holderRequiredClass.Count == 0) &&
               skillDefinition.holderRequiredSkills.All(skill => skills.Contains(skill)) &&
               skillDefinition.holderRequiredTags.All(skillTag => skillTags.ContainsKey(skillTag));
    }

    public void AddSkill(SkillDefinition skillDefinition)
    {
        skills.Add(skillDefinition);
        RunManager.instance.ReloadHeroes();
    }

    public Dictionary<DynamicValue, List<DynamicValue>> GetModifiedDynamicValues(AbilityDefinition ability)
    {
        var modifiedDynamicValues = new Dictionary<DynamicValue, List<DynamicValue>>();
        foreach (var skill in skills)
        {
            if (skill is ModifyDynamicValueSkillDefinition modificationSkill &&
                modificationSkill.linkedAbility == ability)
            {
                if (modifiedDynamicValues.ContainsKey(modificationSkill.toReplace))
                {
                    modifiedDynamicValues[modificationSkill.toReplace].Add(modificationSkill.modification);
                }
                else
                {
                    modifiedDynamicValues.Add(modificationSkill.toReplace,
                        new List<DynamicValue>() { modificationSkill.modification });
                }
            }
        }

        return modifiedDynamicValues;
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