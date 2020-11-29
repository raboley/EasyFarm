using System.Collections.Generic;
using System.Threading;
using EasyFarm.Context;
using EliteMMO.API;

namespace EasyFarm.Missions.SandOria
{
    public static class TheMerchantsBidding
    {
        public class TheMerchantsBiddingCompletion : ICompleteRightNowCondition
        {
            public IGameContext Context;
        
            public bool CanCompleteNow()
            {
                if (Context.Traveler.CurrentZone.Map.MapName != "Southern_San_dOria")
                    return false;
                if (Context.Inventory.GetCountOfItemsInContainer("Rabbit Hide") < 3)
                    return false;

                return true;
            }
        }

        public class TradeRabbitHideToParvipon : BaseQuestStep
        {
            private string QuestGiverName { get; set; } = "Parvipon";
            
            public override void DoStep()
            {
                _traveler.WalkAcrossWorldToNpcByName(QuestGiverName); 
                if (_traveler.AmNotWithinTalkingDistanceToPersonByName(QuestGiverName))
                    return;
                
                var items = new List<ItemsToTrade>
                {
                    new ItemsToTrade
                    {
                        Name = "Rabbit Hide",
                        NumberToTrade = 3
                    }
                };

                _trade.TradeItemsToPersonByName(_context, QuestGiverName, items);
                
                // press enter until you see Obtained 120 gil.
                while (_chat.LastThingSaid().Contains("Thank you for the help"))
                {
                    Thread.Sleep(100);
                }
                
                _context.API.Windower.SendKeyPress(Keys.NUMPADENTER);
            }

            public TradeRabbitHideToParvipon(IGameContext context) : base(context)
            {
            }
        }
    }
}