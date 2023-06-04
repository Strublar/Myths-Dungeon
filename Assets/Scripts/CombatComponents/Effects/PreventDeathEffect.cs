using UnityEngine;

[CreateAssetMenu(fileName = "NewEffect", menuName = "Effects/PreventDeath")]

public class PreventDeathEffect : Effect
{
    public override void Apply(Context context)
    {
        if (context.target is Hero hero)
        {
            hero.isAlive = true;
            hero.currentHp = 1;
        }
    }
}