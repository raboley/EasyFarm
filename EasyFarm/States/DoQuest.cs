using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EasyFarm.Context;
using EliteMMO.API;
using Pathfinder;
using Pathfinder.People;
using Player = EasyFarm.Classes.Player;

namespace EasyFarm.States
{
    public class DoQuest : BaseState
    {
        public override bool Check(IGameContext context)
        {
            if (context.Traveler?.CurrentZone?.Map == null)
                return false;

            if (new RestState().Check(context)) return false;

            if (new NeedSignet().Check(context)) return false;
            
            if (new SellSomeJunk().Check(context)) return false;

            if (context.Traveler.CurrentZone.Map.MapName == "Southern_San_dOria" &&
                context.Inventory.GetCountOfItemsInContainer("Rabbit Hide") > 3)
                return true;

            return false;
        }

        public override void Run(IGameContext context)
        {
            // Go to Parvipon
            var parvipon = context.Npcs.FirstOrDefault(x => x.Name == "Parvipon");
            context.Traveler.PathfindAndWalkToFarAwayWorldMapPosition(parvipon.Position);

            var distanceToParvipon = GridMath.GetDistancePos(context.Traveler.Walker.CurrentPosition,
                parvipon.Position);

            if (distanceToParvipon > 1)
                return;

            // target him
            TargetUnitFromPerson(context, parvipon);

            // Talk to him to make sure he will take rabbits
            // context.Navigator.InteractWithUnit(context, context.API, unitParvipon);
            // if You an adventurer?

            // press enter
            // choose I shall help.
            // press enter

            // if Still I await 3 rabbit hides

            const string itemToTradeName = "Rabbit Hide";
            const int itemToTradeQuantity = 3;
            var itemsToTrade = new List<Item>();
            
            var itemId = context.API.Inventory.GetItemIdFromName(itemToTradeName);
            if (itemId == null)
                return;
            
            
            itemsToTrade.Add(new Item()
            {
                Name = itemToTradeName,
                Count = itemToTradeQuantity
            });
            while (context.Inventory.GetCountOfItemsInContainer(itemToTradeName) >= itemToTradeQuantity)
            {
                // Trade 3 rabbit hides to him
                while (!context.API.Trade.IsTradeMenuOpen)
                {
                    TargetUnitFromPerson(context, parvipon);
                    Thread.Sleep(200);
                    context.API.Menu.OpenTradeMenu();
                }

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
                        ItemCount = (byte) item.Count,
                        ItemId = inventoryItem.Id,
                        ItemIndex = (byte) inventoryItem.Index
                    };

                    context.API.Trade.SetTradeItem(i, tradeItem);
                }

                context.API.Menu.ClickTrade();

                // press enter until you see Obtained 120 gil.
                while (!context.API.Chat.LastThingSaid().Contains("Thank you for the help"))
                {
                    Thread.Sleep(100);
                }
                
                context.API.Windower.SendKeyPress(Keys.NUMPADENTER);
            }
        }

        private static void TargetUnitFromPerson(IGameContext context, Person parvipon)
        {
            var unitParvipon = context.Memory.UnitService.GetUnitByName(parvipon.Name);
            context.Target = unitParvipon;
            Player.SetTarget(context.API, unitParvipon);
        }
    }
}