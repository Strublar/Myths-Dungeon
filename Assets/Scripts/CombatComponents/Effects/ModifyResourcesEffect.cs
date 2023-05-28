using UnityEngine;

[CreateAssetMenu(fileName = "NewEffect", menuName = "Effects/ModifyResources")]
public class ModifyResourcesEffect : Effect
{
    public DynamicValue value;

    public override void Apply(Context context)
    {
        if (context.target is Hero hero)
        {
            var futureValue = hero.resources + value.computeValue(context);
            hero.resources = Mathf.Clamp(futureValue, 0,
                hero.definition.canResourceOverflow ? futureValue : hero.definition.maxResources);
        }
    }
}