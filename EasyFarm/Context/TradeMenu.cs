using System.Collections.Generic;
using EliteMMO.API;
using MemoryAPI;

namespace EasyFarm.Context
{
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
    }
}