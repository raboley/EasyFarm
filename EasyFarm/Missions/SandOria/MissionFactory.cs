using System.Collections.Generic;
using EasyFarm.Context;
using EasyFarm.Missions.SandOria;

namespace EasyFarm.Missions
{
    public static class MissionFactory
    {
        public static IQuest SandyOneOne(IGameContext context)
        {
            var todoList = new List<IQuestStep>
            {
                new TalkToClosestGuardInSandy(context),
                // get an orcish axe
                new GiveAxeToGuardInSandy(context)
            };

            var sandyCompletion = new SandyOneOneCompletion {Context = context};

            return new Quest(todoList, sandyCompletion);
        }
    }
}