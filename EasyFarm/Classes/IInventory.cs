namespace EasyFarm.Classes
{
    public interface IInventory
    {
        int GetCountOfItemsInContainer(string itemPattern, int InventoryContainerId = 0);
        System.Collections.Generic.List<EliteMMO.API.EliteAPI.IItem> GetMatchingItemsFromContainer(string itemPattern, int InventoryContainerId = 0);
        bool HaveItemInInventoryContainer(string itemPattern, int InventoryContainerId = 0);
        bool InventoryIsFull(int InventoryContainerId = 0);
    }
}