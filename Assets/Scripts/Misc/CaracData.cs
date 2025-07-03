using System;

namespace Misc
{
    [Serializable]
    public struct CaracData
    {
        public Carac carac;
        public int value;

        public CaracData(Carac carac, int value)
        {
            this.carac = carac;
            this.value = value;
        }

        public override string ToString()
        {
            switch (carac)
            {
                case Carac.MaxHp:
                case Carac.CurrentHp:
                    return $"<b><color=#{Utils.GetCaracColor(carac)}>+{value} Health</color></b>";
                case Carac.Armor:
                    return $"<b><color=#{Utils.GetCaracColor(carac)}>+{value} Armor</color></b>";
                case Carac.Power:
                    return $"<b><color=#{Utils.GetCaracColor(carac)}>+{value}% Power</color></b>";
                case Carac.CritChance:
                    return $"<b><color=#{Utils.GetCaracColor(carac)}>+{value}% Critical chance </color></b>";
                case Carac.CritPower:
                    return $"<b><color=#{Utils.GetCaracColor(carac)}>+{value}% Critical power</color></b>";
                case Carac.Mastery:
                    return $"<b><color=#{Utils.GetCaracColor(carac)}>+{value} Mastery</color></b>";
                case Carac.AbilityHaste:
                    return $"<b><color=#{Utils.GetCaracColor(carac)}>+{value} Ability haste</color></b>";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}