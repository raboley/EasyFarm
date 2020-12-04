using System.Collections.Generic;

namespace FinalFantasyXI.Missions.SandOria
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