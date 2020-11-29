namespace EasyFarm.Missions
{
    public interface IQuest
    {
        bool Repeatable { get; set; }
        bool Completed { get; }
        void Do();
        void MarkStepAsDone(IQuestStep questStep);
        ICompleteRightNowCondition CanComplete { get; set; }
    }
}