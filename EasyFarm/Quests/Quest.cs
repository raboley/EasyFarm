using EasyFarm.Classes;
using EasyFarm.Context;

namespace EasyFarm.Quests
{
    public class Quest
    {
        private IInventory _inventory;

        public Quest(IGameContext context)
        {
            _inventory = context.Inventory;
        }

        public bool PreReqsMet()
        {
            return false;
        }

        public void DoQuest()
        {
            
        }
    }
}