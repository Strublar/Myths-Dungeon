namespace CombatComponents.Shields
{
    public struct ShieldData
    {
        public int remainingValue;
        public float remainingDuration;

        public ShieldData(int value, float duration)
        {
            this.remainingValue = value;
            this.remainingDuration = duration;
        }

        public ShieldData UpdateValue(int newValue)
        {
            this.remainingValue = newValue;
            return this;
        }

        public ShieldData UpdateDuration(float newDuration)
        {
            remainingDuration = newDuration;
            return this;
        }
    }
}