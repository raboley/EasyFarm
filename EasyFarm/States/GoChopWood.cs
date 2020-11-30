using System.Linq;
using System.Threading;
using EasyFarm.Classes;
using EasyFarm.Context;
using EasyFarm.ViewModels;
using Pathfinder;
using Pathfinder.Map;
using Pathfinder.People;
using Pathfinder.Travel;
using Player = EasyFarm.Classes.Player;

namespace EasyFarm.States
{
    public class GoChopWood : BaseState
    {
        public override bool Check(IGameContext context)
        {
            if (context.Traveler == null) return false;

            if (new RestState().Check(context)) return false;

            if (new NeedSignet().Check(context)) return false;
            
            if (new SellSomeJunk().Check(context)) return false;
            if (new CraftSomething().Check(context)) return false;
            
            if (context.Player.IsDead) return false;

            if (context.Player.HasAggro) return false;

            // TODO: Remove this when I have a wandering mob killing state up.
            return true;
            
            if (context.Inventory.HaveItemInInventoryContainer("Hatchet")) return true;

            if (context.Player.CurrentGoal == "Chop Wood")
                context.Player.CurrentGoal = "Aimless";

            return false;
        }

        public override void Run(IGameContext context)
        {
            context.WoodChopper.ChopWoodZone = "Ronfaure_East"; 
            context.Player.CurrentGoal = "Chop Wood";
            context.WoodChopper.GoToTargetZone(context);

            if (context.API.Player.Zone.ToString() != context.WoodChopper.ChopWoodZone)
                return;

            if (context.Traveler.Zoning)
                return;

            if (context.Traveler.CurrentZone.Map.MapName != context.WoodChopper.ChopWoodZone)
                return;

            // Add logging points to queue if empty
            if (context.WoodChopper.LoggingPoints.Count == 0)
            {
                // First add things close by
                var closeLoggingPoints =
                    context.Memory.UnitService.NpcUnits.ToList().FindAll(x => x.Name == "Logging Point");

                foreach (var loggingPoint in closeLoggingPoints)
                {
                    var tree = new Person(loggingPoint.Id, loggingPoint.Name, GridMath.RoundVector3(loggingPoint.Position.To2DVector3()));
                    context.WoodChopper.LoggingPoints.Enqueue(tree);
                }
                
                // Then all the ones that have ever been known
                var loggingPoints = context.Npcs.ToList().FindAll(x => x.Name == "Logging Point");
                foreach (var loggingPoint in loggingPoints)
                {
                    context.WoodChopper.LoggingPoints.Enqueue(loggingPoint);
                }
            }

            // go to next logging point

            // TODO: Remove the if statement once I have a state for roaming around trying to make money.
            IUnit closeByLoggingPoint = null;
            if (HasHatchet(context))
                closeByLoggingPoint = context.Memory.UnitService.GetClosestUnitByPartialName("Logging Point");


            if (closeByLoggingPoint != null && closeByLoggingPoint.IsRendered )
                context.WoodChopper.NextPoint = new Person(closeByLoggingPoint.Id, closeByLoggingPoint.Name,
                    GridMath.RoundVector3(closeByLoggingPoint.Position.To2DVector3()));

            if (context.WoodChopper.NextPoint == null)
                context.WoodChopper.SetNextPoint();


            context.Traveler.PathfindAndWalkToFarAwayWorldMapPosition(context.WoodChopper.NextPoint.Position);
            
            // TODO: Make traveler stop path finding when someone has aggro.
            // context.Traveler.PathfindAndWalkTwoFarAwayWorldMapPosition(context.WoodChopper.NextPoint.Position);
            // Chop wood

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

            if (HasHatchet(context)) 
                PersonLooper.ChopTree(context, loggingUnit);
        }

        private bool HasHatchet(IGameContext context)
        {
            if (context.Inventory.InventoryIsFull())
                return false;

            if (context.Inventory.HaveItemInInventoryContainer("Hatchet"))
                return true;

            return false;
        }
    }
}