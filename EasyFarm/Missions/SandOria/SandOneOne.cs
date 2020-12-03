using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EasyFarm.Context;
using EliteMMO.API;

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
            AcceptSandyOneOneQuest();

            if (IsDoneCheck() == Completed.Done)
            {
                InProgress = false;
                IsDone = true;
            }
        }

        private void AcceptSandyOneOneQuest()
        {
            // Press enter when it says Orcish axe
            while (!_chat.LastThingSaid().Contains("Orcish"))
            {
                Thread.Sleep(100);
            }
            _context.API.Windower.SendKeyPress(Keys.NUMPADENTER);
            _talk.RespondWith(_context, "I accept.");
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

            var tradeFailed = true;
            while (tradeFailed)
            {
                var thingSaidBeforeTrading = _chat.ChatEntries.LastOrDefault();
                _trade.TradeItemsToPersonByName(_context, GateGuardName, items);
                var needToStartQuestAgain =
                    _chat.WaitToSeeIfStatementWasSaid("before you bring me something", thingSaidBeforeTrading, 5);

                if (needToStartQuestAgain)
                {
                    StartSandyOneOneAgain();
                }
                else
                {
                    tradeFailed = false;
                }
            }


            WaitForCutsceneToEnd();
        }

        private void WaitForCutsceneToEnd()
        {
            while (!_context.API.Chat.LastThingSaid().Contains("your work is done."))
            {
                Thread.Sleep(100);
            }
        }

        private void StartSandyOneOneAgain()
        {
            var responses = new List<string>
            {
                "Smash the Orc",
                "accept",
            };
            _context.Dialog.HaveConversationWithPerson(_context, GateGuardName, responses);
        }

        private void AcceptSandyOneOneQuest()
        {
            // Press enter when it says Orcish axe
            while (!_chat.LastThingSaid().Contains("Orcish"))
            {
                Thread.Sleep(100);
            }
            _context.API.Windower.SendKeyPress(Keys.NUMPADENTER);
            _talk.RespondWith(_context, "I accept.");
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