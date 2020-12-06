using EasyFarm.Context;
using EasyFarm.Logging;
using EasyFarm.Persistence;
using EasyFarm.States;
using EasyFarm.UserSettings;
using MemoryAPI;
using MemoryAPI.Navigation;
using System;
using System.IO;
using EasyFarm.ViewModels;
using EliteMMO.API;
using Pathfinder;

namespace EasyFarm.Classes
{
    class Navigator : INavigator
    {
        private IMemoryAPI _memory;

        public Navigator(IMemoryAPI memory)
        {
            _memory = memory;
        }

        private void AvoidObstacles()
        {
            if (IsStuck())
            {
                LogViewModel.Write("I am stuck at!" + _memory.Player.Position);
                //RecordTravelBlock();
                //if (IsEngaged()) Disengage();
                //WiggleCharacter(attempts: 3);
            }
        }

        public bool IsStuck()
        {
            var firstX = _memory.Player.PosX;
            var firstZ = _memory.Player.PosZ;
            TimeWaiter.Pause(2000);
            var dchange = Math.Pow(firstX - _memory.Player.PosX, 2) + Math.Pow(firstZ - _memory.Player.PosZ, 2);
            return Math.Abs(dchange) < 1;
        }

        public void HomePointWarpAddon(IGameContext context, IMemoryAPI fface, string homePoint)
        {
            fface.Windower.SendString($"/addon load HomePoint");
            fface.Windower.SendString($"/hp warp {homePoint}");
            //fface.Windower.SendString($"/addon unload HomePoint");
            WaitForZone(fface, context);
        }

        // public void InteractWithUnit(IGameContext context, IMemoryAPI fface, IUnit unit)
        // {
        //     context.Memory.EliteApi.Navigator.GotoNPC(unit.Id, context.Config.IsObjectAvoidanceEnabled);
        //     InteractWithoutMoving(context, fface, unit);
        // }

        public void TravelToNpcAndTalk(IGameContext context, IUnit unit)
        {
            var action = new InitiateConversationAction {Unit = unit};
            TravelToNpcAndPerformAction(context, unit, action);
        }

        public void TravelToNpcAndPerformAction(IGameContext context, IUnit unit, INpcAction action)
        {
            if (!TryTravelToNpc(context, unit)) return;

            context.API.Navigator.FaceHeading(unit.Position);
            context.API.Navigator.Reset();

            // Has the user decided we should engage in battle. 
            action.PerformAction(context);
        }

        public  bool TryTravelToNpc(IGameContext context, IUnit unit)
        {
            var path = context.NavMesh.FindPathBetween(context.API.Player.Position, unit.Position);
            if (path.Count == 0)
                return true;

            if (path.Count > 1)
            {
                context.API.Navigator.DistanceTolerance = 0.5;
            }
            else
            {
                context.API.Navigator.DistanceTolerance = context.Config.MeleeDistance;
            }

            while (path.Count > 0 &&
                   path.Peek().Distance(context.API.Player.Position) <= context.API.Navigator.DistanceTolerance)
            {
                path.Dequeue();
            }

            if (path.Count > 0)
            {
                context.API.Navigator.GotoNPC(unit.Id, path.Peek(), true);
                return false;
            }

            return true;
        }

        public void InteractWithoutMoving(IGameContext context, IMemoryAPI fface, IUnit unit)
        {
            TargetUnit(context, fface, unit);
            context.API.Windower.SendKeyPress(Keys.RETURN);
            TimeWaiter.Pause(1000);
        }

        public void OpenDoor(IGameContext context, IMemoryAPI fface)
        {
            IUnit door = context.Memory.UnitService.GetClosestUnitByPartialName("Door");
            var distanceToDoor = GridMath.GetDistancePos(context.Traveler.Walker.CurrentPosition,
                GridMath.RoundVector3(door.Position.ToVector3()));

            if (distanceToDoor < 5)
            {
                // TODO: Figure out how to make this only trigger when looking at a door.
                // context(door.Position, context.API.Player.Position);
                // var radian = context.API.Navigator.CalculateRadianForWhereToTurn(door.Position, context.API.Player.Position);

                context.Navigator.InteractWithoutMoving(context, fface, door);
            }
        }

        // public void GoToNpc(IGameContext context, IMemoryAPI fface, string npcName)
        // {
        //     IUnit npc = context.Memory.UnitService.GetClosestUnitByPartialName(npcName);
        //     context.Memory.EliteApi.Navigator.GotoNPC(npc.Id, , true);
        // }

        public static void FaceUnit(IGameContext context, IMemoryAPI fface, IUnit unit)
        {
            var player = fface.Player.Position;
            var position = unit.Position;
            while (context.Memory.EliteApi.Player.Position.H <= position.H - 0.25 ||
                   context.Memory.EliteApi.Player.Position.H >= position.H + 0.25)
            {
                context.API.Windower.SendKeyPress(Keys.A);
            }
        }

        public static void TargetUnit(IGameContext context, IMemoryAPI fface, IUnit unit)
        {
            context.Target = unit;
            Player.SetTarget(fface, context.Target);
        }

        public void WaitForZone(IMemoryAPI fface, IGameContext context)
        {
            TimeWaiter.Pause(1200);
            Position zero = new Position {H = 0, X = 0, Y = 0, Z = 0};

            //waiting for the zoning to start


            //waiting for zoning to end
            while (DistanceTo(fface, zero) == 0)
            {
                TimeWaiter.Pause(100);
            }

            // just because we got a pos, doesn't mean things are loaded.
            TimeWaiter.Pause(5000);
        }

        public double DistanceTo(IMemoryAPI fface, Position position)
        {
            var player = fface.Player.Position;

            return Math.Sqrt(
                Math.Pow(position.X - player.X, 2) +
                Math.Pow(position.Y - player.Y, 2) +
                Math.Pow(position.Z - player.Z, 2));
        }

        public void TravelPath(IGameContext context, string routePath)
        {
            throw new NotImplementedException();
        }

        // public void TravelPath(IGameContext context, string routePath)
        // {
        //     LoadRoute(routePath);
        //     StartRoute(context);
        //
        //     for (int i = 0; i < 5; i++)
        //     {
        //         context.API.Windower.SendKeyPress(Keys.NUMPAD8);
        //         TimeWaiter.Pause(100);
        //     }
        //
        //     Config.Instance.Route.Reset();
        //     context.API.Navigator.Reset();
        // }

        public void LoadRoute(string routePath)
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

        // public static void StartRoute(IGameContext context)
        // {
        //     var shouldKeepRunningToNextWaypoint = context.Config.Route.Waypoints.Count != 1;
        //     var getNewRoute = false;
        //
        //     while (shouldKeepRunningToNextWaypoint && getNewRoute == false)
        //     {
        //         //context.API.Navigator.DistanceTolerance = 1; // maybe lower this so that the mjol section doesn't suck so bad.
        //
        //         var nextPosition = context.Config.Route.GetNextPositionPath(context.API.Player.Position);
        //         if (nextPosition == null)
        //         {
        //             shouldKeepRunningToNextWaypoint = false;
        //         }
        //         else
        //         {
        //
        //             getNewRoute = context.API.Navigator.GotoWaypoint(
        //                 nextPosition,
        //                 context.Config.IsObjectAvoidanceEnabled,
        //                 shouldKeepRunningToNextWaypoint, shouldKeepRunningToNextWaypoint);
        //         }
        //     }
        // }

        public void WarpHome(IMemoryAPI fface, IGameContext context)
        {
            Position zero = new Position {H = 0, X = 0, Y = 0, Z = 0};
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

        //private void HomepointWarpFromFavorites(IGameContext context, IMemoryAPI fface, string favoriteHomePoint)
        //{
        //    List<string> PortJuenoMHomePointWarp = new List<string>() { "Travel to another home point.", "Select from favorites.", favoriteHomePoint, "Yes, please." };
        //    HomePointWarp(context, fface, PortJuenoMHomePointWarp);
        //}

        //public void HomePointWarp(IGameContext context, IMemoryAPI fface, List<string> PortJuenoMHomePointWarp)
        //{
        //    var homepoint = context.Memory.UnitService.GetClosestUnitByPartialName("Home Point");
        //    InteractWithUnit(context, fface, homepoint);
        //    ChooseDialogOptions(context, PortJuenoMHomePointWarp);
        //    WaitForZone(fface, context);
        //}
    }

    internal class InitiateConversationAction : INpcAction
    {
        public void PerformAction(IGameContext context)
        {
            context.Navigator.InteractWithoutMoving(context, context.API, Unit);
        }

        public IUnit Unit { get; set; }
    }
}