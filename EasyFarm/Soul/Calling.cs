using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using EasyFarm.Classes;
using EasyFarm.Context;
using MemoryAPI;
using MemoryAPI.Chat;
using Pathfinder;
using Pathfinder.People;
using Pathfinder.Travel;

namespace EasyFarm.Soul
{
    public class Objective
    {
        public Func<IGameContext, bool> ShouldDo { get; set; }
        public Func<IGameContext, bool> Done { get; set; }

        // Action is the same as func, but it doesn't have a return.
        public Action<IGameContext> Do { get; set; }
    }

    public class Calling : ICalling
    {
        private IGameContext _context;

        protected Traveler _traveler
        {
            get => _context.Traveler;
        }

        protected IDialog _talk;
        protected ITradeMenu _trade;
        protected IChatTools _chat;
        protected IInventory _inventory;

        public Calling(IGameContext context)
        {
            _context = context;
            _talk = context.Dialog;
            _trade = context.Trade;
            _chat = context.API.Chat;
            _inventory = context.Inventory;
        }

        public bool CanDo()
        {
            return true;
        }

        public void Do()
        {
            // Get to lvl 3 fighting rabbits in east ronafare
            var levelUpToFive = new Objective
            {
                ShouldDo = context => context.Player.JobLevel <= 5,
                Do = context => FightRabbitsInEastRon(context)
            };
            if (levelUpToFive.ShouldDo(_context))
                levelUpToFive.Do(_context);

            // Get to lvl 5
            // if (_context.Player.JobLevel > 5 && _context.Player.JobLevel <= 15)


            if (_context.Player.JobLevel > 30 && !_context.Inventory.HaveItemInInventoryContainer("Rabbit charm"))
            {
                FarmJagedyEaredJack();
                return;
            }

            var chopWoodInRon = new Objective
            {
                Done = context => context.Player.JobLevel > 14,

                ShouldDo = context =>
                {
                    if (context.Player.JobLevel <= 5)
                        return false;

                    // Might change this, but only doing this for leveling right now.
                    if (context.Player.JobLevel > 14)
                        return false;
                    return context.Inventory.HaveItemInInventoryContainer("Hatchet");
                },
                Do = context =>
                {
                    var resourceName = "Logging Point";
                    var chopWoodZone = Zone.Ronfaure_East.ToString();
                    var purpose = "ChopWoodInRon";
                    var mobsToFight = new List<string>();

                    if (context.Player.JobLevel < 16)
                    {
                        // Ground crystals
                        // mobsToFight.Add("Rabbit");

                        // Wind Crystals
                        mobsToFight.Add("bat");
                    }

                    // And Fire Crystals
                    // I want to steal some beastcoins
                    mobsToFight.Add("orc");
                    // Get some grass thread
                    mobsToFight.Add("weaver");

                    context.WoodChopper.SetMobsToTarget(context, mobsToFight);


                    context.WoodChopper.ChopTreesInZone(context, chopWoodZone, purpose, resourceName);
                }
            };

            if (chopWoodInRon.ShouldDo(_context))
                chopWoodInRon.Do(_context);

            var levelUpPastFifteen = new Objective
            {
                ShouldDo = context =>
                {
                    if (!chopWoodInRon.Done(context))
                        return false;

                    if (context.Player.JobLevel > 21)
                        return false;

                    return true;
                },
                Do = context =>
                {
                    var targetZone = Zone.La_Theine_Plateau.ToString();
                    var purpose = "LevelUpPast15";

                    var mobsToFight = new List<string>();
                    mobsToFight.Add("Hare");
                    mobsToFight.Add("orc");
                    // Adding this for a chance at wind crystals
                    mobsToFight.Add("Bat");

                    _context.WoodChopper.LoopOverMobsInZoneMatchingList(context, mobsToFight, targetZone, purpose); 
                }
                
                
            };

            if (levelUpPastFifteen.ShouldDo(_context))
                levelUpPastFifteen.Do(_context);
        }

        private void FarmJagedyEaredJack()
        {
            var targetZone = Zone.Ronfaure_West.ToString();
            var purpose = "FarmJEJ";

            var mobsToFight = new List<string>();
            mobsToFight.Add("Hare");
            mobsToFight.Add("Jack");
            // I want to steal some beastcoins
            mobsToFight.Add("orc");
            // Get some grass thread
            mobsToFight.Add("weaver");

            var centerPoint = new Vector3(-268, 0, -257);
            var distance = 30;

            _context.WoodChopper.LoopOverMobsWithinDistanceOfPoint(_context, mobsToFight, targetZone, purpose, centerPoint, distance);
        }

        private void FightBatsInTomb()
        {
        }

        private void FightRabbitsInEastRon(IGameContext context)
        {
            var targetZone = Zone.Ronfaure_East.ToString();
            var purpose = "LevelTo6";

            var mobsToFight = new List<string>();
            mobsToFight.Add("Hare");
            mobsToFight.Add("Rabbit");
            // Adding this for a chance at wind crystals
            mobsToFight.Add("Bat");

            var centerPoint = new Vector3(79, 0, 275);
            var distance = 300;


            _context.WoodChopper.LoopOverMobsWithinDistanceOfPoint(context, mobsToFight, targetZone, purpose, centerPoint, distance);

            // Run to Ranfare's tomb.
        }

        // private void LoopOverMobsInList(List<string> mobsToFight, string targetZone, string purpose, Vector3 centerPoint, int distance)
        // {
        //     _context.WoodChopper.SetMobsToTarget(_context, mobsToFight);
        //
        //     _context.WoodChopper.ChopWoodZone = targetZone;
        //     if (!_context.WoodChopper.TryToGoToTargetZone(_context))
        //         return;
        //
        //
        //     if (_context.WoodChopper.Purpose != purpose || _context.WoodChopper.LoggingPoints.Count == 0)
        //     {
        //         _context.WoodChopper.Purpose = purpose;
        //         _context.WoodChopper.SetAllMobsWithinDistanceOfPointToLoggingPoints(_context, mobsToFight, centerPoint,
        //             distance);
        //     }
        //
        //     if (_context.WoodChopper.LoggingPoints.IsEmpty)
        //         return;
        //
        //     if (_context.WoodChopper.NextPoint == null)
        //         _context.WoodChopper.SetNextPoint();
        //
        //     if (_context.WoodChopper.NextPoint != null)
        //         _traveler.PathfindAndWalkToFarAwayWorldMapPosition(_context.WoodChopper.NextPoint.Position);
        //
        //     _context.WoodChopper.SetNextPointIfHasBeenReached(_traveler.Walker.CurrentPosition);
        // }
    }
}