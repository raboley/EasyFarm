using System.Collections.Generic;

namespace FinalFantasyXI.Context
{
    public interface IShop
    {
        void SelectSell();
        void SellAllJunk(List<string> junkItems);
        void SellAllJunkToMerchant(IGameContext context, string merchantName, List<string> junkItems);
    }
}