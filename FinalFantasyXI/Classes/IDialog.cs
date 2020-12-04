using System.Collections.Generic;
using EliteMMO.API;
using FinalFantasyXI.Context;

namespace FinalFantasyXI.Classes
{
    public interface IDialog
    {
        void ResumeToNextDialog(IGameContext context);
        EliteAPI.ChatEntry LastThingAnNpcSaid(IGameContext context);
        bool AmInDialog(IGameContext context, int secondsToWait = 3);
        void ChooseDialogOptions(IGameContext context, List<string> desiredOptionTexts);
        void SelectDialogOption(IGameContext context, string desiredOptionText);
        void MoveToDesiredDialogOption(IGameContext context, int desiredIndex);
        void ExitDialog(IGameContext context);
        void TalkToPersonByName(IGameContext context, string name);
        void RespondWith(IGameContext context, string response);
        void WalkAndTalkToPersonByName(IGameContext context, string name);
        bool HasNpcSaidSomethingNew(IGameContext context, EliteAPI.ChatEntry lastChatEntry, int secondsToWait);

        NpcTalkStyle IsInADialogOrConversationWithNpc(IGameContext context, EliteAPI.ChatEntry lastChatEntryFromNpc, int secondsToCheckFor = 5);
        void HaveConversationWithPerson(IGameContext context, string personName, List<string> responses);
        void ResumeUntilNothingElseSaid(IGameContext context);
    }
}