using System.Collections.Generic;
using System.Linq;
using EasyFarm.Classes;
using EasyFarm.Context;
using Pathfinder.Travel;

namespace EasyFarm.Missions
{
    public class Quest : IQuest
    {
        List<IQuestStep> ToDoSteps;
        List<IQuestStep> DoneSteps = new List<IQuestStep>();

        public bool Repeatable { get; set; } = true;

        public bool Completed => ToDoSteps.Count == 0;


        public ICompleteRightNowCondition CanComplete { get; set; }

        public Quest(List<IQuestStep> questSteps, ICompleteRightNowCondition canComplete = null)
        {
            ToDoSteps = questSteps;

            if (canComplete == null)
                CanComplete = new NoInstantCompletion();
            else
                CanComplete = canComplete;
        }

        public void Do()
        {
            var questStepsInProgress = new List<IQuestStep>();

            if (CanComplete.CanCompleteNow())
            {
                var lastQuestStep = ToDoSteps.LastOrDefault();
                if (lastQuestStep == null)
                    return;
                questStepsInProgress.Add(lastQuestStep);
            }
            else
            {
                questStepsInProgress.AddRange(ToDoSteps);
            }


            foreach (var questStep in questStepsInProgress)
            {
                if (!questStep.InProgress)
                {
                    if (questStep.IsDone)
                        continue;

                    if (questStep.IsDoneCheck() == Missions.Completed.Done)
                    {
                        MarkStepAsDone(questStep);
                        continue;
                    }
                }


                // questStep.InProgress = true;
                // while (questStep.InProgress)
                // {
                // DoStep should change questStep to IsDone when 
                // all it is completed.
                questStep.DoStep();
                // questStep.InProgress = false;
                // }

                MarkStepAsDone(questStep);
            }
        }

        public void MarkStepAsDone(IQuestStep questStep)
        {
            ToDoSteps.Remove(questStep);
            DoneSteps.Add(questStep);
        }
    }
}