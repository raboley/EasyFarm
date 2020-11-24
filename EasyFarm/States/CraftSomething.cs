using EasyFarm.Context;

namespace EasyFarm.States
{
    public class CraftSomething : BaseState
    {
        public override bool Check(IGameContext context)
        {
            return false;
        }

        public override void Run(IGameContext context)
        {
            context.Craft.AttemptToCraft(CraftingRecipe.StoneSoup());
        }
    }
}