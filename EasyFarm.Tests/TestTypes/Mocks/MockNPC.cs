using MemoryAPI;
using MemoryAPI.Navigation;

namespace EasyFarm.Tests.TestTypes.Mocks
{
    public class MockNPC
    {
        public int ClaimID { get; set; }
        public double Distance { get; set; }
        public Position Position { get; set; } = new Position();
        public short HealthPercent { get; set; }
        public bool IsActive { get; set; }
        public bool IsClaimed { get; set; }
        public bool IsRendered { get; set; }
        public string Name { get; set; }
        public NpcType NPCType { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public Status Status { get; set; }
        public int PetID { get; set; }
    }
}