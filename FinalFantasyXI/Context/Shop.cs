using System;
using System.Collections.Generic;
using System.Threading;
using FinalFantasyXI.Classes;

namespace FinalFantasyXI.Context
{
    public class Shop : MenuBase, IShop
    {
        private int _sellMenuIndex = 2;

        public Shop(IMemoryAPI context) : base(context)
        {
            _context = context;
        }

        public void SellAllJunkToMerchant(IGameContext context, string merchantName, List<string> junkItems)
        {
            // Now that I should be close to merchant talk to them
            IMemoryAPI fface = context.API;
            IUnit npc = context.Memory.UnitService.GetClosestUnitByPartialName(merchantName);

            // Close menu in case player has menu open prior to trading.
            CloseMenu();

            context.Navigator.InteractWithUnit(context, fface, npc);

            SellAllJunk(junkItems);
        }

        public void SellAllJunk(List<string> junkItems)
        {
            SelectSell();
            // Go to the top of the shop list
            HighlightFirstItemInShopMenu();

            int itemsInCurrentInventory = _context.Inventory.GetContainerCount();
            for (int i = 0; i <= itemsInCurrentInventory+1; i++)
            {
                // Check if item is junk
                Thread.Sleep(1000);
                if (ItemIsAJunkItem(junkItems))
                {
                    if (!ShowPriceAndGetToSellOption())
                        continue;

                    // need to sell multiple
                    if (_context.Menu.MenuItemCount == 5)
                    {
                        ChooseToSellAll();
                    }

                    Sell();
                    // If we sold it, the cursor will move to the next item automatically
                    i++;
                    continue;
                }


                var lastIndex = _context.Inventory.SelectedItemIndex;
                var j = 0;
                while (_context.Inventory.SelectedItemIndex == lastIndex)
                {
                    Down();
                    Thread.Sleep(500);
                    j++;
                    if (j > 5)
                        break;
                }
            }

            CloseMenu();
        }

        public void SelectSell()
        {
            while (_context.Menu.IsMenuOpen != true)
                Thread.Sleep(100);

            while (_context.Menu.MenuIndex != _sellMenuIndex)
            {
                Down();
            }

            Enter();
        }


        private void HighlightFirstItemInShopMenu()
        {
            while (_context.Menu.MenuItemCount != 10)
                Thread.Sleep(100);

            while (_context.Menu.MenuIndex != 1)
                Up();
        }


        /// <summary>
        /// Allows for fuzzy matching if junk item name contains a * anywhere, (doesn't care where)
        /// </summary>
        /// <param name="junkItems"></param>
        /// <returns></returns>
        private bool ItemIsAJunkItem(List<string> junkItems)
        {
            if (!ItemIsInJunkItemsList(junkItems))
                return false;

            if (ItemIsCurrentlyEquipped())
                return false;

            return true;
        }

        private bool ItemIsInJunkItemsList(List<string> junkItems)
        {
            foreach (var junkItem in junkItems)
            {
                if (junkItem.Contains("*"))
                {
                    var wildcardJunkItem = junkItem.Replace("*", "");
                    if (_context.Inventory.SelectedItemName.ToLower().Contains(wildcardJunkItem.ToLower()))
                        return true;
                }

                if (String.Equals(_context.Inventory.SelectedItemName, junkItem,
                    StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }

            return false;
        }


        private bool ItemIsCurrentlyEquipped()
        {
            for (int i = 0; i < 17; i++)
            {
                var equippedItem = _context.Inventory.GetEquippedItem(i);
                if (equippedItem == null)
                    continue;

                if (equippedItem.Index == _context.Inventory.SelectedItemIndex)
                    return true;
            }

            return false;
        }

        private bool ShowPriceAndGetToSellOption()
        {
            var i = 0;
            while (_context.Menu.MenuItemCount == 10)
            {
                Enter();
                Thread.Sleep(500);
                i++;
                if (i > 5)
                    return false;
            }

            return true;
        }

        private void ChooseToSellAll()
        {
            while (_context.Inventory.ShopItemCount != _context.Inventory.ShopItemCountMax)
            {
                Left();
                Thread.Sleep(501);
            }
        }

        private void Sell()
        {
            var i = 0;
            while (_context.Menu.MenuIndex != 1)
            {
                Up();
                Thread.Sleep(1000);
                i++;
                if (i > 5)
                    return;
            }

            Enter();
            Thread.Sleep(1000);
        }
    }
}