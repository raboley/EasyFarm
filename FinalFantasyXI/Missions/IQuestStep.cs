namespace FinalFantasyXI.Missions
{
    public interface IQuestStep
    {
        void DoStep();
        Completed IsDoneCheck();
        bool InProgress { get; set; }
        bool IsDone { get; set; }
    }
}