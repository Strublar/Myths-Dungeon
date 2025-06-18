using System;

namespace Misc
{
    public enum SkillTag : int
    {
        //Class tags
        Warrior,
        Paladin,
        Swordsman,
        Archer,
        Mage,
        Priest,
        Druid,
        Cleric
    }

    [Serializable]
    public struct SkillTagData
    {
        public SkillTag tag;
        public int weight;

        public SkillTagData(SkillTag tag, int weight)
        {
            this.tag = tag;
            this.weight = weight;
        }
    }
}