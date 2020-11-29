using System.Collections.Generic;
using EliteMMO.API;

namespace MemoryAPI.Memory.EliteMMOWrapper
{
    public interface ITradeMenuTools
    {
        EliteAPI.TradeItem GetTradeItem(int index);
        bool SetTradeItem(int index, EliteAPI.TradeItem item);
        bool SetTradeItems(List<EliteAPI.TradeItem> items);
        bool IsTradeMenuOpen { get; }
    }
}