using System.Collections.Generic;

namespace FinalFantasyXI.Context
{
    public interface ITradeMenu
    {
        TradeItem GetTradeItem(int index);
        bool SetTradeItem(int index, TradeItem item);
        bool SetTradeItems(List<TradeItem> items);
        bool IsTradeMenuOpen { get; }
        void TradeItemsToPersonByName(IGameContext context, string name, List<ItemsToTrade> itemsToTrade);
    }
}