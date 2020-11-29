using System.Collections.Generic;

namespace EasyFarm.Context
{
    public interface IShop
    {
        void SelectSell();
        void SellAllJunk(List<string> junkItems);
    }
}