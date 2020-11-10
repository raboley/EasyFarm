using MemoryAPI;
using MemoryAPI.Navigation;
using Pathfinder.Map;

namespace EasyFarm.Tests.TestTypes.Mocks
{
    public class MockNavigatorTools : INavigatorTools
    {
        public double DistanceTolerance { get; set; }
        public bool IsRunning { get; set; }

        public void FaceHeading(Position position)
        {
        }

        public bool GotoWaypoint(Position position, bool useObjectAvoidance, bool keepRunning, ZoneMap zoneMap)
        {
            throw new System.NotImplementedException();
        }

        public void GotoNPC(int ID, bool useObjectAvoidance)
        {
            throw new System.NotImplementedException();
        }

        public void Reset()
        {
            IsRunning = false;
        }
    }
}