using Xunit;

namespace EasyFarm.Tests.Pathfinding
{
    public class MappingConversionsTests
    {
        [Fact]
        public void ConvertFromFloatToIntCanRoundAndConvertCorrectly()
        {
            int expected = 1;
            float startingFloat = 0.51f;

            int actual = MappingConversion.ConvertFromFloatToInt(startingFloat);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConvertFromFloatToIntRoundsDownWhenLessThanFive()
        {
            int expected = 0;
            float startingFloat = 0.4999f;

            int actual = MappingConversion.ConvertFromFloatToInt(startingFloat);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConvertFromFloatToIntCanRoundAndConvertNegativeCorrectly()
        {
            int expected = -1;
            float startingFloat = -0.51f;

            int actual = MappingConversion.ConvertFromFloatToInt(startingFloat);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConvertFromFloatToIntRoundsDownWhenLessThanFiveNegative()
        {
            int expected = -0;
            float startingFloat = -0.4999f;

            int actual = MappingConversion.ConvertFromFloatToInt(startingFloat);

            Assert.Equal(expected, actual);
        }
    }
}
