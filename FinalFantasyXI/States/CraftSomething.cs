using System.Collections.Generic;

namespace FinalFantasyXI.States
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
            
            var knownRecipes = GetKnownRecipes(context);

            foreach (var recipe in knownRecipes)
            {
                if (context.Craft.CanAndShouldCraft(recipe))
                    return true;
            }

            return false;
        }



        public override void Run(IGameContext context)
        {
            var knownRecipes = GetKnownRecipes(context);

            foreach (var recipe in knownRecipes)
            {
                if (context.Craft.CanAndShouldCraft(recipe))
                {
                    context.Craft.AttemptToCraft(recipe);
                    break;
                }
            }
        }
        
        private static List<CraftingRecipe> GetKnownRecipes(IGameContext context)
        {
            var knownRecipes = new List<CraftingRecipe>();
            knownRecipes.Add(CraftingRecipe.WorkBench());
            knownRecipes.Add(CraftingRecipe.StoneSoup());
            knownRecipes.Add(CraftingRecipe.ArrowWoodLumber(context));
            knownRecipes.Add(CraftingRecipe.AshLumber(context));
            knownRecipes.Add(CraftingRecipe.BronzeIngotFromGoblinHelmet());
            knownRecipes.Add(CraftingRecipe.BronzeIngotFromGoblinMail());
            knownRecipes.Add(CraftingRecipe.MapleLumber());
            knownRecipes.Add(CraftingRecipe.Hatchet(context));
            knownRecipes.Add(CraftingRecipe.BronzeIngotFromBeastCoin());
            knownRecipes.Add(CraftingRecipe.BronzeSheet());
            knownRecipes.Add(CraftingRecipe.RabbitMantle());
            knownRecipes.Add(CraftingRecipe.MapleWand());
            knownRecipes.Add(CraftingRecipe.SaltedHare());
            return knownRecipes;
        }
    }
}