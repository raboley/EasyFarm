using System.Collections.Generic;
using System.Linq;
using MemoryAPI;
using MemoryAPI.Navigation;

namespace EasyFarm.Tests.TestTypes.Mocks
{
    public class MockNPCTools : INPCTools
    {
        public MockNPCTools()
        {
            InitializeEntities();
        }

        private void InitializeEntities()
        {
            Entities = Enumerable.Range(0, Constants.UnitArrayMax)
                .Select(x => new KeyValuePair<int, MockNPC>(x, new MockNPC()))
                .ToDictionary(x => x.Key, x => x.Value);
        }

        public void AddOrUpdateEntity(int index, MockNPC entity)
        {
            if (Entities.ContainsKey(index))
                Entities[index] = entity;
            else
                Entities.Add(index, entity);
        }

        public Dictionary<int, MockNPC> Entities { get; set; } = new Dictionary<int, MockNPC>();

        public int ClaimedID(int id)
        {
            return Entities[id].ClaimID;
        }

        public double Distance(int id)
        {
            return Entities[id].Distance;
        }

        public Position GetPosition(int id)
        {
            return Entities[id].Position;
        }

        public short HPPCurrent(int id)
        {
            return Entities[id].HealthPercent;
        }

        public bool IsActive(int id)
        {
            return Entities[id].IsActive;
        }

        public bool IsClaimed(int id)
        {
            return Entities[id].IsClaimed;
        }

        public bool IsRendered(int id)
        {
            return Entities[id].IsRendered;
        }

        public string Name(int id)
        {
            return Entities[id].Name;
        }

        public NpcType NPCType(int id)
        {
            return Entities[id].NPCType;
        }

        public float PosX(int id)
        {
            return Entities[id].PosX;
        }

        public float PosY(int id)
        {
            return Entities[id].PosY;
        }

        public float PosZ(int id)
        {
            return Entities[id].PosZ;
        }

        public Status Status(int id)
        {
            return Entities[id].Status;
        }

        public int PetID(int id)
        {
            return Entities[id].PetID;
        }
    }
}