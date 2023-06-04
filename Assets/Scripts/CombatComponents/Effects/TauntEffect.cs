using UnityEngine;

[CreateAssetMenu(fileName = "NewEffect", menuName = "Effects/Taunt")]

public class TauntEffect : Effect
{
    public override void Apply(Context context)
    {
        FightManager.instance.ResetThreat();
        if (context.source is Hero hero)
        {
            hero.threat += 100f * (1 + 0.2f * hero.level);
            FightManager.instance.UpdateMostThreatHero(hero);
        }
    }
}