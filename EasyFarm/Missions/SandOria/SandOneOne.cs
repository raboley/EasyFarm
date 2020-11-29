using System.Collections.Generic;
using System.Threading;
using EasyFarm.Context;

namespace EasyFarm.Missions.SandOria
{
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
            MarkQuestStepCompleted();
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


