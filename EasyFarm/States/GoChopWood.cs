using System.Linq;
using System.Threading;
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
        private static string _chopWoodZone= "Ronfaure_East";

        public override bool Check(IGameContext context)
        {
            if (context.Traveler == null) return false;
            
            if (new RestState().Check(context)) return false;
            
            if (context.Player.IsDead) return false;

            if (context.Player.HasAggro) return false;

            if (context.Inventory.HaveItemInInventoryContainer("Hatchet")) return true;

            return false;
        }

        public override void Run(IGameContext context)
        {
            GoToChopWoodZone(context);

            if (context.API.Player.Zone.ToString() != _chopWoodZone)
                return;

            if (context.Traveler.Zoning)
                return;

            if (context.Traveler.CurrentZone.Map.MapName != _chopWoodZone)
                return;

            // Add logging points to queue if empty
            if (context.WoodChopper.LoggingPoints.Count == 0)
            {
                var loggingPoints = context.Npcs.ToList().FindAll(x => x.Name == "Logging Point");
                foreach (var loggingPoint in loggingPoints)
                {
                    context.WoodChopper.LoggingPoints.Enqueue(loggingPoint);
                }
            }

            // go to next logging point
            var closeByLoggingPoint = context.Memory.UnitService.NpcUnits.ToList()
                .FirstOrDefault(x => x.Name == "Logging Point");
            
            if (closeByLoggingPoint != null)
                context.WoodChopper.NextPoint = new Person(closeByLoggingPoint.Id, closeByLoggingPoint.Name, GridMath.RoundVector3(closeByLoggingPoint.Position.To2DVector3()));
            
            if (context.WoodChopper.NextPoint == null) 
                context.WoodChopper.NextPoint = context.WoodChopper.LoggingPoints.Dequeue();
            
            
            context.Traveler.GoToPosition(context.WoodChopper.NextPoint.Position);
            // Chop wood

            var distanceToLoggingPoint = GridMath.GetDistancePos(context.Traveler.Walker.CurrentPosition,
                context.WoodChopper.NextPoint.Position);

            if (distanceToLoggingPoint > 1)
                return;
            
            var loggingUnit = context.Memory.UnitService.NpcUnits.FirstOrDefault(x => x.Id == context.WoodChopper.NextPoint.Id);
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
            Player.SetTarget(context.API, context.Target);

            LogViewModel.Write("Chopping down tree at: " + context.Target.Position);
            context.API.Windower.SendString("/item Hatchet <t>");
            Thread.Sleep(3000);
        }

        private static void GoToChopWoodZone(IGameContext context)
        {
            LogViewModel.Write("Going to go Chop Wood in: " + _chopWoodZone);
            while (context.Traveler == null)
            {
                Thread.Sleep(100);
            }

            context.Traveler.GoToZone(_chopWoodZone);
        }
    }
}