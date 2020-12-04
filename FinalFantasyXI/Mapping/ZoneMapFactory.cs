using System.Collections.Generic;

namespace FinalFantasyXI.Mapping
{
    public static class ZoneMapFactory
    {
        public static IZoneMap GenerateZoneMap(Position startingPosition, int mapSize)
        {
            IZoneMap zoneMap = new ZoneMap();

            ISpot startingSpot = SpotFactory.SpotFromPosition(startingPosition);

            List<ISpot> spots = AddNeighboringSpotsForSize(mapSize, startingSpot);

            spots.Add(startingSpot);

            zoneMap.Spots = spots;

            return zoneMap;
        }

        private static List<ISpot> AddNeighboringSpotsForSize(int size, ISpot startingSpot)
        {
            List<ISpot> spots = new List<ISpot>();

            for (int i = 1; i <= size; i++)
            {
                //up
                spots.Add(SpotFactory.SpotFromSpotOffset(startingSpot, zOffset: i));

                // upper right
                spots.Add(SpotFactory.SpotFromSpotOffset(startingSpot, i, zOffset: i));

                // right middle
                spots.Add(SpotFactory.SpotFromSpotOffset(startingSpot, i));

                // down right
                spots.Add(SpotFactory.SpotFromSpotOffset(startingSpot, i, zOffset: i * -1));

                // down
                spots.Add(SpotFactory.SpotFromSpotOffset(startingSpot, zOffset: i * -1));

                // down left
                spots.Add(SpotFactory.SpotFromSpotOffset(startingSpot, i * -1, zOffset: i * -1));

                // left middle
                spots.Add(SpotFactory.SpotFromSpotOffset(startingSpot, i * -1));

                // upper left
                spots.Add(SpotFactory.SpotFromSpotOffset(startingSpot, i * -1, zOffset: i));
            }


            // getting additional diagonals for larger maps
            for (int i = size; i >= 1; i--)
            {
                for (int j = i - 1; j >= 1; j--)
                {
                    // upper right
                    spots.Add(SpotFactory.SpotFromSpotOffset(startingSpot, i - j, zOffset: i));
                    spots.Add(SpotFactory.SpotFromSpotOffset(startingSpot, i, zOffset: i - j));

                    // down right
                    spots.Add(SpotFactory.SpotFromSpotOffset(startingSpot, i - j, zOffset: i * -1));
                    spots.Add(SpotFactory.SpotFromSpotOffset(startingSpot, i, zOffset: (i - j) * -1));

                    // down left
                    spots.Add(SpotFactory.SpotFromSpotOffset(startingSpot, (i - j) * -1, zOffset: i * -1));
                    spots.Add(SpotFactory.SpotFromSpotOffset(startingSpot, i * -1, zOffset: (i - j) * -1));

                    // upper left
                    spots.Add(SpotFactory.SpotFromSpotOffset(startingSpot, i * -1, zOffset: i - j));
                    spots.Add(SpotFactory.SpotFromSpotOffset(startingSpot, (i - j) * -1, zOffset: i));
                }
            }


            return spots;
        }
    }
}