using System.Threading;
using EliteMMO.API;
using FinalFantasyXI.Context;
using MemoryAPI;

namespace FinalFantasyXI.Classes
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

        public void Up()
        {
            _api.Windower.SendKeyPress(Keys.UP);
            Thread.Sleep(100);
        }

        public void Down()
        {
            _api.Windower.SendKeyPress(Keys.DOWN);
            Thread.Sleep(100);
        }

        public void Left()
        {
            _api.Windower.SendKeyPress(Keys.LEFT);
            Thread.Sleep(100);
        }

        public void Right()
        {
            _api.Windower.SendKeyPress(Keys.RIGHT);
            Thread.Sleep(100);
        }

        public void Enter()
        {
            _api.Windower.SendKeyPress(Keys.NUMPADENTER);
            Thread.Sleep(100);
        }
    }
}
