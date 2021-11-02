using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEffect", menuName = "Effects/PlaceStatus")]
public class PlaceStatusEffect : Effect
{
    public PassiveDefinition status;

    public override void Apply(Entity source, Entity target, int level)
    {
        GameObject newPassive = Instantiate(target.passivePrefab, target.transform);
        newPassive.GetComponent<Passive>().holder = target;
        newPassive.GetComponent<Passive>().definition = status;
        newPassive.GetComponent<Passive>().level = level;

    }
}
