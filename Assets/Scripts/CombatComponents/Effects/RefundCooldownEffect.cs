using UnityEngine;

[CreateAssetMenu(fileName = "RefundCooldownEffect", menuName = "Effects/RefundCooldown")]

public class RefundCooldownEffect : Effect
{
    public DynamicValue percentAmount;
    public override void Apply(Context context)
    {
        if (context.source is Hero hero)
        {
            hero.currentCooldown -= percentAmount.computeValue(context) / 100f * hero.ability.cooldown;
        }
    }
}