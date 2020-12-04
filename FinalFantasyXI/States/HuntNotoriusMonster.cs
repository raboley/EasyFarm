namespace FinalFantasyXI.States
{
    public class HuntNotoriusMonster : BaseState
    {
        private string thingToHuntName;

        public override bool Check(IGameContext context)
        {
            if (context.Traveler?.CurrentZone?.Map == null)
                return false;
            
            if (new RestState().Check(context)) return false;

            if (new NeedSignet().Check(context)) return false;
            
            if (context.Inventory.InventoryIsFull())
                return false;

            if (new DoQuest().Check(context)) return false;
            
            if (context.Player.IsDead) return false;

            if (context.Player.HasAggro) return false;
            
            if (context.Target.IsValid) return false;

            return true;
        }


        public override void Run(IGameContext context)
        {
            context.WoodChopper.ChopWoodZone = "Ronfaure_West"; 
            LogViewModel.Write("Going to Hunt Notorious Monster in Zone: " + context.WoodChopper.ChopWoodZone);
            context.WoodChopper.TryToGoToTargetZone(context, "Ronfaure_West");

            if (context.API.Player.Zone.ToString() != context.WoodChopper.ChopWoodZone)
                return;

            if (context.Traveler.Zoning)
                return;

            if (context.Traveler.CurrentZone.Map.MapName != context.WoodChopper.ChopWoodZone)
                return;

            // Add logging points to queue if empty
            if (context.WoodChopper.LoggingPoints.Count == 0)
            {
                thingToHuntName = "Jaggedy-Eared Jack";
                var nm = context.Mobs.First(x => x.Name == thingToHuntName);
                context.WoodChopper.LoggingPoints.Enqueue(nm);
                
                // First add things close by
                var closeLoggingPoints =
                    context.Memory.UnitService.MobArray.ToList().FindAll(x => x.Name.Contains(thingToHuntName));

                foreach (var loggingPoint in closeLoggingPoints)
                {
                    var tree = new Person(loggingPoint.Id, loggingPoint.Name, GridMath.RoundVector3(loggingPoint.Position.To2DVector3()));
                    context.WoodChopper.LoggingPoints.Enqueue(tree);
                }
                
                
                // // Then all the ones that have ever been known
                // var loggingPoints = context.Mobs.ToList().FindAll(x => x.Name.Contains(thingToHunt));
                // foreach (var loggingPoint in loggingPoints)
                // {
                //     context.WoodChopper.LoggingPoints.Enqueue(loggingPoint);
                // }
            }

            if (context.WoodChopper.LoggingPoints.Count == 0)
                return;

            if (context.WoodChopper.NextPoint == null)
                context.WoodChopper.SetNextPoint();
            
            context.Traveler.PathfindAndWalkToFarAwayWorldMapPosition(context.WoodChopper.NextPoint.Position);
            
            var distanceToLoggingPoint = GridMath.GetDistancePos(context.Traveler.Walker.CurrentPosition,
                context.WoodChopper.NextPoint.Position);
            if (distanceToLoggingPoint > 1)
                return;
            
            context.WoodChopper.NextPoint = null;
            
        }
    }
}