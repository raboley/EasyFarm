using System.Collections.Generic;
using EliteMMO.API;
using MemoryAPI;
using MemoryAPI.Navigation;
using StatusEffect = MemoryAPI.StatusEffect;

namespace EasyFarm.Tests.TestTypes.Mocks
{
    public class MockPlayerTools : IPlayerTools
    {
        public float CastPercentEx { get; set; }
        public int HPPCurrent { get; set; }
        public int ID { get; set; }
        public int MPCurrent { get; set; }
        public int MPPCurrent { get; set; }
        public string Name { get; set; }
        public Position Position { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public Structures.PlayerStats Stats { get; set; }
        public Status Status { get; set; }
        public StatusEffect[] StatusEffects { get; set; } = new StatusEffect[0];
        public int TPCurrent { get; set; }
        public Zone Zone { get; set; }
        public Job Job { get; set; }
        public Job SubJob { get; set; }

        public int JobLevel { get; set; }

        public int SubJobLevel { get; set; }

        public List<EliteAPI.InventoryItem> Equipment { get; set; }

        public int MeritPoints => throw new System.NotImplementedException();

        public Zone HomePoint => throw new System.NotImplementedException();
        public Nations Nation { get; }
    }
}