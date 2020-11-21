using System.Threading;
using EasyFarm.Context;
using EasyFarm.ViewModels;
using Pathfinder.Travel;

namespace EasyFarm.States
{
    public class GoFight : BaseState
    {
        private string _fightZone = "Gustaberg_South";

        public override bool Check(IGameContext context)
        {
            if (context.Zone.Name != _fightZone)
                return true;

            return false;
        }

        public override void Run(IGameContext context)
        {
            LogViewModel.Write("Going to go Fight at: " + _fightZone );
            while (context.Traveler == null)
            {
                Thread.Sleep(100); 
            }
            
            context.Traveler.GoToZone(_fightZone);
        }
    }
}