using EasyFarm.Context;

namespace EasyFarm.States
{
    public class CraftSomething : BaseState
    {
        public override bool Check(IGameContext context)
        {
            if (context.Craft.HaveAllMaterialsToCraft(CraftingRecipe.StoneSoup()))
                return true;
            
            return false;
        }

        public override void Run(IGameContext context)
        {
            context.Craft.AttemptToCraft(CraftingRecipe.StoneSoup());
        }
    }
}