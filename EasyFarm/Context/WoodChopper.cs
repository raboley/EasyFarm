using System.Collections.Generic;
using System.Threading;
using EasyFarm.Classes;
using EasyFarm.ViewModels;
using Pathfinder.People;

namespace EasyFarm.Context
{
    public class WoodChopper
    {
        public Queue<Person> LoggingPoints { get; set; } = new Queue<Person>();
        public Person NextPoint { get; set; }
        public string ChopWoodZone = "Ronfaure_East";

        public static void ChopTree(IGameContext context, IUnit loggingUnit)
        {
            context.Target = loggingUnit;
            // Face mob. 
            context.API.Navigator.FaceHeading(context.Target.Position);
            context.API.Navigator.GotoNPC(context.Target.Id, context.Config.IsObjectAvoidanceEnabled);

            // Target mob if not currently targeted. 
            Classes.Player.SetTarget(context.API, context.Target);

            LogViewModel.Write("Chopping down tree at: " + context.Target.Position);
            context.API.Windower.SendString("/item Hatchet <t>");
            Thread.Sleep(4000);
        }

        public void GoToChopWoodZone(IGameContext context)
        {
            LogViewModel.Write("Going to zone: " + ChopWoodZone);
            while (context.Traveler?.World?.Zones == null)
            {
                Thread.Sleep(100);
            }

            context.Traveler.WalkToZone(ChopWoodZone);
        }
    }
}