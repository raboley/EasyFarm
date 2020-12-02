using System.Collections.Generic;
using System.Linq;
using EasyFarm.Context;
using EasyFarm.Missions;

namespace EasyFarm.Soul
{
    public static class CallingFactory
    {
        public static Calling JuniorWoodWorker(IGameContext gameContext, List<IObjective> questList)
        {
            var calling = new Calling(gameContext);
            calling.Name = "JuniorWoodWorker";
            calling.Done = context => context.API.Player.CraftSkills.Woodworking.Skill >= 10;

            var objectives = new List<IObjective>();


            var itemFinder = new ItemFinder(questList);

            // ArrowWood
            var arrowWoodRecipe = CraftingRecipe.ArrowWoodLumber(gameContext);
            var twoArrowWoodLumber = new CraftingObjective(gameContext)
            {
                DoPreWork = context =>
                {
                    if (context.Inventory.HaveItemInInventoryContainer(arrowWoodRecipe.Crystal))
                        itemFinder.GoGetItem(context, arrowWoodRecipe.Crystal);

                    if (context.Inventory.GetCountOfItemsInContainer(arrowWoodRecipe.RequiredItems.First().Name) <
                        arrowWoodRecipe.RequiredItems.First().Count)
                        itemFinder.GoGetItem(context, arrowWoodRecipe.RequiredItems.First().Name);
                },
                CanDo = context => context.Craft.HaveAllMaterialsToCraft(CraftingRecipe.ArrowWoodLumber(context)),
                Do = context => context.Craft.AttemptToCraft(CraftingRecipe.ArrowWoodLumber(context)),
                Done = context => context.API.Player.CraftSkills.Woodworking.Skill >= 2
            };
            // Maple Lumber
            var mapleRecipe = CraftingRecipe.MapleLumber();
            var fiveMapleLumber = new CraftingObjective(gameContext)
            {
                DoPreWork = context =>
                {
                    if (!twoArrowWoodLumber.Done(context))
                        twoArrowWoodLumber.Do(context);
                    
                    if (context.Inventory.HaveItemInInventoryContainer(mapleRecipe.Crystal))
                        itemFinder.GoGetItem(context, mapleRecipe.Crystal);

                    if (context.Inventory.GetCountOfItemsInContainer(mapleRecipe.RequiredItems.First().Name) <
                        mapleRecipe.RequiredItems.First().Count)
                        itemFinder.GoGetItem(context, mapleRecipe.RequiredItems.First().Name);
                },
                CanDo = context => context.Craft.HaveAllMaterialsToCraft(CraftingRecipe.MapleLumber()),
                Do = context => context.Craft.AttemptToCraft(CraftingRecipe.MapleLumber()),
                Done = context => context.API.Player.CraftSkills.Woodworking.Skill >= 5
            };
            
            // Lauan Lumber => Workbench
            
            var lauanLumberRecipe = CraftingRecipe.LauanLumber();
            var preWorkLauanLumber = new CraftingObjective(gameContext)
            {
                DoPreWork = context =>
                {
                    if (context.Inventory.HaveItemInInventoryContainer(lauanLumberRecipe.Crystal))
                        itemFinder.GoGetItem(context, lauanLumberRecipe.Crystal);

                    if (context.Inventory.GetCountOfItemsInContainer(lauanLumberRecipe.RequiredItems.First().Name) <
                        lauanLumberRecipe.RequiredItems.First().Count)
                        itemFinder.GoGetItem(context, lauanLumberRecipe.RequiredItems.First().Name);
                },
                CanDo = context => context.Craft.HaveAllMaterialsToCraft(CraftingRecipe.WorkBench()),
                Do = context => context.Craft.AttemptToCraft(CraftingRecipe.WorkBench()),
                Done = context => context.API.Player.CraftSkills.Woodworking.Skill >= 5
            };
            
            var workBenchRecipe = CraftingRecipe.WorkBench();
            var sevenWorkBench = new CraftingObjective(gameContext)
            {
                DoPreWork = context =>
                {
                    if (!fiveMapleLumber.Done(context))
                        fiveMapleLumber.Do(context);
                    
                    // TODO: Refactor this to probably just be a method called getItemsForRecipe
                    if (context.Inventory.HaveItemInInventoryContainer(workBenchRecipe.Crystal))
                        itemFinder.GoGetItem(context, workBenchRecipe.Crystal);

                    
                    if (context.Inventory.GetCountOfItemsInContainer(workBenchRecipe.RequiredItems.First().Name) <
                        workBenchRecipe.RequiredItems.First().Count)
                        // TODO: Add all the crafting recipes to GoGetItems so it can craft things that are pre-reqs.
                        itemFinder.GoGetItem(context, workBenchRecipe.RequiredItems.First().Name);
                },
                CanDo = context => context.Craft.HaveAllMaterialsToCraft(CraftingRecipe.WorkBench()),
                Do = context => context.Craft.AttemptToCraft(CraftingRecipe.WorkBench()),
                Done = context => context.API.Player.CraftSkills.Woodworking.Skill >= 5
            };
            // Ash Lumber

            ////// Test Bed /////
            if (!sevenWorkBench.CanDo(gameContext))
                sevenWorkBench.DoPreWork(gameContext);

            /////////////////////
            calling.Objectives = objectives;
            return calling;
        }
    }

    public class ItemFinder
    {
        public List<IObjective> Quests { get; set; }

        public ItemFinder(List<IObjective> questList)
        {
            Quests = questList;
        }


        public void GoGetItem(IGameContext context, string item)
        {
            if (item.Contains("Log"))
            {
                Quests.First(x => x.Name == "ChopWoodInRon").Do(context);
                return;
            }
            
            if (item.Contains("Crystal"))
            {
                LevelUpToGetCrystals(context);
            }
        }

        private void LevelUpToGetCrystals(IGameContext context)
        {
            foreach (var objective in Quests)
            {
                if (objective.ShouldDo(context))
                {
                    objective.Do(context);
                    return;
                }
            }
        }
    }
}