using FinalFantasyXI.Context;
using MemoryAPI;
using MemoryAPI.Navigation;

namespace FinalFantasyXI.Classes
{
    public interface INavigator
    {
        double DistanceTo(IMemoryAPI fface, Position position);
        void HomePointWarpAddon(IGameContext context, IMemoryAPI fface, string homePoint);
        void InteractWithUnit(IGameContext context, IMemoryAPI fface, IUnit unit);
        bool IsStuck();
        void LoadRoute(string routePath);
        void TravelPath(IGameContext context, string routePath);
        void WaitForZone(IMemoryAPI fface, IGameContext context);
        void WarpHome(IMemoryAPI fface, IGameContext context);
        void GoToNpc(IGameContext context, IMemoryAPI fface, string npcName);
    }
}