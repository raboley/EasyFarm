using System.Collections.Generic;

namespace EasyFarm.Context
{
    public interface ITradeMenu
    {
        TradeItem GetTradeItem(int index);
        bool SetTradeItem(int index, TradeItem item);
        bool SetTradeItems(List<TradeItem> items);
        bool IsTradeMenuOpen { get; }
    }
}