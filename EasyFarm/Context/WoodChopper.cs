using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using EasyFarm.Classes;
using EasyFarm.ViewModels;
using NLog.Fluent;
using Pathfinder;
using Pathfinder.People;

namespace EasyFarm.Context
{
    public class PersonLooper
    {
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

        public bool TryToGoToTargetZone(IGameContext context)
        {
            while (context.Traveler?.World?.Zones == null)
            {
                Thread.Sleep(100);
            }

            if (context.Traveler.CurrentZone.Name == ChopWoodZone)
                return true;

            LogViewModel.Write("Going to zone: " + ChopWoodZone);
            context.Traveler.WalkToZone(ChopWoodZone);
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

        public void SetMobsToTarget(IGameContext context, List<string> mobsToFight)
        {
            // if (context.Config.TargetedMobs.ToList() != mobsToFight)
            // {
            //     while (context.Config.TargetedMobs.Count > 0)
            //     {
            //         context.Config.TargetedMobs.RemoveAt(0);
            //     }
            // }


            foreach (var mob in mobsToFight)
            {
                if (!context.Config.TargetedMobs.Contains(mob))
                    context.Config.TargetedMobs.Add(mob);
            }
        }

        public void SetAllMobsWithinDistanceOfPointToLoggingPoints(IGameContext context, List<string> mobsToFight,
            Vector3 centerPoint, int distance)
        {
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

        private void SetAllMobsInZoneToLoggingPoints(IGameContext context , List<string> mobsToFight)
        {
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

        public void LoopOverMobsWithinDistanceOfPoint(IGameContext context, List<string> mobsToFight, string targetZone,
            string purpose, Vector3 centerPoint, int distance)
        {


            ChopWoodZone = targetZone;
            if (!TryToGoToTargetZone(context))
                return;


            if (ShouldSetNewLoggingPoints(purpose))
            {
                Purpose = purpose;
                SetAllMobsWithinDistanceOfPointToLoggingPoints(context, mobsToFight, centerPoint,
                    distance);
            }

            LoopOverMobs(context, mobsToFight);
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
            return Purpose != purpose || LoggingPoints.Count == 0;
        }

        public void ChopTreesInZone(IGameContext context, string targetZone, string purpose, string resourceName)
        {
            ChopWoodZone = targetZone;
            if (!TryToGoToTargetZone(context))
                return;

            if (ShouldSetNewLoggingPoints(purpose))
            {
                Purpose = purpose;
                SetLggingPointsWithAllGatherPointsInZone(context, resourceName);
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

        private void SetLggingPointsWithAllGatherPointsInZone(IGameContext context, string gatherPointName)
        {
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
            context.API.Navigator.GotoNPC(context.Target.Id, context.Config.IsObjectAvoidanceEnabled);

            // Target mob if not currently targeted. 
            Classes.Player.SetTarget(context.API, context.Target);

            LogViewModel.Write("Chopping down tree at: " + context.Target.Position);
            context.API.Windower.SendString("/item Hatchet <t>");
            Thread.Sleep(4000);
        }

        public void LoopOverMobsInZoneMatchingList(IGameContext context, List<string> mobsToFight, string targetZone,
            string purpose)
        {
            if (context.Traveler?.World?.Mobs == null)
                return;

            ChopWoodZone = targetZone;
            if (!TryToGoToTargetZone(context))
                return;


            if (ShouldSetNewLoggingPoints(purpose))
            {
                Purpose = purpose;
                SetAllMobsInZoneToLoggingPoints(context, mobsToFight);
            }
            
            LoopOverMobs(context, mobsToFight); 
        }
    }
}