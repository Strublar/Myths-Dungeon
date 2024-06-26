﻿using UnityEngine;

[CreateAssetMenu(fileName = "Value", menuName = "DynamicValue/Constant")]
public class ConstantDynamicValue : DynamicValue
{

    public int value;
    public int valuePerLevel;
    public override int computeValue(Context context)
    {
        return value + valuePerLevel * (context.level - 1);
    }
}