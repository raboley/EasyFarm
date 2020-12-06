using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Threading;
using EasyFarm.Classes;
using EasyFarm.States;
using EasyFarm.ViewModels;
using EasyFarm.Views;
using NLog.Fluent;
using Pathfinder;
using Pathfinder.People;

namespace EasyFarm.Context
{
    public static class ExtensionMethods
    {
        public static int Remove<T>(
            this ObservableCollection<T> coll, Func<T, bool> condition)
        {
            var itemsToRemove = coll.Where(condition).ToList();

            foreach (var itemToRemove in itemsToRemove)
            {
                coll.Remove(itemToRemove);
            }

            return itemsToRemove.Count;
        }
    }

    public class PersonLooper
    {
        /// <summary>
        /// Finds all known mobs in a zone, and then walks to each of them from closest mob to farthest mob.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mobsToFight"></param>
        /// <param name="targetZone"></param>
        /// <param name="purpose"></param>
        public void LoopOverMobsInZoneMatchingList(IGameContext context, List<string> mobsToFight, string targetZone,
            string purpose)
        {
            if (!TryToGoToTargetZone(context, targetZone))
            {
                SetMobsToTargetToNothing(context);
                return;
            }

            if (ShouldSetNewLoggingPoints(purpose))
            {
                Purpose = purpose;
                SetAllMobsInZoneToLoggingPoints(context, mobsToFight);
            }

            LoopOverMobs(context, mobsToFight);
        }

        /// <summary>
        /// Finds all mobs within a certain distance from a given center point that match a given list of mob names
        /// then walks to each mob in a loop based on distance from the player.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mobsToFight"></param>
        /// <param name="targetZone"></param>
        /// <param name="purpose"></param>
        /// <param name="centerPoint"></param>
        /// <param name="distance"></param>
        public void LoopOverMobsWithinDistanceOfPoint(IGameContext context, List<string> mobsToFight, string targetZone,
            string purpose, Vector3 centerPoint, int distance)
        {
            if (!TryToGoToTargetZone(context, targetZone))
            {
                SetMobsToTargetToNothing(context);
                return;
            }


            if (ShouldSetNewLoggingPoints(purpose))
            {
                Purpose = purpose;
                SetAllMobsWithinDistanceOfPointToLoggingPoints(context, mobsToFight, centerPoint,
                    distance, purpose);
            }

            LoopOverMobs(context, mobsToFight);
        }

        public bool TryToGoToTargetZone(IGameContext context, string targetZone)
        {
            ChopWoodZone = targetZone;
            if (context.Traveler?.World?.Zones == null)
            {
                return false;
            }

            if (context.Traveler.CurrentZone.Name == ChopWoodZone)
                return true;

            LogViewModel.Write("Going to zone: " + ChopWoodZone);
            context.Traveler.WalkToZone(ChopWoodZone);
            return false;
        }


        public ConcurrentQueue<Person> LoggingPoints { get; set; } = new ConcurrentQueue<Person>();
        public Person NextPoint { get; set; }
        public string Purpose { get; set; }

        public string ChopWoodZone = "Ronfaure_East";

        public bool SetNextPoint()
        {
            var worked = LoggingPoints.TryDequeue(out var nextPoint);
            if (worked == false)
                return false;

            NextPoint = nextPoint;

            return true;
        }

        public bool HasHatchet(IGameContext context)
        {
            if (context.Inventory.InventoryIsFull())
                return false;

            if (context.Inventory.HaveItemInInventoryContainer("Hatchet"))
                return true;

            return false;
        }


        public void SetNextPointIfHasBeenReached(Vector3 currentPosition, int acceptableDistance = 1)
        {
            if (NextPoint != null)
            {
                var distance = GridMath.GetDistancePos(currentPosition, NextPoint.Position);
                if (distance > acceptableDistance)
                    return;
            }


            if (LoggingPoints.IsEmpty)
                return;

            SetNextPoint();
        }

        public List<Person> GetAllMobsWithinDistanceOfPoint(IGameContext context, Vector3 centerPoint, int distance)
        {
            var allMobsWithinDistanceOfPoint = context.Traveler.World.Mobs.Where(x =>
                GridMath.GetDistancePos(centerPoint, x.Position) < distance);

            return allMobsWithinDistanceOfPoint.ToList();
        }

        public void SetMobsToTargetToNothing(IGameContext context)
        {
            var mobsToFight = new List<string>
            {
                "dontFightAnything"
            };
            
            RemoveAllTargetedMobs(context, mobsToFight);
            AddAllMobsToFight(context, mobsToFight);
        }
        public void SetMobsToTarget(IGameContext context, List<string> mobsToFight)
        {
            RemoveAllTargetedMobs(context, mobsToFight);
            ConfigureSettingsToFight(context, mobsToFight);
        }

        private static void RemoveAllTargetedMobs(IGameContext context, List<string> mobsToFight)
        {
            if (context.Config.TargetedMobs.Count > mobsToFight.Count)
            {
                while (context.Config.TargetedMobs.Count > 0)
                {
                    App.Current.Dispatcher.Invoke((Action) delegate // <--- HERE
                    {
                        context.Config.TargetedMobs.RemoveAt(0);
                    });
                }
            }
        }

        private static void ConfigureSettingsToFight(IGameContext context, List<string> mobsToFight)
        {
            AddAllMobsToFight(context, mobsToFight);

            if (!context.Config.UnclaimedFilter)
            {
                App.Current.Dispatcher.Invoke((Action) delegate // <--- HERE
                {
                    context.Config.UnclaimedFilter = true;
                });
            }

            if (!context.Config.AggroFilter)
            {
                App.Current.Dispatcher.Invoke((Action) delegate // <--- HERE
                {
                    context.Config.AggroFilter = true;
                });
            }

            if (!context.Config.PartyFilter)
            {
                App.Current.Dispatcher.Invoke((Action) delegate // <--- HERE
                {
                    context.Config.PartyFilter = true;
                });
            }

            if (Math.Abs(context.Config.DetectionDistance - 50) > 0)
            {
                App.Current.Dispatcher.Invoke((Action) delegate // <--- HERE
                {
                    context.Config.DetectionDistance = 50;
                });
            }
        }

        private static void AddAllMobsToFight(IGameContext context, List<string> mobsToFight)
        {
            foreach (var mob in mobsToFight)
            {
                if (!context.Config.TargetedMobs.Contains(mob))
                {
                    App.Current.Dispatcher.Invoke((Action) delegate // <--- HERE
                    {
                        context.Config.TargetedMobs.Add(mob);
                    });
                }
            }
        }

        public void SetAllMobsWithinDistanceOfPointToLoggingPoints(IGameContext context, List<string> mobsToFight,
            Vector3 centerPoint, int distance, string purpose)
        {
            if (context.Traveler?.World?.Mobs == null)
                return;

            Purpose = purpose;
            LoggingPoints = new ConcurrentQueue<Person>();
            var allMobsWithinDistance =
                GetAllMobsWithinDistanceOfPoint(context, centerPoint, distance);

            var mobsMatchingNameWithinDistance = GetMobsMatchingListOfStrings(mobsToFight, allMobsWithinDistance);


            var closestMobsFirst = mobsMatchingNameWithinDistance
                .OrderBy(x => GridMath.GetDistancePos(context.Traveler.Walker.CurrentPosition, x.Position)).ToList();
            foreach (var person in closestMobsFirst)
            {
                LoggingPoints.Enqueue(person);
            }
        }

        private void SetAllMobsInZoneToLoggingPoints(IGameContext context, List<string> mobsToFight)
        {
            if (context.Traveler?.World?.Mobs == null)
                return;
            LoggingPoints = new ConcurrentQueue<Person>();

            var allKnownMatchingMobs = GetMobsMatchingListOfStrings(mobsToFight, context.Traveler.World.Mobs);

            var closestMobsFirst = allKnownMatchingMobs
                .OrderBy(x => GridMath.GetDistancePos(context.Traveler.Walker.CurrentPosition, x.Position)).ToList();
            foreach (var person in closestMobsFirst)
            {
                LoggingPoints.Enqueue(person);
            }
        }

        private static List<Person> GetMobsMatchingListOfStrings(List<string> mobsToFight, List<Person> listOfMobs)
        {
            var mobsMatchingName = new List<Person>();
            foreach (var mob in listOfMobs)
            {
                foreach (var targetMob in mobsToFight)
                {
                    if (!mob.Name.Contains(targetMob))
                        continue;

                    mobsMatchingName.Add(mob);
                    break;
                }
            }

            return mobsMatchingName;
        }


        private void LoopOverMobs(IGameContext context, List<string> mobsToFight)
        {
            SetMobsToTarget(context, mobsToFight);

            if (LoggingPoints.IsEmpty)
                return;

            if (NextPoint == null)
                SetNextPoint();

            if (NextPoint == null)
                return;

            context.Traveler.PathfindAndWalkToFarAwayWorldMapPosition(NextPoint.Position);
            SetNextPointIfHasBeenReached(context.Traveler.Walker.CurrentPosition);
        }

        private bool ShouldSetNewLoggingPoints(string purpose)
        {
            var countOfLogPoints = LoggingPoints.Count;
            if (Purpose == purpose && countOfLogPoints > 0)
                return false;
            
            LogViewModel.Write("Setting up things to loop over for purpose: " + purpose);
            return true;
        }

        public void ChopTreesInZone(IGameContext context, string targetZone, string purpose, string resourceName)
        {
            if (!TryToGoToTargetZone(context, targetZone))
                return;

            if (ShouldSetNewLoggingPoints(purpose))
            {
                SetLoggingPointsWithAllGatherPointsInZone(context, resourceName, purpose);
            }

            if (LoggingPoints.IsEmpty)
                return;

            if (NextPoint == null)
                SetNextPoint();

            if (NextPoint == null)
                return;
            IUnit closeByLoggingPoint = null;

            if (HasHatchet(context))
                closeByLoggingPoint = context.Memory.UnitService.GetClosestUnitByPartialName("Logging Point");
            if (closeByLoggingPoint != null && closeByLoggingPoint.IsRendered)
                context.WoodChopper.NextPoint = new Person(closeByLoggingPoint.Id, closeByLoggingPoint.Name,
                    GridMath.RoundVector3(closeByLoggingPoint.Position.To2DVector3()));

            context.Traveler.PathfindAndWalkToFarAwayWorldMapPosition(NextPoint.Position);
            if (HasHatchet(context))
                ChopTree(context, resourceName);
            SetNextPointIfHasBeenReached(context.Traveler.Walker.CurrentPosition);
        }

        private void SetLoggingPointsWithAllGatherPointsInZone(IGameContext context, string gatherPointName,
            string purpose)
        {
            Purpose = purpose;
            // First add things close by
            var closeLoggingPoints =
                context.Memory.UnitService.NpcUnits.ToList().FindAll(x => x.Name == gatherPointName && x.IsRendered);

            var orderedCloseByLoggingPoints = closeLoggingPoints.OrderBy(x => x.Distance);

            foreach (var loggingPoint in orderedCloseByLoggingPoints)
            {
                var tree = new Person(loggingPoint.Id, loggingPoint.Name,
                    GridMath.RoundVector3(loggingPoint.Position.To2DVector3()));

                context.WoodChopper.LoggingPoints.Enqueue(tree);
            }

            // Then all the ones that have ever been known
            var loggingPoints = context.Npcs.ToList().FindAll(x => x.Name == gatherPointName);
            var orderedLoggingPoints = loggingPoints.OrderBy(x =>
                GridMath.GetDistancePos(context.Traveler.Walker.CurrentPosition, x.Position));
            foreach (var loggingPoint in orderedLoggingPoints)
            {
                if (context.WoodChopper.LoggingPoints.Contains(loggingPoint))
                    continue;

                context.WoodChopper.LoggingPoints.Enqueue(loggingPoint);
            }
        }

        public static void ChopTree(IGameContext context, string resourceName)
        {
            var distanceToLoggingPoint = GridMath.GetDistancePos(context.Traveler.Walker.CurrentPosition,
                context.WoodChopper.NextPoint.Position);

            if (distanceToLoggingPoint > 1)
                return;

            var loggingUnit =
                context.Memory.UnitService.NpcUnits.FirstOrDefault(x => x.Id == context.WoodChopper.NextPoint.Id);
            context.WoodChopper.NextPoint = null;


            if (loggingUnit == null)
            {
                return;
            }

            context.Target = loggingUnit;
            // Face mob. 
            context.API.Navigator.FaceHeading(context.Target.Position);
            context.Navigator.TravelToNpcAndPerformAction(context, context.Target, new ChopWoodAction());

 
        }
    }

    public class ChopWoodAction : INpcAction
    {
        public void PerformAction(IGameContext context)
        {
            // Target mob if not currently targeted. 
            Classes.Player.SetTarget(context.API, context.Target);

            LogViewModel.Write("Chopping down tree at: " + context.Target.Position);
            context.API.Windower.SendString("/item Hatchet <t>");
            Thread.Sleep(4000);
        }
    }
}