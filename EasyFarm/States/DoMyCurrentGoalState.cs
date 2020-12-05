using EasyFarm.Context;
using EasyFarm.Soul;

namespace EasyFarm.States
{
    public class DoMyCurrentGoalState : BaseState
    {
        private readonly ICalling _calling;

        public DoMyCurrentGoalState(IGameContext context,ICalling calling)
        {
            _calling = calling;
            // _calling = CallingFactory.JuniorWoodWorker(context, calling.Objectives);


        }

        public override bool Check(IGameContext context)
        {
            if (context.Traveler?.CurrentZone?.Map == null)
                return false;
            
            if (new RestState().Check(context)) return false;

            if (new NeedSignet().Check(context)) return false;

            if (new SellSomeJunk().Check(context)) return false;

            // if (new DoQuest().Check(context)) return false;
            if (new CraftSomething().Check(context)) return false;
            
            if (context.Player.IsDead) return false;

            if (context.Player.HasAggro) return false;
            
            if (context.Target.IsValid) return false; 
            
            return true;
        }

        public override void Run(IGameContext context)
        {
            _calling.Do();
        }
    }
}