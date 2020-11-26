using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using EasyFarm.Classes;
using EasyFarm.Context;
using EasyFarm.ViewModels;
using MemoryAPI;
using Pathfinder;
using Pathfinder.People;
using Pathfinder.Travel;

namespace EasyFarm.States
{
    public class SellSomeJunk : BaseState
    {
        public override bool Check(IGameContext context)
        {
            if (context.Traveler == null)
                return false;
            
            if (context.Traveler.Zoning)
                return false;

            if (new ZoneState().Check(context))
                return false;
            
            if (context.Inventory.InventoryIsFull()) 
                return true;

            if (context.Inventory.HaveItemInInventoryContainer("Yew Fishing Rod"))
                return true;

            return false;
        }

        public override void Run(IGameContext context)
        {
            LogViewModel.Write("Inventory is full, going to go Sell Junk!");
            
            List<string> junkItems = new List<string>();
            junkItems.Add("Orcish Axe");
            junkItems.Add("Zinc Ore");
            junkItems.Add("Earth Crystal");
            junkItems.Add("Fruit Seeds");
            junkItems.Add("Bind");
            junkItems.Add("Blind");
            junkItems.Add("*Fishing Rod");
            junkItems.Add("Shall Shell");
            junkItems.Add("Grass Cloth");
            junkItems.Add("Grass Thread");
            junkItems.Add("Rabbit Hide");
            junkItems.Add("Wild Onion");
            junkItems.Add("handful of pugil scales");
            
            
            var merchantName = "Phamelise";
            // var merchant = "Ostalie";
            
            // is merchant in zone?
            
            
            // Find Merchant
            var merchant = SearchWorldForPerson(context, merchantName);
            
            // Go to Zone of merchant
            while (context.API.Player.Zone.ToString() != merchant.MapName)
            {
                context.Traveler.GoToZone(merchant.MapName);
            }
            
            while (context.Traveler.Zoning)
            {
                Thread.Sleep(100);
            }
            
            if (merchant.MapName != context.Traveler.CurrentZone.Name)
                return;
            
            // Now that in the same zone, walk to merchant
            var talkDistance = 3;
            context.Traveler.PathfindAndWalkToFarAwayWorldMapPosition(merchant.Position, talkDistance);
            
            if (GridMath.GetDistancePos(context.Traveler.Walker.CurrentPosition, merchant.Position) > talkDistance)
                return;
            
            // Now that I should be close to merchant talk to them
            IMemoryAPI fface = context.API;
            IUnit npc = context.Memory.UnitService.GetClosestUnitByPartialName(merchantName);
            context.Navigator.InteractWithUnit(context, fface, npc);
            
            // Sell all junk
            context.Shop.SellAllJunk(junkItems);

        }
        public Person SearchWorldForPerson(IGameContext context, string personName)
        {
            while (context.NpcOverseer == null)
            {
                Debug.Write(
                    "SearchWorldForPerson is Waiting for NpcOverseer to be non null so it can use it to search all NPCs");
                Thread.Sleep(200);
            }

            List<Person> allPeople = context.NpcOverseer.GetAllPeople();
            var npc = allPeople.Find(p => p.Name.Contains(personName));

            return npc;
            
        }


    }
}