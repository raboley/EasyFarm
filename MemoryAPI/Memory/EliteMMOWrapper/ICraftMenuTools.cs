using System.Collections.Generic;

namespace MemoryAPI.Memory.EliteMMOWrapper
{
    public interface ICraftMenuTools
    {
        CraftItem GetCraftItem(int index);
        List<CraftItem> GetCraftItems();
        bool SetCraftItem(int index, int itemId, int itemIndex, int itemCount);
        bool IsCraftMenuOpen { get; }
        bool IsCrafting { get; }
    }
}