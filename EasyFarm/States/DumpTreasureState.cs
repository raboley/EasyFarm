using EasyFarm.Classes;
using EasyFarm.Context;

using EasyFarm.Infrastructure;
using EasyFarm.Logging;
using EasyFarm.Parsing;
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
using static EliteMMO.API.EliteAPI;
using static MemoryAPI.Memory.EliteMmoWrapper;


namespace EasyFarm.States
{
    class DumpTreasureState : BaseState
    {
        private const string RoutePathRoot = "D:\\Users\\Russell\\Source\\Repos\\EasyFarm\\EasyFarm\\bin\\Debug\\";
     

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
                if (context.Inventory.InventoryIsFull(0))
                {
                    return true;
                }

                // you died, gotta restart
                if (context.Memory.EliteApi.Player.Zone == context.Player.Homepoint)
                {
                    return true;
                }
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
                //string minutes = DateTime.Now.ToString("mm");
                //if (minutes == context.Config.TimeToDumpInMinutes.ToString())
                //{
                //    return true;
                //}
            }
                       
            return false;
        }
        // TODO Fix the exit menus for dialog
        public override void Run(IGameContext context)
        {
            IMemoryAPI fface = context.API;
            context.Memory.EliteApi.Navigator.DistanceTolerance = 3;

            LogViewModel.Write("Dumping items to storage and stuff...");
            context.Navigator.WarpHome(fface, context);
            //SpendMeritPoints(context, fface);
            //ExitMogHouseGoToCrystal(context, fface);
            //HomePointWarpPortJuenoMogHouse(context, fface);
            context.Navigator.TravelPath(context, RoutePathRoot + "_port_jeuno_moghouse_to_shemo.ewl");
            StoreSealsAndCretsAtShemo(context, fface);

            context.Navigator.TravelPath(context, RoutePathRoot + "_shemo_to_port_jueno_moghouse_homepoint.ewl");
            WarpToGoldsmith(context, fface);
            
            StoreCrystalsAndSellItemsToGoldsmith(context, fface);
            TravelToHomepointOne(context, fface);
            ConvertSparksToGil(context, fface);
            GetSignet(context, fface);
            SpendUnityPointsAndWarpToBoyda(context, fface);

            SummonTrustsAndGoInvis(fface);

            //// walk to hard mobs zone
            context.Navigator.TravelPath(context, RoutePathRoot + "_boyda_to_jp_route.ewl");

            // start bot
            context.Navigator.LoadRoute("C:\\Users\\Russell\\Desktop\\Release\\boyda_route_jp.ewl");
        }


        private void SpendMeritPoints(IGameContext context, IMemoryAPI fface)
        {
            // go to mog house
            string homepoint3ToMogHouse = RoutePathRoot + "_homepoint3_to_moghouse.ewl";
            context.Navigator.TravelPath(context, homepoint3ToMogHouse);

            for (int i = 0; i < 10; i++)
            {
                context.API.Windower.SendKeyPress(EliteMMO.API.Keys.NUMPAD8);
                TimeWaiter.Pause(100);
            }

            context.Navigator.WaitForZone(fface, context);
            // spend merit points
            SpendMeritPointToMaxMeritPoints(context);
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
            
            context.Menu.ExitMenus(context);
        }

        private void ExitMogHouseGoToCrystal(IGameContext context, IMemoryAPI fface)
        {
            // exit mog house
            ExitMogHouse(context);

            // Wait for zone to load
            context.Navigator.WaitForZone(fface, context);

            // go to warp crystal 
            context.Navigator.TravelPath(context, RoutePathRoot + "_moghouse_to_homepoint3.ewl");
        }

        private static void ExitMogHouse(IGameContext context)
        {
            FaceMogHouseExitDoor(context);

            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.TAB);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.LEFT);
            TimeWaiter.Pause(1000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
        }

        private static void FaceMogHouseExitDoor(IGameContext context)
        {
            while (context.Memory.EliteApi.Player.Position.H <= 1.25 || context.Memory.EliteApi.Player.Position.H >= 1.75)
            {
                context.API.Windower.SendKeyPress(EliteMMO.API.Keys.A);
            }
        }

        private static void StoreSealsAndCretsAtShemo(IGameContext context, IMemoryAPI fface)
        {
            IUnit shemo = context.Memory.UnitService.GetClosestUnitByPartialName("shemo");
            context.Memory.EliteApi.Navigator.GotoNPC(shemo.Id, context.Config.IsObjectAvoidanceEnabled);

            TradeAllItemsToTalkers(context, fface, "seal", shemo);
            TradeAllItemsToTalkers(context, fface, "crest", shemo);
            context.Dialog.ExitDialog(context);
            
            // extra escape for good measure
            context.Memory.EliteApi.Windower.SendKeyPress(EliteMMO.API.Keys.ESCAPE);
        }

        private static void TradeAllItemsToTalkers(IGameContext context, IMemoryAPI fface, string item, IUnit unitToTradeTo)
        {
            while (context.Inventory.HaveItemInInventoryContainer(item))
            {
                int beforeCount = context.Inventory.GetCountOfItemsInContainer(item);
                if (beforeCount == 0)
                {
                    break;
                }

                Classes.Player.SetTarget(fface, unitToTradeTo);
                fface.Windower.SendString($"/tradeall *{item}");
                TimeWaiter.Pause(2000);
                context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
                TimeWaiter.Pause(500);
                context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
                TimeWaiter.Pause(500);
                context.API.Windower.SendKeyPress(EliteMMO.API.Keys.ESCAPE);

                int afterCount = context.Inventory.GetCountOfItemsInContainer(item);
                if (beforeCount == afterCount)
                {
                    // if the count of items attempting to be traded doesn't change then
                    // container may be full of that particular set of items
                    // ie the moogle has the max capacity of 5k of a type of crystal
                    break;
                }

            }
        }

        private void WarpToGoldsmith(IGameContext context, IMemoryAPI fface)
        {
            context.Navigator.HomePointWarpAddon(context, fface, "Bastok Markets 4");

            //HomepointWarpFromFavorites(context, fface, "Bastok Markets #4.");


            //IUnit homePointOne = new NullUnit
            //{
            //    Id = 86
            //};
            //context.Target = homePointOne;
            ////Classes.Player.SetTarget(fface, context.Target);
            //context.Navigator.InteractWithUnit(context, fface, homePointOne);

            //// Not working since api returns hidden options with no way to determine what the correct option is for home point warp.
            ////List<string> HomePointWarpBastokMarketsGoldsmith = new List<string>() { "Travel to another home point.", "Bastok.", "Bastok Markets.", "Bastok Markets #4.", "Yes, please." };
            ////ChooseDialogOptions(context, HomePointWarpBastokMarketsGoldsmith);

            ////TimeWaiter.Pause(3000);
            //context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            //TimeWaiter.Pause(3000);
            //context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            //TimeWaiter.Pause(1000);
            //context.API.Windower.SendKeyPress(EliteMMO.API.Keys.DOWN);
            //TimeWaiter.Pause(1000);
            //context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            //TimeWaiter.Pause(1000);
            //context.API.Windower.SendKeyPress(EliteMMO.API.Keys.DOWN);
            //TimeWaiter.Pause(1000);
            //context.API.Windower.SendKeyPress(EliteMMO.API.Keys.DOWN);
            //TimeWaiter.Pause(1000);
            //context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            //TimeWaiter.Pause(1000);
            //context.API.Windower.SendKeyPress(EliteMMO.API.Keys.UP);
            //TimeWaiter.Pause(1000);
            //context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);

            //context.Navigator.WaitForZone(fface, context);

        }

        private void StoreCrystalsAndSellItemsToGoldsmith(IGameContext context, IMemoryAPI fface)
        {

            // go to goldsmith door
            context.Navigator.TravelPath(context, RoutePathRoot + "_homepoint4_to_goldsmith_door.ewl");

            // open door
            OpenGoldsmithDoor(context, fface);

            // go to ephemeral moogle
            context.Navigator.TravelPath(context, RoutePathRoot + "_goldsmith_door_to_ephemeral_moogle.ewl");

            // drop off crystals
            StoreCrystalsAtMoogle(context, fface);


            // deposit and sort inventories (sack and stuff)
            StoreGoodsAndDropTrash(fface);

            // go to merchent
            context.Navigator.TravelPath(context, RoutePathRoot + "_ephemeral_moogle_to_teerth.ewl");

            // sell all things
            IUnit teerth = new NullUnit
            {
                Id = 12
            };
            SellAllJunk(context, fface, teerth);
        }

        private static void OpenGoldsmithDoor(IGameContext context, IMemoryAPI fface)
        {
            IUnit goldsmithDoor = new NullUnit
            {
                Id = 72
            };
            OpenDoor(context, fface, goldsmithDoor);
        }

        private static void OpenDoor(IGameContext context, IMemoryAPI fface, IUnit door)
        {
            context.Navigator.InteractWithUnit(context, fface, door);
        }

        private static void StoreCrystalsAtMoogle(IGameContext context, IMemoryAPI fface)
        {
            IUnit ephemeralMoogle = new NullUnit
            {
                Id = 393
            };
            TradeAllItems(context, fface, "crystal", ephemeralMoogle);
            TradeAllItems(context, fface, "cluster", ephemeralMoogle);

        }

        private static void TradeAllItems(IGameContext context, IMemoryAPI fface, string item, IUnit unitToTradeTo)
        {
            while (context.Inventory.HaveItemInInventoryContainer(item))
            {
                int beforeCount = context.Inventory.GetCountOfItemsInContainer(item);
                if (beforeCount == 0)
                {
                    break;
                }

                Classes.Player.SetTarget(fface, unitToTradeTo);
                fface.Windower.SendString($"/tradeall *{item}");
                TimeWaiter.Pause(10000);
                int afterCount = context.Inventory.GetCountOfItemsInContainer(item);

                if (beforeCount == afterCount)
                {
                    // container may be full of that particular set of items
                    // ie the moogle has the max capacity of 5k of a type of crystal
                    break;
                }

            }
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

        private static void SellAllJunk(IGameContext context, IMemoryAPI fface, IUnit shopKeeper)
        {
            if (shopKeeper == null)
            {
                throw new ArgumentNullException(nameof(shopKeeper));
            }
            
            context.Navigator.InteractWithUnit(context, fface, shopKeeper);
            TimeWaiter.Pause(1000);
            context.Dialog.ExitDialog(context);
            TimeWaiter.Pause(1000);
            fface.Windower.SendString("/sellall *");
        }

        private void TravelToHomepointOne(IGameContext context, IMemoryAPI fface)
        {
            // go to door
            context.Navigator.TravelPath(context, RoutePathRoot + "_teerth_to_goldsmith_door.ewl");

            // open door
            OpenGoldsmithDoor(context, fface);

            // go to crystal
            context.Navigator.TravelPath(context, RoutePathRoot + "_goldsmith_doort_to_homepoint4.ewl");

            // warp to markets entrance
            WarpToEntrance(context, fface);
            context.Navigator.WaitForZone(fface, context);
        }

        private void WarpToEntrance(IGameContext context, IMemoryAPI fface)
        {
            context.Navigator.HomePointWarpAddon(context, fface, "Bastok Markets 1");

            //IUnit homePointFour = new NullUnit
            //{
            //    Id = 87
            //};
            //context.Target = homePointFour;
            //context.Navigator.InteractWithUnit(context, fface, homePointFour);

            //// Not working since api returns hidden options with no way to determine what the correct option is for home point warp.
            ////List<string> HomePointWarpBastokMarketsGoldsmith = new List<string>() { "Travel to another home point.", "Bastok.", "Bastok Markets.", "Home Point #1 (E).", "Yes, please." };
            ////ChooseDialogOptions(context, HomePointWarpBastokMarketsGoldsmith);

            ////TimeWaiter.Pause(homepointDelayConstant);
            ////context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            //TimeWaiter.Pause(3000);
            //context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            //TimeWaiter.Pause(homepointDelayConstant);
            //context.API.Windower.SendKeyPress(EliteMMO.API.Keys.DOWN);
            //TimeWaiter.Pause(homepointDelayConstant);
            //context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            //TimeWaiter.Pause(homepointDelayConstant);
            //context.API.Windower.SendKeyPress(EliteMMO.API.Keys.DOWN);
            //TimeWaiter.Pause(homepointDelayConstant);
            //context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            //TimeWaiter.Pause(homepointDelayConstant);
            //context.API.Windower.SendKeyPress(EliteMMO.API.Keys.UP);
            //TimeWaiter.Pause(homepointDelayConstant);
            //context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
        }

        private void ConvertSparksToGil(IGameContext context, IMemoryAPI fface)
        {



            // go to isakoth
            context.Navigator.TravelPath(context, RoutePathRoot + "_homepoint1_to_isakoth.ewl");
            IUnit Isakoth = new NullUnit
            {
                Id = 177
            };
            TimeWaiter.Pause(100);
            context.API.Navigator.GotoNPC(Isakoth.Id, context.Config.IsObjectAvoidanceEnabled);

            // trying to make sure he is loaded so that you can actually exchange sparks.
            TimeWaiter.Pause(2000);
            context.Navigator.InteractWithUnit(context, fface, Isakoth);
            context.Dialog.ExitDialog(context);

            //// buy all acheron shields
            BuyAllAcheronShields(context, fface);

            // go to merchant door
            context.Navigator.TravelPath(context, RoutePathRoot + "_isakoth_to_mjol_door.ewl");

            // open door
            OpenMjollsGoodsDoor(context, fface);

            // go to merchant
            context.Navigator.TravelPath(context, RoutePathRoot + "_mjol_door_to_olwyn.ewl");

            // sell all things
            IUnit olwyn = new NullUnit
            {
                Id = 29
            };
            SellAllJunk(context, fface, olwyn);
        }

        private static void BuyAllAcheronShields(IGameContext context, IMemoryAPI fface)
        {

            fface.Windower.SendString("/addon unload sparks");
            TimeWaiter.Pause(1000);
            fface.Windower.SendString("/addon load sparks");
            string item = "Acheron";

            while (true)
            {
                int beforeCount = context.Inventory.GetCountOfItemsInContainer(item);

                fface.Windower.SendString("/sparks buy Acheron Shield");
                TimeWaiter.Pause(1500);
                int afterCount = context.Inventory.GetCountOfItemsInContainer(item);

                if (beforeCount == afterCount)
                {
                    // Buy shields until it doesn't add another shield to inventory
                    break;
                }
            }
        }

        private static void OpenMjollsGoodsDoor(IGameContext context, IMemoryAPI fface)
        {
            IUnit goldsmithDoor = new NullUnit
            {
                Id = 82
            };
            OpenDoor(context, fface, goldsmithDoor);
        }

        private void GetSignet(IGameContext context, IMemoryAPI fface)
        {

            // go to merchant door
            context.Navigator.TravelPath(context, RoutePathRoot + "_olwyn_to_mjol_door.ewl");

            // open door
            OpenMjollsGoodsDoor(context, fface);

            // go to signet guy
            context.Navigator.TravelPath(context, RoutePathRoot + "_mjol_door_to_rabid_wolf.ewl");
            IUnit rabidWolfIM = new NullUnit
            {
                Id = 52
            };
            TimeWaiter.Pause(100);
            //context.API.Navigator.GotoNPC(rabidWolfIM.Id, context.Config.IsObjectAvoidanceEnabled);

            // get signet
            GetSignetFromRabidWolf(context, fface);
        }

        private static void GetSignetFromRabidWolf(IGameContext context, IMemoryAPI fface)
        {
            IUnit rabidWolfIM = new NullUnit
            {
                Id = 52
            };
            context.Navigator.InteractWithUnit(context, fface, rabidWolfIM);
            TimeWaiter.Pause(2000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            TimeWaiter.Pause(5000);
        }

        private void SpendUnityPointsAndWarpToBoyda(IGameContext context, IMemoryAPI fface)
        {

            // go to warp guy
            context.Navigator.TravelPath(context, RoutePathRoot + "_rabid_wolf_to_igsli.ewl");

            // buy all gobbie keys
            IUnit igsli = new NullUnit
            {
                Id = 185
            };
            BuyAllGobbieKeys(context, fface, igsli);

            // warp to boyda
            WarpToBoyahdaTree(context, fface, igsli);
            TimeWaiter.Pause(5000);
            context.Navigator.WaitForZone(fface, context);
        }

        private static void BuyAllGobbieKeys(IGameContext context, IMemoryAPI fface, IUnit igsli)
        {
            context.Navigator.InteractWithUnit(context, fface, igsli);
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
            context.Dialog.ExitDialog(context);
        }

        private static void WarpToBoyahdaTree(IGameContext context, IMemoryAPI fface, IUnit igsli)
        {
            context.Navigator.InteractWithUnit(context, fface, igsli);
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
            
            // waiting for the animation of teleporting to end
            TimeWaiter.Pause(2000);
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

        private static void SummonTrust(IMemoryAPI fface, string trust)
        {
            fface.Windower.SendString($"/ma {trust} <me>");
            TimeWaiter.Pause(6000);
        }
























        
    }
}
