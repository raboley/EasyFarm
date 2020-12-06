using System;
using System.Threading;
using EasyFarm.Context;
using EasyFarm.ViewModels;

namespace EasyFarm.States
{
    public class GoFight : BaseState
    {

        public override bool Check(IGameContext context)
        {
            string fightZone = GetFightZoneByNation(context);
            
            if (context.PathfinderZone.Name != fightZone)
                return true;

            return false;
        }

        public override void Run(IGameContext context)
        {
            string fightZone = GetFightZoneByNation(context);
            
            LogViewModel.Write("Going to go Fight at: " + fightZone );
            while (context.Traveler == null)
            {
                Thread.Sleep(100); 
            }
            
            context.Traveler.WalkToZone(fightZone);
        }

        private string GetFightZoneByNation(IGameContext context)
        {
            var nation = context.API.Player.Nation.ToString();
            if (nation == "Bastok")
                return "Gustaberg_South";
            if (nation == "SandOria")
                return "Ronfaure_East";
            // if (nation == "Windurst")
            //     return "T.K.";

            throw new Exception("Don't know the NPC pattern for nation: " + nation);
        }
    }
}