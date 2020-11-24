using System.Collections.Generic;
using EliteMMO.API;

namespace EasyFarm.Classes
{
    public interface IInventory
    {
        bool InventoryIsFull(int InventoryContainerId = 0);
        bool HaveItemInInventoryContainer(string itemPattern, int InventoryContainerId = 0);
        List<EliteAPI.IItem> GetMatchingItemsFromContainer(string itemPattern, int InventoryContainerId = 0);
        int GetCountOfItemsInContainer(string itemPattern, int InventoryContainerId = 0);
    }
}