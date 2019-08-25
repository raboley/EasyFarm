using MemoryAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EliteMMO.API.EliteAPI;

namespace EasyFarm.Classes
{

    public class Inventory : IInventory
    {
        //private static Inventory _instance = new Inventory();
        private IMemoryAPI _api;

        public Inventory(IMemoryAPI api)
        {
            this._api = api;
        }

        //public bool IsMoving { get; set; }

        //public static Inventory Instance
        //{
        //    get { return _instance = _instance ?? new Inventory(); }
        //    private set { _instance = value; }
        //}

        public bool InventoryIsFull(int InventoryContainerId = 0)
        {
            if (_api.Inventory.GetContainerCount(InventoryContainerId) == _api.Inventory.GetContainerMaxCount(InventoryContainerId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HaveItemInInventoryContainer(string itemPattern, int InventoryContainerId = 0)
        {
            var itemInInventory = GetMatchingItemsFromContainer(itemPattern);
            if (itemInInventory.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<IItem> GetMatchingItemsFromContainer(string itemPattern, int InventoryContainerId = 0)
        {
            List<IItem> items = GetItemsFromContainer();
            return items.Where(item => item.Name[0].ToLower().Contains(itemPattern.ToLower())).ToList();
        }

        private List<IItem> GetItemsFromContainer(int InventoryContainerId = 0)
        {
            List<IItem> items = new List<IItem>();
            var maxCount = _api.Inventory.GetContainerMaxCount(InventoryContainerId);
            for (uint i = 0; i < maxCount; i++)
            {
                var hasItem = _api.Inventory.GetContainerItem(InventoryContainerId, (int)i);
                if (hasItem.Id == 0)
                {
                    continue;
                }
                var item = _api.Resource.GetItem(hasItem.Id);
                items.Add(item);
            }

            return items;
        }

        public int GetCountOfItemsInContainer(string itemPattern, int InventoryContainerId = 0)
        {
            int count = 0;

            List<IItem> items = new List<IItem>();
            var maxCount = _api.Inventory.GetContainerMaxCount(InventoryContainerId);
            for (uint i = 0; i < maxCount; i++)
            {
                var hasItem = _api.Inventory.GetContainerItem(InventoryContainerId, (int)i);
                if (hasItem.Id == 0)
                {
                    continue;
                }
                
                var item = _api.Resource.GetItem(hasItem.Id);
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
