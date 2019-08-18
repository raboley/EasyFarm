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
    class Menu : IMenuTools
    {

        private readonly IMemoryAPI _api;

        public Menu(IMemoryAPI api)
        {
            _api = api;
        }

        public bool IsMenuOpen { get; }
        public int MenuItemCount { get; }
        public int MenuIndex { get; set; }
        public string MenuName { get; }
        public string HelpName { get; }
        public string HelpDescription { get; }

        public int GetMenuIndex() => _api.Menu.MenuIndex;
        public void SetIndex(int index) => _api.Menu.MenuIndex = index;

        public string GetMenuName() => _api.Menu.MenuName;

        
    }
}
