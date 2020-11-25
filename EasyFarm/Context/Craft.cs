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
    public class CraftingRecipe
    {
        public static CraftingRecipe BronzeIngotFromGoblinMail()
        {
            var soup = new CraftingRecipe();
            soup.Crystal = "Lightng. Crystal";
            soup.RequiredItems = new List<Item>
            {
                new Item() {Name = "Goblin Mail", Count = 1},
            };

            return soup;
        }
        
        public static CraftingRecipe BronzeIngotFromGoblinHelmet()
        {
            var soup = new CraftingRecipe();
            soup.Crystal = "Lightng. Crystal";
            soup.RequiredItems = new List<Item>
            {
                new Item() {Name = "Goblin Helm", Count = 1},
            };

            return soup;
        }
        
        public static CraftingRecipe StoneSoup()
        {
            var soup = new CraftingRecipe();
            soup.Crystal = "Fire Crystal";
            soup.RequiredItems = new List<Item>
            {
                new Item() {Name = "Flint Stone", Count = 3},
                new Item() {Name = "Distilled Water", Count = 1}
            };

            return soup;
        }
        public static CraftingRecipe Hatchet()
        {
            var soup = new CraftingRecipe();
            soup.Crystal = "Fire Crystal";
            soup.RequiredItems = new List<Item>
            {
                new Item() {Name = "Bronze Ingot", Count = 2},
                new Item() {Name = "Maple Lumber", Count = 1}
            };

            return soup;
        }
        
        public static CraftingRecipe ArrowWoodLumber()
        {
            var soup = new CraftingRecipe();
            soup.Crystal = "Wind Crystal";
            soup.RequiredItems = new List<Item>
            {
                new Item() {Name = "Arrowwood Log", Count = 1},
            };

            return soup;
        }
        
        public static CraftingRecipe MapleLumber()
        {
            var soup = new CraftingRecipe();
            soup.Crystal = "Wind Crystal";
            soup.RequiredItems = new List<Item>
            {
                new Item() {Name = "Maple Log", Count = 1},
            };

            return soup;
        }

        public string Crystal { get; set; }
        public List<Item> RequiredItems { get; set; }
    }

    public class Item
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int Index { get; set; }
        public int Count { get; set; }
        public int Flag { get; set; }
        public int Price { get; set; }
        public byte[] Extra { get; set; }
    }

    public class Craft
    {
        private IMemoryAPI _context;

        public Craft(IMemoryAPI context)
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

        private void ClickCraft()
        {
            NavigateToTopRightCraftButton();
            Enter();
            Thread.Sleep(1000);
        }

        private void NavigateToTopRightCraftButton()
        {
            for (int i = 0; i < 5; i++)
            {
                Right();
            }
            Up();
        }

        private void Right()
        {
            _context.Windower.SendKeyPress(Keys.RIGHT);
            Thread.Sleep(100);
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

        private void CloseMenu()
        {
            if (_context.Craft.IsCraftMenuOpen)
            {
                _context.Windower.SendKeyPress(Keys.ESCAPE);
                Thread.Sleep(100);
                Enter();
            }
                
            
            while (_context.Menu.IsMenuOpen)
            {
                _context.Windower.SendKeyPress(Keys.ESCAPE);
                Thread.Sleep(100);
            }

            Thread.Sleep(100);
        }

        private void Enter()
        {
            _context.Windower.SendKeyPress(Keys.NUMPADENTER);
            Thread.Sleep(100);
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

        private void Up()
        {
            _context.Windower.SendKeyPress(Keys.UP);
            Thread.Sleep(100);
        }

        private void ChooseCrystalSynthesis()
        {
            while (_context.Menu.MenuItemCount > 2)
            {
                Thread.Sleep(10);
            }

            while (_context.Menu.MenuIndex != 1)
            {
                Down();
            }

            Thread.Sleep(100);

            Select();
        }

        private void Select()
        {
            _context.Windower.SendKeyPress(Keys.NUMPADENTER);
            Thread.Sleep(100);
        }

        private void OpenSynthesisMenu()
        {
            RefreshMenuInCaseSomewhereWeird();

            const int synthesisMenuIndex = 10;
            const string synthesisMenuHelpName = "Synthesis";

            while (_context.Menu.HelpName != synthesisMenuHelpName)
            {
                var startIndex = _context.Menu.MenuIndex;
                Down();
                while (startIndex != _context.Menu.MenuIndex)
                {
                    if (_context.Menu.HelpName == synthesisMenuHelpName)
                        break;
                    
                    Down();
                }
                
                if (_context.Menu.HelpName == synthesisMenuHelpName)
                    break;

                _context.Windower.SendKeyPress(Keys.RIGHT);
                Thread.Sleep(100);
            }

            Select();
        }

        private void Down()
        {
            _context.Windower.SendKeyPress(Keys.DOWN);
            Thread.Sleep(100);
        }

        private void RefreshMenuInCaseSomewhereWeird()
        {
            if (_context.Menu.IsMenuOpen)
                _context.Windower.SendKeyPress(Keys.MINUS);

            _context.Windower.SendKeyPress(Keys.MINUS);
        }

        private void ResetMenu()
        {
            _context.Windower.SendKeyUp(Keys.UP);
            _context.Windower.SendKeyUp(Keys.DOWN);
            _context.Windower.SendKeyUp(Keys.RIGHT);
            _context.Windower.SendKeyUp(Keys.ESCAPE);
        }
    }
}