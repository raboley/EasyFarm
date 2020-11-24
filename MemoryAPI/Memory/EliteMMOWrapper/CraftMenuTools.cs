using System.Collections.Generic;
using EliteMMO.API;

namespace MemoryAPI.Memory.EliteMMOWrapper
{
    public class CraftMenuTools : ICraftMenuTools
    {
        private readonly EliteAPI _api;

        public CraftMenuTools(EliteAPI api)
        {
            _api = api;
                
        }
        
        public CraftItem GetCraftItem(int index)
        {
            var eliteCraftItem = _api.CraftMenu.GetCraftItem(index);
            var craftItem = eliteCraftItemToCraftItem(eliteCraftItem);
            return craftItem;
        }

        private static CraftItem eliteCraftItemToCraftItem(EliteAPI.CraftItem eliteCraftItem)
        {
            var craftItem = new CraftItem()
            {
                Index = eliteCraftItem.Index,
                ItemId = eliteCraftItem.ItemId,
                Count = eliteCraftItem.Count
            };
            return craftItem;
        }

        public List<CraftItem> GetCraftItems()
        {
            var eliteCraftItems = _api.CraftMenu.GetCraftItems();
            var craftItems = new List<CraftItem>();
            
            if (eliteCraftItems.Count == 0)
                return craftItems;

            foreach (EliteAPI.CraftItem eliteCraftItem in eliteCraftItems)
            {
               craftItems.Add(eliteCraftItemToCraftItem(eliteCraftItem)); 
            }

            return craftItems;
        }

        public bool SetCraftItem(int index, int itemId, int itemIndex, int itemCount)
        {
            return _api.CraftMenu.SetCraftItem(index, (ushort) itemId, (byte) itemIndex, (byte) itemCount);
        }

        public bool IsCraftMenuOpen => _api.CraftMenu.IsCraftMenuOpen;

        public bool IsCrafting => _api.CraftMenu.IsCrafting;
        
    }
    
    public class CraftItem
    {
        public int Index { get; set; }

        public int ItemId { get; set; }

        public int Count { get; set; }
    }
}