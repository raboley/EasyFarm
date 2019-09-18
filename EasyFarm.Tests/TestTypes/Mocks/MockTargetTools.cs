using MemoryAPI;

namespace EasyFarm.Tests.TestTypes.Mocks
{
    public class MockTargetTools : ITargetTools
    {
        public int LastTargetID { get; set; }

        public int ID { get; set; }

        public bool SetNPCTarget(int index)
        {
            LastTargetID = index;
            return true;
        }
    }
}