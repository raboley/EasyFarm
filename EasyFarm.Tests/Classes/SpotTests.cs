using EasyFarm.Mapping;
using MemoryAPI.Navigation;
using Xunit;

namespace EasyFarm.Tests.Classes
{
    public class SpotTests
    {
        [Fact]
        public void EqualityWorksWhenSpotsAreTheSame()
        {
            // given
            ISpot expectedSpot = new Spot
            {
                X = 1,
                Y = 1,
                Z = 1
            };
            Position testPosition = new Position {H = 1, X = 1, Y = 1, Z = 1};
            // when
            ISpot actualSpot = new Spot
            {
                X = 1,
                Y = 1,
                Z = 1
            };

            // Then
            Assert.Equal(expectedSpot, actualSpot);
        }

        [Fact]
        public void NullSpotIsNotEqualToARealSpot()
        {
            // given
            ISpot expectedSpot = new Spot
            {
                X = 1,
                Y = 1,
                Z = 1
            };
            ISpot actualSpot = new Spot();

            // Then
            Assert.NotEqual(expectedSpot, actualSpot);
        }

        [Fact]
        public void DifferentSpotsAreNotEqual()
        {
            // given
            ISpot expectedSpot = new Spot
            {
                X = 2,
                Y = 1,
                Z = 1
            };
            Position testPosition = new Position {H = 1, X = 1, Y = 1, Z = 1};
            // when
            ISpot actualSpot = SpotFactory.SpotFromPosition(testPosition);

            // Then
            Assert.NotEqual(expectedSpot, actualSpot);
        }


        [Fact]
        public void SpotsCanBeWalls()
        {
            // given
            ISpot expectedSpot = new Spot
            {
                X = 1,
                Y = 1,
                Z = 1,
                F = 0,
                G = 0,
                H = 1,
                Neighbors = null,
                Previous = null,
                IsWall = true
            };
            Position testPosition = new Position { H = 1, X = 1, Y = 1, Z = 1 };

            // When
            ISpot actualSpot = SpotFactory.SpotFromPosition(testPosition);
            actualSpot.IsWall = true;

            // then
            Assert.Equal(expectedSpot, actualSpot);


        }

    }
}