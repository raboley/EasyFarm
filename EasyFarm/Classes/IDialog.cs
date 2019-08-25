using System.Collections.Generic;
using EasyFarm.Context;

namespace EasyFarm.Classes
{
    public interface IDialog
    {
        void ChooseDialogOptions(IGameContext context, List<string> desiredOptionTexts);
        void MoveToDesiredDialogOption(IGameContext context, int desiredIndex);
        void SelectDialogOption(IGameContext context, string desiredOptionText);
        void ExitDialog(IGameContext context);
    }
}