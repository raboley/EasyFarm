using MemoryAPI.Navigation;

namespace EasyFarm.Mapping
{
    public class SpotFactory
    {
        public static ISpot SpotFromPosition(Position position)
        {
            ISpot spot = new Spot
            {
                X = (int) position.X,
                Y = (int) position.Y,
                Z = (int) position.Z
            };
            if(spot.X < 0 || spot.Z < 0)
            {
                spot.IsWall = true;
            }
            return spot;
        }

        public static ISpot SpotFromSpotOffset(ISpot spot, int xOffset=0, int yOffset=0, int zOffset=0)
        {
            ISpot offsetSpot = new Spot
            {
                X = spot.X + xOffset,
                Y = spot.Y + yOffset,
                Z = spot.Z + zOffset
            };

            if (offsetSpot.X < 0 || offsetSpot.Z < 0)
            {
                offsetSpot.IsWall = true;
            }

            return offsetSpot;
        }
    }
}