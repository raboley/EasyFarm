using EasyFarm.Context;
using EliteMMO.API;
using MemoryAPI;
using MemoryAPI.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.Classes
{
    class Menu : IMenu
    {

        private readonly IMemoryAPI _api;

        public Menu(IMemoryAPI api)
        {
            _api = api;
        }

        public void ExitMenus(IGameContext context)
        {
            while (_api.Menu.IsMenuOpen == true)
            {
                context.API.Windower.SendKeyPress(Keys.ESCAPE);
                TimeWaiter.Pause(100);
            }
            //for (int i = 0; i < 7; i++)
            //{
            //    TimeWaiter.Pause(500);
            //    context.API.Windower.SendKeyPress(Keys.ESCAPE);
            //}
        }
    }
}
