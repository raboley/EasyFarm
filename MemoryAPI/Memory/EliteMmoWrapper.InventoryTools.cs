using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EliteMMO.API;
using MemoryAPI.Inventory;

namespace MemoryAPI.Memory
{
    public partial class EliteMmoWrapper
    {
        public class InventoryTools : IInventoryTools
        {
            private readonly EliteAPI _api;

            public InventoryTools(EliteAPI api)
            {
                _api = api;
                
            }

            public string SelectedItemName => (string)_api.Inventory.SelectedItemName;
            public uint SelectedItemId => _api.Inventory.SelectedItemId;
            public uint SelectedItemIndex => _api.Inventory.SelectedItemIndex;
            public uint ShopItemCount => _api.Inventory.ShopItemCount;
            public uint ShopItemCountMax
            {
                get
                {
                    return _api.Inventory.ShopItemCountMax;
                }
                set
                {
                    _api.Inventory.ShopItemCountMax = value;
                }
            }

            public int GetContainerCount(int containerId)
            {
                return _api.Inventory.GetContainerCount(containerId);
            }

            public EliteAPI.InventoryItem GetContainerItem(int containerId, int itemIndex)
            {
                return _api.Inventory.GetContainerItem(containerId, itemIndex);
            }

            public int GetContainerMaxCount(int containerId)
            {
                return _api.Inventory.GetContainerMaxCount(containerId);
            }

            public EliteAPI.InventoryItem GetEquippedItem(int slotId)
            {
                return _api.Inventory.GetEquippedItem(slotId);
            }

            public bool SetBazaarPrice(int price)
            {
                return _api.Inventory.SetBazaarPrice(price);
            }
            
            public int? GetItemIdFromName(string itemPattern, int inventoryContainerId = 0)
            {
                var item = GetMatchingItemsFromContainer(itemPattern, inventoryContainerId);
                if (item.Count == 0)
                    return null;
           
                return (int) item[0].ItemID;
            }

            public EliteAPI.TradeItem GetFirstMatchingTradeItem(string itemPattern)
            {
                var tradeItem = new EliteAPI.TradeItem();
                
                var items = GetMatchingInventoryItemsFromContainer(itemPattern);
                if (items.Count == 0)
                    return tradeItem;
                
                var item = items.FirstOrDefault();
                tradeItem.Index = item.Index;
                tradeItem.ItemCount = (byte) item.Count;
                tradeItem.ItemId = item.Id;
                tradeItem.ItemIndex = (byte) item.Index;

                return tradeItem;
            }
            
            public bool InventoryIsFull(int inventoryContainerId = 0)
        {
            if (_api.Inventory.GetContainerCount(inventoryContainerId) == _api.Inventory.GetContainerMaxCount(inventoryContainerId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HaveItemInInventoryContainer(string itemPattern, int inventoryContainerId = 0)
        {
            var itemInInventory = GetMatchingItemsFromContainer(itemPattern);
            if (itemInInventory.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<EliteAPI.IItem> GetMatchingItemsFromContainer(string itemPattern, int inventoryContainerId = 0)
        {
            List<EliteAPI.IItem> items = GetItemsFromContainer();
            return items.Where(item => item.Name[0].ToLower().Contains(itemPattern.ToLower())).ToList();
        }

        public List<EliteAPI.IItem> GetItemsFromContainer(int InventoryContainerId = 0)
        {
            List<EliteAPI.IItem> items = new List<EliteAPI.IItem>();
            var maxCount = _api.Inventory.GetContainerMaxCount(InventoryContainerId);
            for (uint i = 0; i < maxCount; i++)
            {
                var hasItem = _api.Inventory.GetContainerItem(InventoryContainerId, (int)i);
                if (hasItem.Id == 0)
                {
                    continue;
                }
                var item = _api.Resources.GetItem(hasItem.Id);
                items.Add(item);
            }

            return items;
        }
        
        public List<EliteAPI.InventoryItem> GetInventoryItemsFromContainer(int InventoryContainerId = 0)
        {
            List<EliteAPI.InventoryItem> items = new List<EliteAPI.InventoryItem>();
            var maxCount = _api.Inventory.GetContainerMaxCount(InventoryContainerId);
            for (uint i = 0; i < maxCount; i++)
            {
                var inventoryItem = _api.Inventory.GetContainerItem(InventoryContainerId, (int)i);
                if (inventoryItem.Id == 0)
                {
                    continue;
                }
                items.Add(inventoryItem);
            }

            return items;
        }
        public List<EliteAPI.InventoryItem> GetMatchingInventoryItemsFromContainer(string itemPattern, int inventoryContainerId = 0)
        {
            List<EliteAPI.InventoryItem> items = GetInventoryItemsFromContainer();
            var id = GetItemIdFromName(itemPattern, inventoryContainerId);
            if (id == null)
                return items;
            
            return items.FindAll(item => item.Id == (ushort) id) ;
        }
        public int GetCountOfItemsInContainer(string itemPattern, int InventoryContainerId = 0)
        {
            int count = 0;

            List<EliteAPI.IItem> items = new List<EliteAPI.IItem>();
            var maxCount = _api.Inventory.GetContainerMaxCount(InventoryContainerId);
            for (uint i = 0; i < maxCount; i++)
            {
                var hasItem = _api.Inventory.GetContainerItem(InventoryContainerId, (int)i);
                if (hasItem.Id == 0)
                {
                    continue;
                }
                
                var item = _api.Resources.GetItem(hasItem.Id);
                var itemName = item.Name[0];
                if (itemName.ToLower().Contains(itemPattern.ToLower()))
                {
                    count += (int)hasItem.Count;
                }
            }

            return count;
        }
        }
    }
}