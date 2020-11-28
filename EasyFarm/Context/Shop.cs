using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using EliteMMO.API;
using MemoryAPI;
using Xceed.Wpf.Toolkit.Primitives;

namespace EasyFarm.Context
{
    public class Shop : MenuBase, IShop
    {
        private int _sellMenuIndex = 2;

        public Shop(IMemoryAPI context) : base(context)
        {
            _context = context;
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

        public void SellAllJunk(List<string> junkItems)
        {
            SelectSell();
            // Go to the top of the shop list
            HighlightFirstItemInShopMenu();

            int maxInventorySize = 80;
            for (int i = 0; i < maxInventorySize; i++)
            {
                if (ItemIsAJunkItem(junkItems))
                {
                    if (_context.Inventory.ShopItemCountMax > 1)
                    {
                        // If we have multiple, have to select how many.
                        SellMultiple();
                    }

                    Sell();

                    // If we sold it, the cursor will move to the next item automatically
                    Thread.Sleep(500);
                    continue;
                }

                Down();
            }

            CloseMenu();
        }

        /// <summary>
        /// Allows for fuzzy matching if junk item name contains a * anywhere, (doesn't care where)
        /// </summary>
        /// <param name="junkItems"></param>
        /// <returns></returns>
        private bool ItemIsAJunkItem(List<string> junkItems)
        {
            foreach (var junkItem in junkItems)
            {
                if (junkItem.Contains("*"))
                {
                    var wildcardJunkItem = junkItem.Replace("*", "");
                    if (_context.Inventory.SelectedItemName.Contains(wildcardJunkItem))
                        return true;
                }

                if (_context.Inventory.SelectedItemName == junkItem)
                    return true;
            }

            return false;
        }

        private void SellMultiple()
        {
            GetToHowManyToSellMenu();
            ChooseToSellAll();
            Sell();
        }

        private void Sell()
        {
            while (_context.Menu.MenuItemCount != 2)
            {
                Enter();
                Thread.Sleep(1000);
            }

            while (_context.Menu.MenuIndex != 1)
            {
                Up();
                Thread.Sleep(1000);
            }

            Enter();
            Thread.Sleep(1000);
        }

        private void ChooseToSellAll()
        {
            Thread.Sleep(501);
            Left();
        }

        private void GetToHowManyToSellMenu()
        {
            // The four directions for increasing and decreasing price + 1 for typing in the number seem to be the trick.
            // Have to do a while loop because we don't know if the price is visible or not.
            int MenuItemCountForSellItem = 5;
            while (_context.Menu.MenuItemCount != MenuItemCountForSellItem)
            {
                Enter();
            }
        }


        private void HighlightFirstItemInShopMenu()
        {
            while (_context.Menu.MenuItemCount != 10)
                Thread.Sleep(100);

            while (_context.Menu.MenuIndex != 1)
                Up();
        }
    }
}