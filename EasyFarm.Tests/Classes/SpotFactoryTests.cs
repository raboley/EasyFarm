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
    public class SpotFactoryTests
    {
        [Fact]
        public void SpotFactoryCanCreateASpotFromAPosition()
        {
            // given
            ISpot expectedSpot = new Spot
            {
                X = 1,
                Y = 1,
                Z = 1
            };
            Position testPosition = new Position { H = 1, X = 1, Y = 1, Z = 1 };
            // when
            ISpot actualSpot = SpotFactory.SpotFromPosition(testPosition);

            // Then
            Assert.Equal(expectedSpot, actualSpot);
        }

        [Fact]
        public void SpotFactoryAlwaysRoundsFloatsDown()
        {
            // given
            ISpot expectedSpot = new Spot
            {
                X = 1,
                Y = 1,
                Z = 1
            };
            Position testPosition = new Position { H = 1, X = (float)1.99, Y = (float)1.5, Z = (float)1.1 };
            // when
            ISpot actualSpot = SpotFactory.SpotFromPosition(testPosition);

            // Then
            Assert.Equal(expectedSpot, actualSpot);
        }

        [Fact]
        public void SpotFactoryAlwaysRoundsFloatsDownWhenNegative()
        {
            // given
            ISpot expectedSpot = new Spot
            {
                X = -1,
                Y = -10,
                Z = -123
            };
            Position testPosition = new Position { H = 1, X = (float)-1.99, Y = (float)-10.8, Z = (float)-123.9 };
            // when
            ISpot actualSpot = SpotFactory.SpotFromPosition(testPosition);

            // Then
            Assert.Equal(expectedSpot, actualSpot);
        }

        [Fact]
        public void SpotFactoryAddsDistanceFromStartingPositionToX()
        {
            // given
            ISpot expectedSpot = new Spot
            {
                X = 5,
                Y = 0,
                Z = 0
            };

            // when
            ISpot spot = new Spot
            {
                X = 0,
                Y = 0,
                Z = 0
            };
            ISpot actualSpot = SpotFactory.SpotFromSpotOffset(spot, 5, 0, 0);

            // Then
            Assert.Equal(expectedSpot, actualSpot);
        }

        [Fact]
        public void SpotFactoryAddsDistanceFromStartingPositionToZ()
        {
            // given
            ISpot expectedSpot = new Spot
            {
                X = 0,
                Y = 0,
                Z = 3
            };

            // when
            ISpot spot = new Spot
            {
                X = 0,
                Y = 0,
                Z = 0
            };
            ISpot actualSpot = SpotFactory.SpotFromSpotOffset(spot, zOffset:3);

            // Then
            Assert.Equal(expectedSpot, actualSpot);
        }

        [Fact]
        public void SpotFactoryAddsDistanceFromStartingPositionToXAndZ()
        {
            // given
            ISpot expectedSpot = new Spot
            {
                X = 5,
                Y = 0,
                Z = 3
            };

            // when
            ISpot spot = new Spot
            {
                X = 0,
                Y = 0,
                Z = 0
            };
            ISpot actualSpot = SpotFactory.SpotFromSpotOffset(spot, 5, 0, 3);

            // Then
            Assert.Equal(expectedSpot, actualSpot);
        }
    }
}
