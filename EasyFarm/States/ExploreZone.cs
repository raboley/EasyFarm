using System;
using System.Collections.Concurrent;
using System.Numerics;
using System.Threading;
using EasyFarm.Context;
using EasyFarm.Parsing;
using Pathfinder.Map;
using Pathfinder.Travel;

namespace EasyFarm.States
{
    public class ExploreZone : BaseState
    {
        public override bool Check(IGameContext context)
        {
            if (context.Traveler?.CurrentZone?.Map == null)
                return false;

            if (new RestState().Check(context)) return false;

            if (new NeedSignet().Check(context)) return false;
            if (new DoQuest().Check(context)) return false;

            if (context.Inventory.InventoryIsFull())
                return false;

            if (context.Player.IsDead) return false;

            if (context.Player.HasAggro) return false;

            if (context.Target.IsValid) return false;

            return true;
        }

        public override void Run(IGameContext context)
        {
            // var zoneToExplore = "Ronfaure_East";
            var zoneToExplore = "La_Theine_Plateau";
            if (context.Traveler.CurrentZone.Map.MapName != zoneToExplore)
                context.Traveler.WalkToZone(zoneToExplore);

            // var goalPosition = new Vector3(-1000, 0, -1000);

            if (context.Traveler.CurrentZone.Map.MapName != zoneToExplore)
                return;

            // Setup all points to explore
            if (context.Traveler.CurrentZone.PointsToExplore.Count == 0 &&
                context.Traveler.CurrentZone.Explored.Count == 0)
            {
                var points = new ConcurrentQueue<Vector3>();
                points.Enqueue(new Vector3(1000, 0, 1000));
                points.Enqueue(new Vector3(-1000, 0, -1000));
                points.Enqueue(new Vector3(1000, 0, -1000));
                points.Enqueue(new Vector3(-1000, 0, 1000));

                context.Traveler.CurrentZone.PointsToExplore = points;
            }

            // Set the Next Point to Explore
            if (context.Traveler.CurrentZone.NextPointToExplore == null)
            {
                var worked = context.Traveler.CurrentZone.PointsToExplore.TryDequeue(out var newPointToExplore);
                if (worked == false)
                    return;

                context.Traveler.CurrentZone.NextPointToExplore = newPointToExplore;
            }

            var next = context.Traveler.CurrentZone.NextPointToExplore;
            if (next == null)
                return;

            context.Traveler.PathfindAndWalkToFarAwayWorldMapPosition((Vector3) next, secondsToRunFor: 300);


            // For now, ZoneMapPersister will set next point to explore to null whenever the traveler zones.
            // And set explored to add that one


            // var distance = GridMath.GetDistancePos(context.Traveler.Walker.CurrentPosition, jejPosition);
            // while (context.API.Player.Zone.ToString() == zoneToExplore && distance < 1)
            // {
            //     // context.Traveler.PathfindAndWalkToFarAwayWorldMapPosition(new Vector3(-500, 0, -500));
            // }


            // Max X and Max Z => walk to point till zone or point is reached
            // Then queue up next zone point.

            // Get a queue of points => Add to Zone so it persists
            // if  (queue of points).count != 0 && Zone.NextPointToExplore == null;
            //     dequeue
            // walk to point

            // if (distance to point < 10 || currentZone != context.api.player.zone.tostring())
            //     Zone.NextPointToExplore == null;
        }

        private static bool ShouldKeepExploring(IGameContext context)
        {
            if (context.Player.HasAggro)
                return false;

            if (context.Traveler.CurrentZone.Name != context.API.Player.Zone.ToString())
                return false;

            return true;
        }
    }
}