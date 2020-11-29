using EasyFarm.Classes;
using EasyFarm.Context;
using MemoryAPI.Chat;
using Pathfinder.Travel;

namespace EasyFarm.Missions
{
    public abstract class BaseQuestStep : IQuestStep
    {

        protected Traveler _traveler
        {
            get => _context.Traveler;
        }
        protected IDialog _talk;
        protected IGameContext _context;
        protected ITradeMenu _trade;
        protected IChatTools _chat;
        protected IInventory _inventory;

        protected BaseQuestStep(IGameContext context)
        {
            _context = context;
            _talk = context.Dialog;
            _trade = context.Trade;
            _chat = context.API.Chat;
            _inventory = context.Inventory;
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