using System.Diagnostics;
using System.Numerics;
using EasyFarm.Context;
using EasyFarm.ffxi;
using MemoryAPI.Navigation;
using Pathfinder;
using Pathfinder.Map;
using Pathfinder.Travel;

namespace EasyFarm.States
{
    public class TestMoveState : BaseState
    {
        public override bool Check(IGameContext context)
        {
            return true;
        }

        public override void Run(IGameContext context)
        {
            var mover = new Mover(context);
            
            var world = new World();

            if (context.Zone == null)
                return; 
            world.Zones.Add(context.Zone);
            
            if (context.Npcs == null) 
                return;
            world.Npcs.AddRange(context.Npcs);
            
            var traveler = new Traveler(context.Zone.Name, world, mover);

            var signetNpc = traveler.SearchForClosestSignetNpc("Bastok");
            if (signetNpc == null)
            {
                Debug.WriteLine("Couldn't find signet NPC!");
                return;
            }
            
            traveler.GoToPosition(signetNpc.Position);

        }
    }
}