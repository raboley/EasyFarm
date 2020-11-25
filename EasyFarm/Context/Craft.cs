using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using MemoryAPI;
using Keys = EliteMMO.API.Keys;


namespace EasyFarm.Context
{
    public class Craft : MenuBase
    {
        private IMemoryAPI _context;

        public Craft(IMemoryAPI context) : base(context)
        {
            _context = context;
        }

        public bool HaveAllMaterialsToCraft(CraftingRecipe recipe)
        {
            if (!HasCrystal(recipe))
                return false;
            
            // have all items
            if (!HasAllItems(recipe))
                return false;

            return true;
        }

        private bool HasAllItems(CraftingRecipe recipe)
        {
            var itemsInInventory = _context.Inventory.GetInventoryItemsFromContainer();
            for (int i = 0; i < recipe.RequiredItems.Count; i++)
            {
                var recipeItem = recipe.RequiredItems[i];

                var item = _context.Inventory.GetMatchingItemsFromContainer(recipeItem.Name);
                if (item.Count == 0)
                    return false;
                var itemId = item[0].ItemID;
                
                var inventoryItem = itemsInInventory.FirstOrDefault(x => x.Id == itemId);
                if (inventoryItem == null)
                    return false;
                
                if (inventoryItem.Count < recipe.RequiredItems[i].Count)
                    return false;
            }

            return true;
        }

        private bool HasCrystal(CraftingRecipe recipe)
        {
            var crystals = _context.Inventory.GetMatchingItemsFromContainer(recipe.Crystal);
            if (crystals.Count == 0)
                return false;

            return true;
        }

        public void AttemptToCraft(CraftingRecipe recipe)
        {
            ResetMenu();
            OpenCraftingMenu();

            // Select Appropriate Crystal
            SelectCrystalToCraftWith(recipe.Crystal);
            // Add Ingredients

            AddIngredients(recipe.RequiredItems);

            // Click Craft
            ClickCraft();

            while (_context.Craft.IsCrafting)
            {
                Thread.Sleep(100);
            }

            CloseMenu();
            ResetMenu();
        }

        private void AddIngredients(List<Item> recipeRequiredItems)
        {
            //Figure Out the Item Ids
            var itemsInInventory = _context.Inventory.GetInventoryItemsFromContainer();

            for (int i = 0; i < recipeRequiredItems.Count; i++)
            {
                var recipeItem = recipeRequiredItems[i];

                var item = _context.Inventory.GetMatchingItemsFromContainer(recipeItem.Name);
                if (item.Count == 0)
                    return;


                var itemId = item[0].ItemID;
                var itemIndex = itemsInInventory.FirstOrDefault(x => x.Id == itemId)?.Index;
                if (itemIndex == null) return;

                _context.Craft.SetCraftItem(i, (int) itemId, (int) itemIndex, recipeRequiredItems[i].Count);
            }
        }



        private void OpenCraftingMenu()
        {
            OpenSynthesisMenu();
            ChooseCrystalSynthesis();
        }

        private void SelectCrystalToCraftWith(string crystal)
        {
            // Lightng. Crystal 
            while (_context.Inventory.SelectedItemName != crystal)
            {
                // Cant trust MenuItemCount because blanks are considered in the iteration
                var OriginalMenuIndex = 999;
                
                while (_context.Inventory.SelectedItemName != crystal &&
                       _context.Menu.MenuIndex != OriginalMenuIndex)
                {
                    OriginalMenuIndex = _context.Menu.MenuIndex;
                    Down();
                }

                OriginalMenuIndex = 999;
                while (_context.Inventory.SelectedItemName != crystal &&
                       _context.Menu.MenuIndex != OriginalMenuIndex)
                {
                    OriginalMenuIndex = _context.Menu.MenuIndex;
                    Up();
                }
            }


            Select();
        }
    }
}