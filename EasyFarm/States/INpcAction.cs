using EasyFarm.Context;

namespace EasyFarm.States
{
    public interface INpcAction
    {
        void PerformAction(IGameContext context);
    }
}