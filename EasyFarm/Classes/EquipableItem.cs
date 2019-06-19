using MemoryAPI;

namespace EasyFarm.Classes
{
    public class EquipableItem : IEquipableItem
    {
        private string name;
        private EquipSlots slot;

        public EquipableItem(EquipSlots slot): this("", slot)
        {}

        public EquipableItem(string name, EquipSlots slot)
        {
            this.name = name;
            this.slot = slot;
        }

        public string Name { get => name; set => name = value; }
        public EquipSlots Slot { get => slot; set => slot = value; }
    }
}
