using System.Collections.Generic;
using EasyFarm.Context;

namespace EasyFarm.Missions.SandOria
{
    public static class QuestFactory
    {
        public static IQuest TheMerchantsBidding(IGameContext context)
        {
            var todoList = new List<IQuestStep>
            {
                new TheMerchantsBidding.FightRabbitsToGetHide(context),
                new TheMerchantsBidding.TradeRabbitHideToParvipon(context)
            };

            var completion = new TheMerchantsBidding.TheMerchantsBiddingCompletion {Context = context};

            return new Quest(todoList, completion);
        }
    }
}