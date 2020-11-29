namespace EasyFarm.Missions
{
    public class NoInstantCompletion : ICompleteRightNowCondition
    {
        
        public bool CanCompleteNow()
        {
            return false;
        }
    }
}