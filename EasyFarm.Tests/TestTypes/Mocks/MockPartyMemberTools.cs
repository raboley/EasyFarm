using MemoryAPI;

namespace EasyFarm.Tests.TestTypes.Mocks
{
    public class MockPartyMemberTools : IPartyMemberTools
    {
        public bool UnitPresent { get; set; }
        public int ServerID { get; set; }
        public string Name { get; set; }
        public int HPCurrent { get; set; }
        public int HPPCurrent { get; set; }
        public int MPCurrent { get; set; }
        public int MPPCurrent { get; set; }
        public int TPCurrent { get; set; }
        public Job Job { get; set; }
        public Job SubJob { get; set; }
        public NpcType NpcType { get; set; }
    }
}