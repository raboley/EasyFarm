using EliteMMO.API;

namespace MemoryAPI.Memory.EliteMMOWrapper
{
    public class MenuTools : IMenuTools
    {
        private readonly EliteAPI _api;

        public MenuTools(EliteAPI api)
        {
            _api = api;
        }

        public int HPPCurrent => (int)_api.Player.HPP;

        public bool IsMenuOpen => (bool)_api.Menu.IsMenuOpen;
        public int MenuItemCount => (int)_api.Menu.MenuItemCount;
        public int MenuIndex
        {
            get
            {
                return (int)_api.Menu.MenuIndex;
            }
            set
            {
                _api.Menu.MenuIndex = value;
            }
        }

        public string MenuName => (string)_api.Menu.MenuName;
        public string HelpName => (string)_api.Menu.HelpName;
        public string HelpDescription => (string)_api.Menu.HelpDescription;

    }
}