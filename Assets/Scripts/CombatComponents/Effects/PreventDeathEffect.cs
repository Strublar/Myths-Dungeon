using Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "ReviveEffect", menuName = "Effects/PreventDeath")]

public class PreventDeathEffect : Effect
{
    public override void Apply(Context context)
    {
        if (context.target is Hero hero)
        {
            hero.isAlive = true;
            hero.caracs[Carac.CurrentHp] = 1;
        }
    }
}