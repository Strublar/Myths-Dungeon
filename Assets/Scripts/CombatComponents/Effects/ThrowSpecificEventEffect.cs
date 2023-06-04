using System.Collections.Generic;
using UnityEngine;

public enum SpecificEvent
{
    Shout
}

[CreateAssetMenu(fileName = "NewEffect", menuName = "Effects/ThrowSpecificEvent")]
public class ThrowSpecificEventEffect : Effect
{
    public SpecificEvent specificEvent;
    public override void Apply(Context context)
    {
        
    }
}