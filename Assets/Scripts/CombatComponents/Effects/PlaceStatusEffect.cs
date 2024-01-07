using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlacePassiveEffect", menuName = "Effects/PlaceStatus")]
public class PlaceStatusEffect : Effect
{
    public PassiveDefinition status;

    public override void Apply(Context context)
    {
        GameObject newPassive = Instantiate(context.target.passivePrefab, context.target.transform);
        newPassive.GetComponent<Passive>().holder = context.target;
        newPassive.GetComponent<Passive>().definition = status;
        newPassive.GetComponent<Passive>().level = context.level;

    }
}
