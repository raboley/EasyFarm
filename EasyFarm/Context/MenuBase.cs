using System.Threading;
using EliteMMO.API;
using MemoryAPI;

namespace EasyFarm.Context
{
    public class MenuBase
    {
        private protected IMemoryAPI _context;

        public MenuBase(IMemoryAPI context)
        {
            _context = context;
        }
        protected void ClickCraft()
        {
            NavigateToTopRightCraftButton();
            Enter();
            Thread.Sleep(1000);
        }
        
        private void NavigateToTopRightCraftButton()
        {
            for (int i = 0; i < 5; i++)
            {
                Right();
            }
            Up();
        }
        
        protected void Select()
        {
            _context.Windower.SendKeyPress(Keys.NUMPADENTER);
            Thread.Sleep(100);
        }
        




        protected void Up()
        {
            _context.Windower.SendKeyPress(Keys.UP);
            Thread.Sleep(100);
        }
        protected void Down()
        {
            _context.Windower.SendKeyPress(Keys.DOWN);
            Thread.Sleep(100);
        }
        protected void Left()
        {
            _context.Windower.SendKeyPress(Keys.LEFT);
            Thread.Sleep(100);
        }
        protected void Right()
        {
            _context.Windower.SendKeyPress(Keys.RIGHT);
            Thread.Sleep(100);
        }
        protected void Enter()
        {
            _context.Windower.SendKeyPress(Keys.NUMPADENTER);
            Thread.Sleep(100);
        }

        protected void CloseMenu()
        {
            if (_context.Craft.IsCraftMenuOpen)
            {
                _context.Windower.SendKeyPress(Keys.ESCAPE);
                Thread.Sleep(100);
                Enter();
            }
                
            
            while (_context.Menu.IsMenuOpen)
            {
                _context.Windower.SendKeyPress(Keys.ESCAPE);
                Thread.Sleep(100);
            }

            Thread.Sleep(100);
        }



        protected void ChooseCrystalSynthesis()
        {
            while (_context.Menu.MenuItemCount > 2)
            {
                Thread.Sleep(10);
            }

            while (_context.Menu.MenuIndex != 1)
            {
                Down();
            }

            Thread.Sleep(100);

            Select();
        }



        protected void OpenSynthesisMenu()
        {
            RefreshMenuInCaseSomewhereWeird();

            const int synthesisMenuIndex = 10;
            const string synthesisMenuHelpName = "Synthesis";

            while (_context.Menu.HelpName != synthesisMenuHelpName)
            {
                var startIndex = _context.Menu.MenuIndex;
                Down();
                while (startIndex != _context.Menu.MenuIndex)
                {
                    if (_context.Menu.HelpName == synthesisMenuHelpName)
                        break;
                    
                    Down();
                }
                
                if (_context.Menu.HelpName == synthesisMenuHelpName)
                    break;

                _context.Windower.SendKeyPress(Keys.RIGHT);
                Thread.Sleep(100);
            }

            Select();
        }



        private void RefreshMenuInCaseSomewhereWeird()
        {
            if (_context.Menu.IsMenuOpen)
                _context.Windower.SendKeyPress(Keys.MINUS);

            _context.Windower.SendKeyPress(Keys.MINUS);
        }

        protected void ResetMenu()
        {
            _context.Windower.SendKeyUp(Keys.UP);
            _context.Windower.SendKeyUp(Keys.DOWN);
            _context.Windower.SendKeyUp(Keys.RIGHT);
            _context.Windower.SendKeyUp(Keys.ESCAPE);
        }
    }
}