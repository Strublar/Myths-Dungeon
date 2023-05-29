﻿using UnityEngine;

[CreateAssetMenu(fileName = "NewEffect", menuName = "Effects/Taunt")]

public class TauntEffect : Effect
{
    public override void Apply(Context context)
    {
        GameManager.gm.ResetThreat();
    }
}