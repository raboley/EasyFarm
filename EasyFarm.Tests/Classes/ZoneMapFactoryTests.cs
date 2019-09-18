using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyFarm.Classes;
using EasyFarm.Mapping;
using MemoryAPI.Navigation;
using Xunit;

namespace EasyFarm.Tests.Classes
{
    public class ZoneMapFactoryTests
    {
        [Fact]
        public void ZoneMapCorrectlyReturnsSameWhenTheyAreTheSameValues()
        {
            // Given
            IZoneMap expectedZoneMap = new ZoneMap();

            expectedZoneMap.Spots = new List<ISpot>
            {
                (Spot) SpotFactory.SpotFromPosition(new Position() {H = 1, X = 1, Y = 1, Z = 1})
            };

            // When
            IZoneMap actualZoneMap = new ZoneMap();
            actualZoneMap.Spots = new List<ISpot>
            {
                (Spot) SpotFactory.SpotFromPosition(new Position() {H = 1, X = 1, Y = 1, Z = 1})
            };

            // Then
            Assert.Equal(expectedZoneMap, actualZoneMap);
        }

        [Fact]
        public void ZonesCorrectlyReturnDifferentWhenSpotsAreDifferent()
        {
            // Given
            IZoneMap expectedZoneMap = new ZoneMap();

            expectedZoneMap.Spots = new List<ISpot>
            {
                (Spot) SpotFactory.SpotFromPosition(new Position() {H = 1, X = 1, Y = 1, Z = 1}),
                (Spot) SpotFactory.SpotFromPosition(new Position() {H = 2, X = 2, Y = 2, Z = 2})
            };

            // When
            IZoneMap actualZoneMap = new ZoneMap();
            actualZoneMap.Spots = new List<ISpot>
            {
                (Spot) SpotFactory.SpotFromPosition(new Position() {H = 1, X = 1, Y = 1, Z = 1})
            };

            // Then
            Assert.NotEqual(expectedZoneMap, actualZoneMap);
        }

        [Fact]
        public void ZoneMapFactoryCreatesAMapFromASizeAndPosition()
        {
            // Given
            Position startingPosition = new Position
            {
                H = 0,
                X = 0,
                Y = 0,
                Z = 0
            };

            IZoneMap expectedZoneMap = new ZoneMap();
            expectedZoneMap.Spots = new List<ISpot>
            {
                // start
                new Spot {X = 0, Y = 0, Z = 0},

                // up
                new Spot {X = 0, Y = 0, Z = 1},
                // upper right
                new Spot {X = 1, Y = 0, Z = 1},
                // right middle
                new Spot {X = 1, Y = 0, Z = 0},
                // down right
                new Spot {X = 1, Y = 0, Z = -1},
                // down
                new Spot {X = 0, Y = 0, Z = -1},
                // down left
                new Spot {X = -1, Y = 0, Z = -1},
                // left middle
                new Spot {X = -1, Y = 0, Z = 0},
                // upper left
                new Spot {X = -1, Y = 0, Z = 1},
            };

            // When
            IZoneMap actualZoneMap = ZoneMapFactory.GenerateZoneMap(startingPosition, 1);

            // Then
            Assert.Equal(expectedZoneMap, actualZoneMap);
        }

        [Fact]
        public void ZoneMapFactoryCreatesAMapFromASizeAndPositionCanMakeABiggerMap()
        {
            // Given
            Position startingPosition = new Position
            {
                H = 0,
                X = 0,
                Y = 0,
                Z = 0
            };

            IZoneMap expectedZoneMap = new ZoneMap();
            expectedZoneMap.Spots = new List<ISpot>
            {
                // start
                new Spot {X = 0, Y = 0, Z = 0},

                // up
                new Spot {X = 0, Y = 0, Z = 1},
                new Spot {X = 0, Y = 0, Z = 2},
                new Spot {X = 0, Y = 0, Z = 3},

                // upper right
                new Spot {X = 1, Y = 0, Z = 1},

                new Spot {X = 1, Y = 0, Z = 2},
                new Spot {X = 2, Y = 0, Z = 1},
                new Spot {X = 2, Y = 0, Z = 2},

                new Spot {X = 3, Y = 0, Z = 1},
                new Spot {X = 3, Y = 0, Z = 2},
                new Spot {X = 1, Y = 0, Z = 3},
                new Spot {X = 2, Y = 0, Z = 3},
                new Spot {X = 3, Y = 0, Z = 3},

                // right middle
                new Spot {X = 1, Y = 0, Z = 0},
                new Spot {X = 2, Y = 0, Z = 0},
                new Spot {X = 3, Y = 0, Z = 0},

                // down right
                new Spot {X = 1, Y = 0, Z = -1},

                new Spot {X = 1, Y = 0, Z = -2},
                new Spot {X = 2, Y = 0, Z = -1},
                new Spot {X = 2, Y = 0, Z = -2},

                new Spot {X = 3, Y = 0, Z = -1},
                new Spot {X = 3, Y = 0, Z = -2},
                new Spot {X = 1, Y = 0, Z = -3},
                new Spot {X = 2, Y = 0, Z = -3},
                new Spot {X = 3, Y = 0, Z = -3},

                // down
                new Spot {X = 0, Y = 0, Z = -1},
                new Spot {X = 0, Y = 0, Z = -2},
                new Spot {X = 0, Y = 0, Z = -3},

                // down left
                new Spot {X = -1, Y = 0, Z = -1},

                new Spot {X = -1, Y = 0, Z = -2},
                new Spot {X = -2, Y = 0, Z = -1},
                new Spot {X = -2, Y = 0, Z = -2},

                new Spot {X = -3, Y = 0, Z = -1},
                new Spot {X = -3, Y = 0, Z = -2},
                new Spot {X = -1, Y = 0, Z = -3},
                new Spot {X = -2, Y = 0, Z = -3},
                new Spot {X = -3, Y = 0, Z = -3},

                // left middle
                new Spot {X = -1, Y = 0, Z = 0},
                new Spot {X = -2, Y = 0, Z = 0},
                new Spot {X = -3, Y = 0, Z = 0},

                // upper left
                new Spot {X = -1, Y = 0, Z = 1},

                new Spot {X = -1, Y = 0, Z = 2},
                new Spot {X = -2, Y = 0, Z = 1},
                new Spot {X = -2, Y = 0, Z = 2},

                new Spot {X = -3, Y = 0, Z = 1},
                new Spot {X = -3, Y = 0, Z = 2},
                new Spot {X = -1, Y = 0, Z = 3},
                new Spot {X = -2, Y = 0, Z = 3},
                new Spot {X = -3, Y = 0, Z = 3},
            };

            // When
            IZoneMap actualZoneMap = ZoneMapFactory.GenerateZoneMap(startingPosition, 3);

            // Then
            Assert.Equal(expectedZoneMap, actualZoneMap);
        }


        [Fact]
        public void TestHugeMap()
        {
            // Given
            Position startingPosition = new Position
            {
                H = 0,
                X = 0,
                Y = 0,
                Z = 0
            };

            // When
            IZoneMap actualZoneMap = ZoneMapFactory.GenerateZoneMap(startingPosition, 1000);

            // Then
            Assert.IsType<ZoneMap>(actualZoneMap);
        }
    }


    
}