using System;
using System.Collections.Generic;
using System.Linq;
using EliteMMO.API;

namespace MemoryAPI.Memory.EliteMMOWrapper
{
    public class TradeMenuTools : ITradeMenuTools
    {
        private EliteAPI _api;

        public TradeMenuTools(EliteAPI api)
        {
            _api = api;
        }

        public EliteAPI.TradeItem GetTradeItem(int index)
        {
            var eliteTradeItem = _api.TradeMenu.GetTradeItem(index);
            var tradeItem = new EliteAPI.TradeItem
            {
                Index = index,
                ItemCount = eliteTradeItem.ItemCount,
                ItemId = eliteTradeItem.ItemId,
                ItemIndex = eliteTradeItem.ItemIndex
            };
            return tradeItem;
        }

        public bool SetTradeItem(int index, EliteAPI.TradeItem item)
        {
            return _api.TradeMenu.SetTradeItem(index, item);
        }

        public bool SetTradeItems(List<EliteAPI.TradeItem> items)
        {
            
            
            return items.Count <= 8 &&
                   !items.Where<EliteAPI.TradeItem>(
                           (Func<EliteAPI.TradeItem, int, bool>)
                           ((t, x) => !this.SetTradeItem(x, t)))
                       .Any<EliteAPI.TradeItem>();
        }

        public bool IsTradeMenuOpen => _api.TradeMenu.IsTradeMenuOpen;
    }
}