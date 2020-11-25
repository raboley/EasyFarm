using System.Collections.Generic;
using EasyFarm.Context;

namespace EasyFarm.States
{
    public class CraftSomething : BaseState
    {
        public override bool Check(IGameContext context)
        {
            if (new RestState().Check(context)) return false;

            if (new NeedSignet().Check(context)) return false;
            
            if (context.Inventory.InventoryIsFull())
                return false;
            
            if (context.Player.IsDead) return false;

            if (context.Player.HasAggro) return false;
            
            if (context.Target.IsValid) return false;
            
            var knownRecipes = GetKnownRecipes();

            foreach (var recipe in knownRecipes)
            {
                if (context.Craft.HaveAllMaterialsToCraft(recipe))
                    return true;
            }

            return false;
        }



        public override void Run(IGameContext context)
        {
            var knownRecipes = GetKnownRecipes();

            foreach (var recipe in knownRecipes)
            {
                if (context.Craft.HaveAllMaterialsToCraft(recipe))
                    context.Craft.AttemptToCraft(recipe);
            }
        }
        
        private static List<CraftingRecipe> GetKnownRecipes()
        {
            var knownRecipes = new List<CraftingRecipe>();
            knownRecipes.Add(CraftingRecipe.StoneSoup());
            knownRecipes.Add(CraftingRecipe.ArrowWoodLumber());
            knownRecipes.Add(CraftingRecipe.BronzeIngotFromGoblinHelmet());
            knownRecipes.Add(CraftingRecipe.BronzeIngotFromGoblinMail());
            knownRecipes.Add(CraftingRecipe.MapleLumber());
            knownRecipes.Add(CraftingRecipe.Hatchet());
            return knownRecipes;
        }
    }
}