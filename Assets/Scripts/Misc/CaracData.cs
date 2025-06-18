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
    }
}