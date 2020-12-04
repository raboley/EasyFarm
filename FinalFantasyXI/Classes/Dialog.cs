using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EliteMMO.API;
using FinalFantasyXI.Context;
using MemoryAPI;

namespace FinalFantasyXI.Classes
{
    class Dialog : IDialog
    {
        private IMemoryAPI _memory;

        public Dialog(IMemoryAPI memory)
        {
            _memory = memory;
        }

        public void HaveConversationWithPerson(IGameContext context, string personName, List<string> responses)
        {
            var lastChatEntry = context.Dialog.LastThingAnNpcSaid(context);
            // Talk to NPC 
            context.Dialog.TalkToPersonByName(context, personName);
            var convoType = context.Dialog.IsInADialogOrConversationWithNpc(context, lastChatEntry, 5);
            if (convoType == NpcTalkStyle.Question)
            {
                foreach (var response in responses)
                {
                    context.Dialog.RespondWith(context, response);
                }
            }
            else
            {
                ResumeUntilNothingElseSaid(context);
            }
        }

        public void ResumeToNextDialog(IGameContext context)
        {
            while (!context.Dialog.AmInDialog(context, 1))
            {
                var lastChatEntry = LastThingAnNpcSaid(context);
                if (LastThingAnNpcSaid(context) == lastChatEntry)
                    context.Menu.Enter();

                Thread.Sleep(500);
            }
        }

        public void ResumeUntilNothingElseSaid(IGameContext context)
        {
            while (true)
            {
                var lastChatEntry = LastThingAnNpcSaid(context);
                if (LastThingAnNpcSaid(context) == lastChatEntry)
                    context.Menu.Enter();

                Thread.Sleep(500);
                if (LastThingAnNpcSaid(context) == lastChatEntry)
                    break;
            } 
        }

        public EliteAPI.ChatEntry LastThingAnNpcSaid(IGameContext context)
        {
            return context.API.Chat.ChatEntries.ToList().FindLast(x => x.ChatType == 152);
        }

        public bool AmInDialog(IGameContext context, int secondsToWait = 3)
        {
            var lastDialogIndex = context.API.Dialog.DialogIndex;

            DateTime duration = DateTime.Now.AddSeconds(secondsToWait);
            while (DateTime.Now < duration)
            {
                context.Menu.Down();
                Thread.Sleep(300);
                if (lastDialogIndex != context.API.Dialog.DialogIndex)
                    return true;

                context.Menu.Up();
                Thread.Sleep(300);
                if (lastDialogIndex != context.API.Dialog.DialogIndex)
                    return true;
            }

            return false;
        }

        public void ChooseDialogOptions(IGameContext context, List<string> desiredOptionTexts)
        {
            foreach (string desiredOptionText in desiredOptionTexts)
            {
                SelectDialogOption(context, desiredOptionText);
            }
        }

        public void SelectDialogOption(IGameContext context, string desiredOptionText)
        {
            int i = 0;

            while (i <= 10)
            {
                var dialog = context.Memory.EliteApi.Dialog.GetDialog();
                List<string> options = dialog.Options;
                int desiredIndex = options.IndexOf(desiredOptionText);
                if (desiredIndex == -1)
                {
                    if (i == 10)
                    {
                        LogViewModel.Write(
                            $"Error occurred when selecting dialog options. Couldn't find dialog option '{desiredOptionText}' for question: '{dialog.Question}'");
                        return;
                    }

                    i++;
                    TimeWaiter.Pause(500);
                    continue;
                }

                MoveToDesiredDialogOption(context, desiredIndex);

                context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
                return;
            }
        }

        public void MoveToDesiredDialogOption(IGameContext context, int desiredIndex)
        {
            int circuitBreaker = 0;
            while (desiredIndex != context.Memory.EliteApi.Dialog.DialogIndex)
            {
                var dialog = context.Memory.EliteApi.Dialog.GetDialog();
                var hash = context.Memory.EliteApi.Dialog.GetHashCode();
                if (desiredIndex > context.Memory.EliteApi.Dialog.DialogIndex)
                {
                    context.API.Windower.SendKeyPress(EliteMMO.API.Keys.DOWN);
                }
                else
                {
                    context.API.Windower.SendKeyPress(EliteMMO.API.Keys.UP);
                }

                TimeWaiter.Pause(100);

                //// some menus have hidden options, but all show up in the returned options, so need to also check that current index text doesn't equal to desired text
                //if(desiredOptionText == context.Memory.EliteApi.Dialog.GetDialog().RawDialog)
                //{
                //    return;
                //}

                if (circuitBreaker == 50)
                    break;
                circuitBreaker++;
            }
        }

        public void ExitDialog(IGameContext context)
        {
            for (int i = 0; i < 5; i++)
            {
                context.API.Windower.SendKeyPress(EliteMMO.API.Keys.ESCAPE);
                TimeWaiter.Pause(200);
            }
        }

        public void TalkToPersonByName(IGameContext context, string name)
        {
            // Move this
            var unit = context.Memory.UnitService.GetUnitByName(name);
            if (unit == null)
                return;

            context.Target = unit;
            Player.SetTarget(context.API, unit);

            // Talk
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            TimeWaiter.Pause(1000);
        }

        public void RespondWith(IGameContext context, string response)
        {
            // while (context.API.Dialog.DialogInfo.Options.FirstOrDefault(x => x.ToLower().Contains(response.ToLower())) == null)
            // {
            // context.API.Windower.SendKeyPress(Keys.NUMPADENTER);
            // Thread.Sleep(1000);
            ResumeToNextDialog(context);
            // }

            var exactResponse =
                Enumerable.FirstOrDefault<string>(context.API.Dialog.DialogInfo.Options, x => x.ToLower().Contains(response.ToLower()));
            SelectDialogOption(context, exactResponse);
        }

        public void WalkAndTalkToPersonByName(IGameContext context, string name)
        {
            context.Traveler.WalkAcrossWorldToNpcByName(name);
            if (context.Traveler.AmNotWithinTalkingDistanceToPersonByName(name))
                return;

            context.Dialog.TalkToPersonByName(context, name);
        }

        public bool HasNpcSaidSomethingNew(IGameContext context, EliteAPI.ChatEntry lastChatEntry, int secondsToWait)
        {
            DateTime duration = DateTime.Now.AddSeconds(secondsToWait);
            while (DateTime.Now < duration)
            {
                if (lastChatEntry != LastThingAnNpcSaid(context))
                    return true;
            }

            return false;
        }

        public NpcTalkStyle IsInADialogOrConversationWithNpc(IGameContext context,
            EliteAPI.ChatEntry lastChatEntryFromNpc, int secondsToCheckFor = 5)
        {
            DateTime duration = DateTime.Now.AddSeconds(5);
            while (DateTime.Now < duration)
            {
                // if a new chat entry added
                if (context.Dialog.HasNpcSaidSomethingNew(context, lastChatEntryFromNpc, 1))
                {
                    return NpcTalkStyle.Statement;
                }

                if (context.Dialog.AmInDialog(context, 1))
                {
                    return NpcTalkStyle.Question;
                }
            }

            return NpcTalkStyle.NotTalkingToAnNpc;
        }
    }

    public enum NpcTalkStyle
    {
        Statement,
        Question,
        NotTalkingToAnNpc
    }
}