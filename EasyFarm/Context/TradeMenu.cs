using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EliteMMO.API;
using MemoryAPI;
using Player = EasyFarm.Classes.Player;

namespace EasyFarm.Context
{
    public class ItemsToTrade
    {
        public string Name { get; set; }
        public int NumberToTrade { get; set; }
    }

    public class TradeMenu : ITradeMenu
    {
        private IMemoryAPI _api;


        public TradeMenu(IMemoryAPI api)
        {
            _api = api;
        }

        public TradeItem GetTradeItem(int index)
        {
            var eliteTradeItem = _api.Trade.GetTradeItem(index);
            var tradeItem = new TradeItem()
            {
                Index = index,
                ItemIndex = eliteTradeItem.ItemIndex,
                ItemId = eliteTradeItem.ItemId,
                ItemCount = eliteTradeItem.ItemCount,
            };

            return tradeItem;
        }

        public bool SetTradeItem(int index, TradeItem item)
        {
            var tradeItem = NewEliteTradeItemFromTradeItem(index, item);

            return _api.Trade.SetTradeItem(index, tradeItem);
        }

        private static EliteAPI.TradeItem NewEliteTradeItemFromTradeItem(int index, TradeItem item)
        {
            EliteAPI.TradeItem tradeItem = new EliteAPI.TradeItem();
            tradeItem.Index = index;
            tradeItem.ItemCount = (byte) item.ItemCount;
            tradeItem.ItemId = (ushort) item.ItemId;
            tradeItem.ItemIndex = (byte) item.ItemIndex;
            return tradeItem;
        }

        public bool SetTradeItems(List<TradeItem> items)
        {
            if (items.Count > 8)
                return false;

            for (int i = 0; i < items.Count; i++)
            {
                var tradeItem = NewEliteTradeItemFromTradeItem(i, items[i]);

                var success = _api.Trade.SetTradeItem(i, tradeItem);
                if (!success)
                    return false;
            }

            return true;
        }

        public bool IsTradeMenuOpen => _api.Trade.IsTradeMenuOpen;

        public void TradeItemsToPersonByName(IGameContext context, string name, List<ItemsToTrade> itemsToTrade)
        {
            StartTradingWithPersonByName(context, name);
            AddItemsFromInventoryToTradeMenu(context, itemsToTrade);
            FinishTrade(context);
        }

        private static void FinishTrade(IGameContext context)
        {
            context.API.Menu.ClickTrade();
        }

        private static void AddItemsFromInventoryToTradeMenu(IGameContext context, List<ItemsToTrade> itemsToTrade)
        {
            var itemsInInventory = context.API.Inventory.GetInventoryItemsFromContainer();
            for (int i = 0; i < itemsToTrade.Count; i++)
            {
                var item = itemsToTrade[i];

                var inventoryItems = context.API.Inventory.GetMatchingInventoryItemsFromContainer(item.Name);
                if (inventoryItems.Count == 0)
                    continue;
                var inventoryItem = inventoryItems.First();

                var tradeItem = new EliteAPI.TradeItem
                {
                    Index = i,
                    ItemCount = (byte) item.NumberToTrade,
                    ItemId = inventoryItem.Id,
                    ItemIndex = (byte) inventoryItem.Index
                };

                context.API.Trade.SetTradeItem(i, tradeItem);
            }
        }

        private void StartTradingWithPersonByName(IGameContext context, string name)
        {
            while (!_api.Trade.IsTradeMenuOpen)
            {
                TargetUnitFromName(context, name);
                Thread.Sleep(200);
                context.API.Menu.OpenTradeMenu();
            }
        }

        private static void TargetUnitFromName(IGameContext context, string name)
        {
            var unit = context.Memory.UnitService.GetUnitByName(name);
            context.Target = unit;
            EasyFarm.Classes.Player.SetTarget(context.API, unit);
        }
    }
}