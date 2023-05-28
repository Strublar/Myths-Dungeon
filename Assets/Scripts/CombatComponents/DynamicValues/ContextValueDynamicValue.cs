using UnityEngine;

[CreateAssetMenu(fileName = "NewDynamicValue", menuName = "DynamicValue/ContextValue")]

    public class ContextValueDynamicValue : DynamicValue
    {
        public override int computeValue(Context context)
        {
            return context.value;
        }
    }
