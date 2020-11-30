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
            if (NextPoint == null)
            {
                var worked = LoggingPoints.TryDequeue(out var nextPoint);
                if (worked == false)
                    return false;

                NextPoint = nextPoint;
            }

            return true;
        }

        public static void ChopTree(IGameContext context, IUnit loggingUnit)
        {
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

        public bool GoToTargetZone(IGameContext context)
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
            if (GridMath.GetDistancePos(currentPosition, NextPoint.Position) > acceptableDistance)
                return;

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

            var mobsMatchingNameWithinDistance = new List<Person>();
            foreach (var mob in allMobsWithinDistance)
            {
                foreach (var targetMob in mobsToFight)
                {
                    if (!mob.Name.Contains(targetMob)) 
                        continue;
                    
                    mobsMatchingNameWithinDistance.Add(mob);
                    break;
                }
            }


            var closestMobsFirst = mobsMatchingNameWithinDistance
                .OrderBy(x => GridMath.GetDistancePos(context.Traveler.Walker.CurrentPosition, x.Position)).ToList();
            foreach (var person in closestMobsFirst)
            {
                LoggingPoints.Enqueue(person);
            }
        }
        
        public void LoopOverMobsInList(IGameContext context,List<string> mobsToFight, string targetZone, string purpose, Vector3 centerPoint, int distance)
        {
            SetMobsToTarget(context, mobsToFight);

            ChopWoodZone = targetZone;
            if (!GoToTargetZone(context))
                return;


            if (Purpose != purpose || LoggingPoints.Count == 0)
            {
                Purpose = purpose;
                SetAllMobsWithinDistanceOfPointToLoggingPoints(context, mobsToFight, centerPoint,
                    distance);
            }

            if (LoggingPoints.IsEmpty)
                return;

            if (NextPoint == null)
                SetNextPoint();

            if (NextPoint != null)
                context.Traveler.PathfindAndWalkToFarAwayWorldMapPosition(NextPoint.Position);

            SetNextPointIfHasBeenReached(context.Traveler.Walker.CurrentPosition);
        }
    }
}