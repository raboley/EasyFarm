using EasyFarm.Classes;
using EasyFarm.Context;
using EasyFarm.Logging;
using EasyFarm.Persistence;
using EasyFarm.UserSettings;
using EasyFarm.ViewModels;
using MemoryAPI;
using MemoryAPI.Navigation;
using MemoryAPI.Windower;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MemoryAPI.Memory.EliteMmoWrapper;

namespace EasyFarm.States
{
    class DumpTreasureState : BaseState
    {
        private const string routePathRoot = "D:\\Users\\Russell\\Source\\Repos\\EasyFarm\\EasyFarm\\bin\\Debug\\";
        public const int homepointDelayConstant = 1000;
        //private const double TooCloseDistance = 1.5;
        //private IWindowerTools windower;
        //private IMemoryAPI fface;

        //public DumpTreasureState(IGameContext context)
        //{
        //    fface = context.API;
        //    windower = context.API.Windower;
        //}

        //private DateTime? _lastTargetCheck;
        

        public override bool Check(IGameContext context)
        {
            // dump items button was pressed
            if (context.Config.ShouldDumpItemsNowButtonPressed == true)
            {
                context.Config.ShouldDumpItemsNowButtonPressed = false;
                return true;
            }
            
            LogViewModel.Write(context.Memory.EliteApi.Menu.HelpName);
            
            // return if
            bool dumpItems = context.Config.EnableDumpItemsAtBastok;
            if (dumpItems == true)
            {
            // inventory is full
            //// merit points are full
            //    int meritPoints = context.API.Player.MeritPoints;
            //    if ( meritPoints >= 50)
            //    {
            //        return true;
            //    }
            //    // signet wore off
            //    // rhapsody points are full
            //    // unity points are full

                // every hour
                string minutes = DateTime.Now.ToString("mm");
                if (minutes == context.Config.TimeToDumpInMinutes.ToString())
                {
                    return true;
                }
            }

            //Debug.Write($"The menu help name is: {context.Memory.EliteApi.Menu.HelpName} and index is: {context.Memory.EliteApi.Menu.MenuIndex}" + Environment.NewLine);
            //TimeWaiter.Pause(1000);

            // debug
            //context.API.Navigator.DistanceTolerance = 3;
            IMemoryAPI fface = context.API;
            ConvertSparksToGil(context, fface);
            //// go to goldsmith door
            //TravelPath(context, routePathRoot + "_homepoint4_to_goldsmith_door.ewl");

            //// open door
            //OpenGoldsmithDoor(context, fface);

            //// go to ephemeral moogle
            //TravelPath(context, routePathRoot + "_goldsmith_door_to_ephemeral_moogle.ewl");

            //// go to merchent
            //TravelPath(context, routePathRoot + "_ephemeral_moogle_to_teerth.ewl");

            //// go to door
            //TravelPath(context, routePathRoot + "_teerth_to_goldsmith_door.ewl");

            //// open door
            //OpenGoldsmithDoor(context, fface);

            //// go to crystal
            //TravelPath(context, routePathRoot + "_goldsmith_doort_to_homepoint4.ewl");

            return false;
        }

        public override void Run(IGameContext context)
        {
            IMemoryAPI fface = context.API;
            //_settings = new SettingsManager("ewl", "EasyFarm Waypoint List");
            string minutes = DateTime.Now.ToString("mm");

            LogViewModel.Write("Dumping items to storage and stuff...");

            WarpHome(fface, context);
            SpendMeritPoints(context, fface);
            ExitMogHouseWarpToGoldsmith(context, fface);
            StoreCrystalsAndSellItemsToGoldsmith(context, fface);
            TravelToHomepointOne(context, fface);
            ConvertSparksToGil(context, fface);
            GetSignet(context, fface);
            SpendUnityPointsAndWarpToBoyda(context, fface);

            SummonTrustsAndGoInvis(fface);

            //// walk to hard mobs zone
            TravelPath(context, routePathRoot + "_boyda_to_jp_route.ewl");
            
            // start bot
            LoadRoute("C:\\Users\\Russell\\Desktop\\Release\\boyda_route_jp.ewl");
        }

        private static void SummonTrustsAndGoInvis(IMemoryAPI fface)
        {
            // summon trusts
            SummonTrust(fface, "'Zeid II'");
            SummonTrust(fface, "'Adelheid'");
            SummonTrust(fface, "'Mihli Aliapoh'");
            SummonTrust(fface, "'Valaineral'");
            SummonTrust(fface, "'Joachim'");

            //use spectral jig
            fface.Windower.SendString($"/ja 'Spectral Jig' <me>");
        }

        private void SpendUnityPointsAndWarpToBoyda(IGameContext context, IMemoryAPI fface)
        {

            // go to warp guy
            TravelPath(context, routePathRoot + "_rabid_wolf_to_igsli.ewl");
            MoveForwardForSeconds(context, 2);

            // buy all gobbie keys
            IUnit igsli = new NullUnit
            {
                Id = 185
            };
            BuyAllGobbieKeys(context, fface, igsli);

            // warp to boyda
            WarpToBoyahdaTree(context, fface, igsli);
            WaitForZone(fface, context);
        }

        private void GetSignet(IGameContext context, IMemoryAPI fface)
        {

            // go to merchant door
            TravelPath(context, routePathRoot + "_olwyn_to_mjol_door.ewl");

            // open door
            OpenMjollsGoodsDoor(context, fface);

            // go to signet guy
            TravelPath(context, routePathRoot + "_mjol_door_to_rabid_wolf.ewl");
            IUnit rabidWolfIM = new NullUnit
            {
                Id = 52
            };
            TimeWaiter.Pause(100);
            context.API.Navigator.GotoNPC(rabidWolfIM.Id, context.Config.IsObjectAvoidanceEnabled);

            // get signet
            GetSignetFromRabidWolf(context, fface);
        }

        private void TravelToHomepointOne(IGameContext context, IMemoryAPI fface)
        {
            // go to door
            TravelPath(context, routePathRoot + "_teerth_to_goldsmith_door.ewl");

            // open door
            OpenGoldsmithDoor(context, fface);

            // go to crystal
            TravelPath(context, routePathRoot + "_goldsmith_doort_to_homepoint4.ewl");

            // warp to markets entrance
            WarpToEntrance(context, fface);
            WaitForZone(fface, context);
        }

        private void ConvertSparksToGil(IGameContext context, IMemoryAPI fface)
        {

           

            // go to isakoth
            TravelPath(context, routePathRoot + "_homepoint1_to_isakoth.ewl");
            IUnit Isakoth = new NullUnit
            {
                Id = 177
            };
            TimeWaiter.Pause(100);
            context.API.Navigator.GotoNPC(Isakoth.Id, context.Config.IsObjectAvoidanceEnabled);

            //MoveForwardForSeconds(context, 5);

            //// buy all acheron shields
            BuyAllAcheronShields(fface);

            // go to merchant door
            TravelPath(context, routePathRoot + "_isakoth_to_mjol_door.ewl");

            // open door
            OpenMjollsGoodsDoor(context, fface);

            // go to merchant
            TravelPath(context, routePathRoot + "_mjol_door_to_olwyn.ewl");
            MoveForwardForSeconds(context, 2);

            // sell all things
            IUnit olwyn = new NullUnit
            {
                Id = 29
            };
            SellAllJunk(context, fface, olwyn);
        }

        private static void MoveForwardForSeconds(IGameContext context, int seconds)
        {
            for (int i = 0; i < seconds; i++)
            {
                context.API.Windower.SendKeyPress(EliteMMO.API.Keys.NUMPAD8);
                TimeWaiter.Pause(1000);
            }
        }

        private void StoreCrystalsAndSellItemsToGoldsmith(IGameContext context, IMemoryAPI fface)
        {
            
            // go to goldsmith door
            TravelPath(context, routePathRoot + "_homepoint4_to_goldsmith_door.ewl");

            // open door
            OpenGoldsmithDoor(context, fface);

            // go to ephemeral moogle
            TravelPath(context, routePathRoot + "_goldsmith_door_to_ephemeral_moogle.ewl");

            // drop off crystals
            for (int i = 3 - 1; i >= 0; i--)
            {
                StoreCrystalsAtMoogle(context, fface);
            }

            // deposit and sort inventories (sack and stuff)
            StoreGoodsAndDropTrash(fface);

            // go to merchent
            TravelPath(context, routePathRoot + "_ephemeral_moogle_to_teerth.ewl");

            // sell all things
            IUnit teerth = new NullUnit
            {
                Id = 12
            };
            SellAllJunk(context, fface, teerth);
        }

        private void ExitMogHouseWarpToGoldsmith(IGameContext context, IMemoryAPI fface)
        {
            // exit mog house
            ExitMogHouse(context);

            // Wait for zone to load
            WaitForZone(fface, context);

            // go to warp crystal 
            string MogHouseTohomepoint3 = routePathRoot + "_moghouse_to_homepoint3.ewl";
            TravelPath(context, MogHouseTohomepoint3);
            MoveForwardForSeconds(context, 10);
            // warp to goldsmith

            WarpToGoldsmith(context, fface);
            WaitForZone(fface, context);
        }

        private void SpendMeritPoints(IGameContext context, IMemoryAPI fface)
        {
            // go to mog house
            string homepoint3ToMogHouse = routePathRoot + "_homepoint3_to_moghouse.ewl";
            TravelPath(context, homepoint3ToMogHouse);

            for (int i = 0; i < 10; i++)
            {
                context.API.Windower.SendKeyPress(EliteMMO.API.Keys.NUMPAD8);
                TimeWaiter.Pause(100);
            }

            WaitForZone(fface, context);
            // spend merit points
            SpendMeritPointToMaxMeritPoints(context);
        }

        private void WarpHome(IMemoryAPI fface, IGameContext context)
        {
            Position zero = new Position { H = 0, X = 0, Y = 0, Z = 0 };
            context.API.Navigator.Reset();
            // waiting for the zoning to start
            while (DistanceTo(fface, zero) != 0)
            {
            // use warp ring
                fface.Windower.SendString("/item 'Warp Ring' <me>");
                TimeWaiter.Pause(15000);
            }
            // waiting for zoning to end
            while (DistanceTo(fface, zero) == 0)
            {
                TimeWaiter.Pause(100);
            }
            // just because we got a pos, doesn't mean things are loaded.
            TimeWaiter.Pause(10000);
        }

        private static void SummonTrust(IMemoryAPI fface, string trust)
        {
            fface.Windower.SendString($"/ma {trust} <me>");
            TimeWaiter.Pause(6000);
        }

        private void WaitForZone(IMemoryAPI fface, IGameContext context)
        {

            TimeWaiter.Pause(12000);
            Position zero = new Position { H = 0, X = 0, Y = 0, Z = 0 };
            
            // waiting for the zoning to start

            // waiting for zoning to end
            while (DistanceTo(fface, zero) == 0)
            {
                TimeWaiter.Pause(100);
            }
            // just because we got a pos, doesn't mean things are loaded.
            TimeWaiter.Pause(10000);
        }

        private static void ExitMogHouse(IGameContext context)
        {
            while (context.Memory.EliteApi.Player.Position.H <= 1.25 || context.Memory.EliteApi.Player.Position.H >= 1.75)
            {
                context.API.Windower.SendKeyPress(EliteMMO.API.Keys.A);
            }

            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.TAB);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.LEFT);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
        }

        private static void SpendMeritPointToMaxMeritPoints(IGameContext context)
        {
            //// Navigate to merit point menu for maximum merit points
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.SUBTRACT);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.DOWN);
            TimeWaiter.Pause(100);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.DOWN);
            //// add a merit into maximum merit points
            TimeWaiter.Pause(100);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.UP);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.LEFT);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            //// exit all the menus
            ExitMenus(context);
        }

        private static void ExitMenus(IGameContext context)
        {
            for (int i = 0; i < 7; i++)
            {
                TimeWaiter.Pause(500);
                context.API.Windower.SendKeyPress(EliteMMO.API.Keys.ESCAPE);
            }
        }

        private void TravelPath(IGameContext context, string routePath)
        {
            LoadRoute(routePath);
            StartRoute(context);

            for (int i = 0; i < 5; i++)
            {
                context.API.Windower.SendKeyPress(EliteMMO.API.Keys.NUMPAD8);
                TimeWaiter.Pause(100);
            }

            Config.Instance.Route.Reset();
            context.API.Navigator.Reset();
        }

        private void LoadRoute(string routePath)
        {
            var route = LoadPath<Route>(routePath);
            var isRouteLoaded = route != null;

            if (isRouteLoaded)
            {
                Config.Instance.Route = route;
                AppServices.InformUser("Path has been loaded.");
            }
            else
            {
                AppServices.InformUser("Failed to load the path.");
            }
        }

        private T LoadPath<T>(string path)
        {
            if (!File.Exists(path))
            {
                return default(T);
            }

            try
            {
                return Serialization.Deserialize<T>(path);
            }
            catch (InvalidOperationException ex)
            {
                Logger.Log(new LogEntry(LoggingEventType.Error, $"{GetType()}: Failure on de-serialize settings", ex));
                return default(T);
            }
        }

        private static void StartRoute(IGameContext context)
        {
            var shouldKeepRunningToNextWaypoint = context.Config.Route.Waypoints.Count != 1;

            while (shouldKeepRunningToNextWaypoint)
            {
                //context.API.Navigator.DistanceTolerance = 1; // maybe lower this so that the mjol section doesn't suck so bad.

                var nextPosition = context.Config.Route.GetNextPositionPath(context.API.Player.Position);
                if (nextPosition == null)
                {
                    shouldKeepRunningToNextWaypoint = false;
                }
                else
                {

                    context.API.Navigator.GotoWaypoint(
                        nextPosition,
                        context.Config.IsObjectAvoidanceEnabled,
                        shouldKeepRunningToNextWaypoint);
                }
            }
        }

        private static void WarpToBoyahdaTree(IGameContext context, IMemoryAPI fface, IUnit igsli)
        {
            InteractWithUnit(context, fface, igsli);
            TimeWaiter.Pause(4000);
            // teleport menu
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.DOWN);
            TimeWaiter.Pause(500);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            TimeWaiter.Pause(1000);
            // content level 135
            for (int i = 0; i < 7; i++)
            {
                context.API.Windower.SendKeyPress(EliteMMO.API.Keys.DOWN);
                TimeWaiter.Pause(500);
            }
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.DOWN);
            TimeWaiter.Pause(500);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.DOWN);
            TimeWaiter.Pause(500);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            // if I don't have enough unity points there is another prompt to go through
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.LEFT);
            TimeWaiter.Pause(100);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
        }

        private static void BuyAllGobbieKeys(IGameContext context, IMemoryAPI fface, IUnit igsli)
        {
            InteractWithUnit(context, fface, igsli);
            TimeWaiter.Pause(3000);
            // navigate to buy some items
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.DOWN);
            TimeWaiter.Pause(500);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.DOWN);
            TimeWaiter.Pause(500);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.DOWN);
            TimeWaiter.Pause(500);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            TimeWaiter.Pause(1000);
            // navigate to gobbie key
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.DOWN);
            TimeWaiter.Pause(500);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.DOWN);
            TimeWaiter.Pause(500);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            TimeWaiter.Pause(2000);
            // at the prompt select to buy 6 keys
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys._6);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            // go through all the confirm prompts
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.UP);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            TimeWaiter.Pause(1000);
            // get out of this menu
            ExitMenus(context);
        }

        private static void GetSignetFromRabidWolf(IGameContext context, IMemoryAPI fface)
        {
            IUnit rabidWolfIM = new NullUnit
            {
                Id = 52
            };
            InteractWithUnit(context, fface, rabidWolfIM);
            TimeWaiter.Pause(2000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            TimeWaiter.Pause(5000);
        }

        private static void BuyAllAcheronShields(IMemoryAPI fface)
        {

            fface.Windower.SendString("/addon unload sparks");
            TimeWaiter.Pause(1000);
            fface.Windower.SendString("/addon load sparks");

            for (int i = 25 - 1; i >= 0; i--)
            {
                fface.Windower.SendString("/sparks buy Acheron Shield");
                TimeWaiter.Pause(1000);
            }
        }

        private static void WarpToGoldsmith(IGameContext context, IMemoryAPI fface)
        {
            IUnit homePointOne = new NullUnit
            {
                Id = 86
            };
            context.Target = homePointOne;
            Classes.Player.SetTarget(fface, context.Target);
            //InteractWithUnit(context, fface, homePointOne);
            //TimeWaiter.Pause(3000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            TimeWaiter.Pause(3000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.DOWN);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.DOWN);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.DOWN);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.UP);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
        }

        private static void WarpToEntrance(IGameContext context, IMemoryAPI fface)
        {
            IUnit homePointFour = new NullUnit
            {
                Id = 87
            };
            context.Target = homePointFour;
            Classes.Player.SetTarget(fface, context.Target);
            TimeWaiter.Pause(homepointDelayConstant);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            TimeWaiter.Pause(3000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            TimeWaiter.Pause(homepointDelayConstant);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.DOWN);
            TimeWaiter.Pause(homepointDelayConstant);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            TimeWaiter.Pause(homepointDelayConstant);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.DOWN);
            TimeWaiter.Pause(homepointDelayConstant);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            TimeWaiter.Pause(homepointDelayConstant);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.UP);
            TimeWaiter.Pause(homepointDelayConstant);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
        }

        private static void OpenGoldsmithDoor(IGameContext context, IMemoryAPI fface)
        {
            IUnit goldsmithDoor = new NullUnit
            {
                Id = 72
            };
            OpenDoor(context, fface, goldsmithDoor);
        }

        private static void OpenMjollsGoodsDoor(IGameContext context, IMemoryAPI fface)
        {
            IUnit goldsmithDoor = new NullUnit
            {
                Id = 82
            };
            OpenDoor(context, fface, goldsmithDoor);
        }

        private static void OpenDoor(IGameContext context, IMemoryAPI fface, IUnit door)
        {
            context.API.Navigator.GotoNPC(door.Id, context.Config.IsObjectAvoidanceEnabled);
            InteractWithUnit(context, fface, door);
        }

        private static void InteractWithUnit(IGameContext context, IMemoryAPI fface, IUnit unit)
        {
            //context.Memory.EliteApi.Navigator.GotoNPC(unit.Id, context.Config.IsObjectAvoidanceEnabled);
            //FaceUnit(context, fface, unit);
            TargetUnit(context, fface, unit);
            //TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            TimeWaiter.Pause(1000);
        }

        public static void FaceUnit(IGameContext context, IMemoryAPI fface, IUnit unit)
        {
            var player = fface.Player.Position;
            var position = unit.Position;
             while (context.Memory.EliteApi.Player.Position.H <= position.H - 0.25 || context.Memory.EliteApi.Player.Position.H >= position.H + 0.25)
            {
                context.API.Windower.SendKeyPress(EliteMMO.API.Keys.A);
            }
        }

        private static void TargetUnit(IGameContext context, IMemoryAPI fface, IUnit unit)
        {
            context.Target = unit;
            Classes.Player.SetTarget(fface, context.Target);
        }
        
        private static void SellAllJunk(IGameContext context, IMemoryAPI fface, IUnit shopKeeper)
        {
            if (shopKeeper == null)
            {
                throw new ArgumentNullException(nameof(shopKeeper));
            }

            InteractWithUnit(context, fface, shopKeeper);
            TimeWaiter.Pause(1000);
            ExitMenus(context);
            TimeWaiter.Pause(1000);
            fface.Windower.SendString("/sellall *");
        }

        private static void StoreGoodsAndDropTrash(IMemoryAPI fface)
        {
            fface.Windower.SendString("/putall *ite");
            fface.Windower.SendString("/putall *geode");
            fface.Windower.SendString("/dropall *seasoning*");
            fface.Windower.SendString("/dropall *fossil*");
            fface.Windower.SendString("/sort sack");
            fface.Windower.SendString("/sort case");
        }

        private static void StoreCrystalsAtMoogle(IGameContext context, IMemoryAPI fface)
        {
            IUnit ephmeralMoogle = new NullUnit();
            ephmeralMoogle.Id = 393;
            context.Target = ephmeralMoogle;
            Classes.Player.SetTarget(fface, context.Target);
            fface.Windower.SendString("/tradeall *crystal");
            TimeWaiter.Pause(10000);
        }

        private double DistanceTo(IMemoryAPI fface, Position position)
        {
            var player = fface.Player.Position;

            return Math.Sqrt(
                Math.Pow(position.X - player.X, 2) +
                Math.Pow(position.Y - player.Y, 2) +
                Math.Pow(position.Z - player.Z, 2));
        }
    }
}
