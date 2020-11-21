using EasyFarm.Context;
using EasyFarm.ViewModels;
using MemoryAPI;
using System.Collections.Generic;

namespace EasyFarm.Classes
{
    class Dialog : IDialog
    {
        private IMemoryAPI _memory;

        public Dialog(IMemoryAPI memory)
        {
            _memory = memory;
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
                        LogViewModel.Write($"Error occurred when selecting dialog options. Couldn't find dialog option '{desiredOptionText}' for question: '{dialog.Question}'");
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
    }
}
