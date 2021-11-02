using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int level;
    public ItemQuality quality;
    public ItemDefinition definition;
    public string GetName()
    {
        string name ="";
        switch(quality)
        {
            case ItemQuality.enhanced:
                name += "Enhanced ";
                break;
            case ItemQuality.masterwork:
                name += "Masterwork ";
                break;
            case ItemQuality.legacy:
                name += "Legacy ";
                break;
        }
        name += definition.name+" "+level;
        return name;
    }
}

public enum ItemQuality
{
    normal,
    enhanced,
    masterwork,
    legacy
}