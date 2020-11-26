using System.Numerics;
using EasyFarm.Context;

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
            var zoneToExplore = "Ronfaure_West";
            if (context.Traveler.CurrentZone.Map.MapName != zoneToExplore)
                context.Traveler.GoToZone(zoneToExplore);
            
            // var goalPosition = new Vector3(-1000, 0, -1000);
            var jejPosition = new Vector3(-435, 0, -349);

            // while (context.API.Player.Zone.ToString() == zoneToExplore)
            //     Thread.Sleep(1000);

            context.Traveler.PathfindAndWalkToFarAwayWorldMapPosition(jejPosition);
            // var distance = GridMath.GetDistancePos(context.Traveler.Walker.CurrentPosition, jejPosition);
            // while (context.API.Player.Zone.ToString() == zoneToExplore && distance < 1)
            // {
            //     // context.Traveler.PathfindAndWalkToFarAwayWorldMapPosition(new Vector3(-500, 0, -500));
            // }
        }
    }
}