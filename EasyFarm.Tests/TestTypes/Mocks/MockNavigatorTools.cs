using MemoryAPI;
using MemoryAPI.Navigation;

namespace EasyFarm.Tests.TestTypes.Mocks
{
    public class MockNavigatorTools : INavigatorTools
    {
        public double DistanceTolerance { get; set; }
        public bool IsRunning { get; set; }

        public void FaceHeading(Position position)
        {
        }

        public void GotoWaypoint(Position position, bool useObjectAvoidance, bool keepRunning)
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