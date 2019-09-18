using MemoryAPI;

namespace EasyFarm.Tests.TestTypes.Mocks
{
    public class MockTimerTools : ITimerTools
    {
        public int RecastTime { get; set; }

        public int GetAbilityRecast(int index)
        {
            return RecastTime;
        }

        public int GetSpellRecast(int index)
        {
            return RecastTime;
        }
    }
}