using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.API;

namespace lockpickChance
{
    public class StealyEntry
    {
        public ushort ItemId {  get; set; }
        public float LockpickChance {  get; set; }
    }

    public class LockpickChanceConfiguration : IRocketPluginConfiguration
    {
        public bool Logging {  get; set; }
        public List<StealyEntry> Stealies { get; set; }

        public void LoadDefaults()
        {
            Logging = true;
            Stealies = new List<StealyEntry>
            {
                new StealyEntry { ItemId = 1353, LockpickChance = 50f}
            };
        }
    }
}
