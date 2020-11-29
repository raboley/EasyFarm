using EasyFarm.Classes;
using EasyFarm.Context;
using MemoryAPI.Memory.EliteMMOWrapper;
using Pathfinder.Travel;

namespace EasyFarm.Missions
{
    public abstract class BaseQuestStep : IQuestStep
    {
        protected Traveler _traveler;
        protected IDialog _talk;
        protected IGameContext _context;
        protected ITradeMenu _trade;

        protected BaseQuestStep(IGameContext context)
        {
            _context = context;
            _traveler = context.Traveler;
            _talk = context.Dialog;
            _trade = context.Trade;
        }

        public virtual void DoStep()
        {
            throw new System.NotImplementedException();
        }

        public virtual Completed IsDoneCheck()
        {
            return Completed.NotDone;
        }

        public void MarkQuestStepCompleted()
        {
            InProgress = false;
            IsDone = true;
        }

        public bool InProgress { get; set; }
        public bool IsDone { get; set; }
    }
}