using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media.Imaging;
using EasyFarm.ViewModels;
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

        public bool CanAndShouldCraft(CraftingRecipe recipe)
        {
            if (!recipe.ShouldCraft())
                return false;


            if (!HaveAllMaterialsToCraft(recipe))
                return false;

            return true;
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
                var itemId = _context.Inventory.GetItemIdFromName(recipeItem.Name);
                if (itemId == null)
                    return false;

                var inventoryItem = itemsInInventory.FirstOrDefault(x => x.Id == (int) itemId);
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
            _context.Menu.OpenSynthesisMenu();
            ChooseCrystalSynthesis();

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


        public void OpenCraftingMenu()
        {
            OpenSynthesisMenu();
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

        public void GetSynthesisSupport(IGameContext context,
            StatusEffect supportType = StatusEffect.Woodworking_Imagery)
        {
            if (supportType == StatusEffect.Woodworking_Imagery)
            {
                if (!context.API.Player.StatusEffects.Contains(supportType))
                {
                    var gil = context.Inventory.GetGill();
                    if (gil > 100)
                        context.Dialog.HaveConversationWithPerson(context, "Ulycille",
                            new List<string> {"Advanced synthesis", "Accept"});
                }

                return;
            }
            
            LogViewModel.Write("I don't know how to get support for effect: " + supportType);
        }

        public void EatCraftSkillUpFood(IGameContext context)
        {
            if (!context.API.Player.StatusEffects.Contains(StatusEffect.Food))
            {
                var haveCraftSkillUpItems = context.Inventory.HaveItemInInventoryContainer("Macaron");
                if (haveCraftSkillUpItems)
                {
                    var macaron = "";

                    var cherryMacaron = "Cherry Macaron";
                    if (context.Inventory.HaveItemInInventoryContainer(cherryMacaron))
                        macaron = cherryMacaron;

                    var coffeeMacaron = "Coffee Macaron";
                    if (context.Inventory.HaveItemInInventoryContainer(coffeeMacaron))
                        macaron = coffeeMacaron;

                    var kitronMacaron = "Kitron Macaron";
                    if (context.Inventory.HaveItemInInventoryContainer(kitronMacaron))
                        macaron = kitronMacaron;

                    if (macaron != "")
                        context.API.Windower.SendString("/item \"" + macaron + "\" <me>");
                }
            } 
        }
    }
}