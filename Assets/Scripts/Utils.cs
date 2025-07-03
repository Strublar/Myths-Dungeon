using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Misc;


public static class Utils
{
    public static string GetCaracColor(Carac carac)
    {
        var colorCode = "";
        switch (carac)
        {
            case Carac.MaxHp:
            case Carac.CurrentHp:
                colorCode = "006600";
                break;
            case Carac.Armor:
                colorCode = "333333";
                break;
            case Carac.Power:
                colorCode = "CC6600";
                break;
            case Carac.CritChance:
            case Carac.CritPower:
                colorCode = "990000";
                break;
            case Carac.Mastery:
                colorCode = "660066";
                break;
            case Carac.AbilityHaste:
                colorCode = "008B8B";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return colorCode;
    }
}