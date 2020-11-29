using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EasyFarm.Classes;
using EasyFarm.Context;
using Pathfinder.Travel;

namespace EasyFarm.Missions
{
    public interface IQuest
    {
        bool Repeatable { get; set; }
        bool Completed { get; }
        void Do();
        void MarkStepAsDone(IQuestStep questStep);
        ICompleteRightNowCondition CanComplete { get; set; }
    }

    public class Quest : IQuest
    {
        List<IQuestStep> ToDoSteps = new List<IQuestStep>();
        List<IQuestStep> DoneSteps = new List<IQuestStep>();
        private IGameContext _context;
        private IInventory _inventory;
        private Traveler _traveler;

        public bool Repeatable { get; set; } = true;

        public bool Completed => ToDoSteps.Count == 0;


        public ICompleteRightNowCondition CanComplete { get; set; }

        public Quest(IGameContext context, List<IQuestStep> questSteps, ICompleteRightNowCondition canComplete = null)
        {
            _context = context;
            ToDoSteps = questSteps;
            
            if (canComplete == null)
               CanComplete = new NoInstantCompletion(); 
            else 
                CanComplete = canComplete;
        }

        public void Do()
        {
            while (!Completed)
            {
                var questStepsInProgress = new List<IQuestStep>();

                if (CanComplete.CanCompleteNow())
                {
                    var lastQuestStep = questStepsInProgress.LastOrDefault();
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


                    questStep.InProgress = true;
                    while (questStep.InProgress)
                    {
                        // DoStep should change questStep to IsDone when 
                        // all it is completed.
                        questStep.DoStep();
                    }

                    MarkStepAsDone(questStep);
                }
            }
        }

        public void MarkStepAsDone(IQuestStep questStep)
        {
            ToDoSteps.Remove(questStep);
            DoneSteps.Add(questStep);
        }

        public static IQuest SandyOneOne(IGameContext context)
        {
            var todoList = new List<IQuestStep>
            {
                new TalkToClosestGuardInSandy(context),
                // get an orcish axe
                new GiveAxeToGuardInSandy(context)
            };
            
            var sandyCompletion = new SandyOneOneCompletion();
            sandyCompletion.Context = context;

            return new Quest(context, todoList, sandyCompletion);
        }
    }

    public class NoInstantCompletion : ICompleteRightNowCondition
    {
        
        public bool CanCompleteNow()
        {
            return false;
        }
    }
    public class SandyOneOneCompletion : ICompleteRightNowCondition
    {
        public IGameContext Context;
        
        public bool CanCompleteNow()
        {
            return Context.Inventory.HaveItemInInventoryContainer("Orcish Axe");
        }
    }

    public class TalkToClosestGuardInSandy : BaseQuestStep
    {
        private const string GateGuardName = "Ambrotien";

        public override Completed IsDoneCheck()
        {
            // How to make this work :/ 
            WalkToGateGuardAndTalk();

            if (TooFarAwayToTalkToGateGuard())
                return Completed.Inconclusive;

            var said = _context.API.Chat.LastThingSaid();
            if (said.Contains("Finish your current mission first"))
                return Completed.Done;

            return Completed.NotDone;
        }

        public override void DoStep()
        {
            WalkToGateGuardAndTalk();

            if (TooFarAwayToTalkToGateGuard())
                return;

            if (IsDoneCheck() == Completed.Done)
            {
                InProgress = false;
                IsDone = true;
                return;
            }

            _talk.RespondWith(_context, "A mission, please.");
            _talk.RespondWith(_context, "Tell me more.");
            // Press enter when it says Orcish axe
            _talk.RespondWith(_context, "I accept.");

            if (IsDoneCheck() == Completed.Done)
            {
                InProgress = false;
                IsDone = true;
            }
        }

        private void WalkToGateGuardAndTalk()
        {
            _talk.WalkAndTalkToPersonByName(_context, GateGuardName);
        }

        private bool TooFarAwayToTalkToGateGuard()
        {
            return _traveler.AmNotWithinTalkingDistanceToPersonByName(GateGuardName);
        }


        public TalkToClosestGuardInSandy(IGameContext context) : base(context)
        {
        }
    }

    public class GiveAxeToGuardInSandy : BaseQuestStep
    {
        public override void DoStep()
        {
            WalkToGateGuard();

            if (TooFarAwayToTalkToGateGuard())
                return;

            TradeAxeToGateGuard();
        }

        private void TradeAxeToGateGuard()
        {
            var items = new List<ItemsToTrade>
            {
                new ItemsToTrade
                {
                    Name = "Orcish Axe",
                    NumberToTrade = 1
                }
            };

            _trade.TradeItemsToPersonByName(_context, GateGuardName, items);

            WaitForCutsceneToEnd();
            MarkQuestStepCompleted();
        }

        private void WaitForCutsceneToEnd()
        {
            while (!_context.API.Chat.LastThingSaid().Contains("your work is done."))
            {
                Thread.Sleep(100);
            }
        }

        private void WalkToGateGuard()
        {
            _traveler.WalkAcrossWorldToNpcByName(GateGuardName);
        }

        private const string GateGuardName = "Ambrotien";

        private bool TooFarAwayToTalkToGateGuard()
        {
            return _traveler.AmNotWithinTalkingDistanceToPersonByName(GateGuardName);
        }

        public GiveAxeToGuardInSandy(IGameContext context) : base(context)
        {
        }
    }
}


