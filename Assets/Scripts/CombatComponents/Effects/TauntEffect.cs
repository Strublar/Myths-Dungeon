using Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "TauntEffect", menuName = "Effects/Taunt")]

public class TauntEffect : Effect
{
    public override void Apply(Context context)
    {
        FightManager.instance.ResetThreat();
        if (context.source is Hero hero)
        {
            hero.threat += hero.GetCarac(Carac.attack) * 10;
            FightManager.instance.UpdateMostThreatHero(hero);
        }
    }
}