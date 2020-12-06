using EasyFarm.Context;
using EasyFarm.States;
using MemoryAPI;
using MemoryAPI.Navigation;

namespace EasyFarm.Classes
{
    public interface INavigator
    {
        double DistanceTo(IMemoryAPI fface, Position position);
        void HomePointWarpAddon(IGameContext context, IMemoryAPI fface, string homePoint);
        void TravelToNpcAndTalk(IGameContext context, IUnit unit);
        bool IsStuck();
        void LoadRoute(string routePath);
        void TravelPath(IGameContext context, string routePath);
        void WaitForZone(IMemoryAPI fface, IGameContext context);
        void WarpHome(IMemoryAPI fface, IGameContext context);
        // void GoToNpc(IGameContext context, IMemoryAPI fface, string npcName);
        void InteractWithoutMoving(IGameContext context, IMemoryAPI fface, IUnit unit);
        void OpenDoor(IGameContext context, IMemoryAPI fface);
        void TravelToNpcAndPerformAction(IGameContext context, IUnit unit, INpcAction action);
        bool TryTravelToNpc(IGameContext context, IUnit unit);
    }
}