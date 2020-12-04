using FinalFantasyXI.Context;

namespace FinalFantasyXI.Classes
{
    public interface IMenu
    {
        void ExitMenus(IGameContext context);
        void Up();
        void Down();
        void Left();
        void Right();
        void Enter();
    }
}