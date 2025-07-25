
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ComparatorCondition", menuName = "Conditions/Comparator")]
public class ComparatorCondition : Condition
{
    public enum Operation
    {
        equals,
        notEquals,
        superior,
        inferior,
        supEquals,
        infEquals
    }
    
    public DynamicValue value1;
    public Operation operation;
    public DynamicValue value2;
    public override bool ShouldTrigger(Context context)
    {
        switch (operation)
        {
            case Operation.equals:
                return value1.Compute(context) == value2.Compute(context);
            case Operation.notEquals:
                return value1.Compute(context) != value2.Compute(context);
            case Operation.superior:
                return value1.Compute(context) > value2.Compute(context);
            case Operation.inferior:
                return value1.Compute(context) < value2.Compute(context);
            case Operation.supEquals:
                return value1.Compute(context) >= value2.Compute(context);
            case Operation.infEquals:
                return value1.Compute(context) <= value2.Compute(context);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}