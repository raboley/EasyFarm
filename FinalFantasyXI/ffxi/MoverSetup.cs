using FinalFantasyXI.Context;

namespace FinalFantasyXI.ffxi
{
    public class MoverSetup
    {
        public static void SetTraveler(IGameContext context)
        {
            var mover = new Mover(context);

            var world = new World();

            if (context.Zone == null)
                return;
            world.Zones.Add(context.Zone);

            if (context.Npcs == null)
                return;
            world.Npcs.AddRange(context.Npcs);

            if (context.Mobs == null)
                return;
            world.Mobs.AddRange(context.Mobs);

            context.Traveler = new Traveler(context.Zone.Name, world, mover);
        }
    }
}