using MemoryAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.Classes
{
    public interface IEquipableItem
    {
        string Name { get; set; }
        EquipSlots Slot { get; set; }
    }
}
