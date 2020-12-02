using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using EasyFarm.Classes;
using EasyFarm.Context;
using EliteMMO.API;
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

            if (context.Player.Nation == Nations.SandOria.ToString())
                Objectives = SandoriaCalling();
        }

        public List<Objective> Objectives { get; set; }

        public bool CanDo()
        {
            return true;
        }

        public void Do()
        {
            foreach (var objective in Objectives)
            {
                if (objective.ShouldDo(_context))
                {
                    objective.Do(_context);
                    break;
                }
            }
        }

        private List<Objective> SandoriaCalling()
        {
            // Get to lvl 3 fighting rabbits in east ronafare
            var levelUpToFive = new Objective
            {
                ShouldDo = context => context.Player.JobLevel <= 5,
                Do = context => FightRabbitsInEastRon(context),
                Done = context => context.Player.JobLevel > 5
            };

            var levelUpToFifteen = new Objective
            {
                Done = context => context.Player.JobLevel > 15,

                ShouldDo = context =>
                {
                    if (!levelUpToFive.Done(context))
                        return false;

                    // Might change this, but only doing this for leveling right now.
                    if (context.Player.JobLevel > 15)
                        return false;

                    return true;
                    // return context.Inventory.HaveItemInInventoryContainer("Hatchet");
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
                        mobsToFight.Add("hare");

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

            var levelUpToTwentyFour = new Objective
            {
                Done = context => context.Player.JobLevel > 23,
                ShouldDo = context =>
                {
                    if (!levelUpToFifteen.Done(context))
                        return false;

                    if (context.Player.JobLevel > 23)
                        return false;

                    return true;
                },
                Do = context =>
                {
                    var targetZone = Zone.La_Theine_Plateau.ToString();
                    string purpose = "";

                    var mobsToFight = new List<string>();

                    if (context.Player.JobLevel < 19)
                    {
                        purpose = "levelUpToNineTeen";
                        mobsToFight.Add("wasp");
                        mobsToFight.Add("orc");
                        mobsToFight.Add("Hare");
                        // Adding this for a chance at wind crystals
                        mobsToFight.Add("Bat");
                    }

                    if (context.Player.JobLevel >= 19)
                    {
                        purpose = "levelUpPastNineteen";
                        mobsToFight.Add("Thickshell");
                        mobsToFight.Add("Funguar");
                        mobsToFight.Add("Sheep");
                        mobsToFight.Add("Neckchopper");
                        mobsToFight.Add("Stonechucker");
                        mobsToFight.Add("Akaba");
                    }


                    _context.WoodChopper.LoopOverMobsInZoneMatchingList(context, mobsToFight, targetZone, purpose);
                }
            };

            var levelUpToThirtyThree = new Objective
            {
                ShouldDo = context =>
                {
                    if (!levelUpToTwentyFour.Done(context))
                        return false;

                    if (context.Player.JobLevel > 32)
                        return false;

                    return true;
                },
                Do = context =>
                {
                    var targetZone = Zone.Valkurm_Dunes.ToString();
                    var purpose = "LevelUpTo33";

                    var mobsToFight = new List<string>();
                    mobsToFight.Add("Hare");
                    mobsToFight.Add("Lizard");
                    // Adding this for a chance at wind crystals
                    mobsToFight.Add("Bat");

                    // TODO: Make this a center point for the entrance to the zone from k. highlands.
                    _context.WoodChopper.LoopOverMobsInZoneMatchingList(context, mobsToFight, targetZone, purpose);
                }
            };


            var CampJagedyEaredJack = new Objective
            {
                ShouldDo = context =>
                    context.Player.JobLevel > 30 && !context.Inventory.HaveItemInInventoryContainer("Rabbit charm") &&
                    context.Player.Job == Job.Thief,
                Do = context => FarmJagedyEaredJack(context)
            };

            var objectives = new List<Objective>();
            objectives.Add(levelUpToFive);
            objectives.Add(levelUpToFifteen);
            objectives.Add(levelUpToTwentyFour);
            objectives.Add(levelUpToThirtyThree);
            objectives.Add(CampJagedyEaredJack);

            return objectives;
        }


        private void FarmJagedyEaredJack(IGameContext context)
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

            context.WoodChopper.LoopOverMobsWithinDistanceOfPoint(context, mobsToFight, targetZone, purpose,
                centerPoint, distance);
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


            _context.WoodChopper.LoopOverMobsWithinDistanceOfPoint(context, mobsToFight, targetZone, purpose,
                centerPoint, distance);
        }
    }
}