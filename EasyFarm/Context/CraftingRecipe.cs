using System;
using System.Collections.Generic;
using Xceed.Wpf.DataGrid.Converters;

namespace EasyFarm.Context
{
    public class CraftingRecipe
    {
        public static CraftingRecipe BronzeIngotFromGoblinMail()
        {
            var soup = new CraftingRecipe();
            soup.Crystal = "Lightng. Crystal";
            soup.RequiredItems = new List<Item>
            {
                new Item() {Name = "Goblin Mail", Count = 1},
            };

            return soup;
        }
        
        public static CraftingRecipe BronzeIngotFromGoblinHelmet()
        {
            var soup = new CraftingRecipe();
            soup.Crystal = "Lightng. Crystal";
            soup.RequiredItems = new List<Item>
            {
                new Item() {Name = "Goblin Helm", Count = 1},
            };

            return soup;
        }
        
        public static CraftingRecipe StoneSoup()
        {
            var soup = new CraftingRecipe();
            soup.Crystal = "Fire Crystal";
            soup.RequiredItems = new List<Item>
            {
                new Item() {Name = "Flint Stone", Count = 3},
                new Item() {Name = "Distilled Water", Count = 1}
            };

            return soup;
        }
        
        public static CraftingRecipe SaltedHare()
        {
            var soup = new CraftingRecipe();
            soup.Crystal = "Fire Crystal";
            soup.RequiredItems = new List<Item>
            {
                new Item() {Name = "Hare Meat", Count = 1},
                new Item() {Name = "Rock Salt", Count = 1}
            };

            return soup;
        }
        
        public static CraftingRecipe BronzeIngotFromBeastCoin()
        {
            var soup = new CraftingRecipe();
            soup.Crystal = "Fire Crystal";
            soup.RequiredItems = new List<Item>
            {
                new Item() {Name = "Beastcoin", Count = 4}
            };

            return soup;
        }
        public static CraftingRecipe BronzeSheet()
        {
            var soup = new CraftingRecipe();
            soup.Crystal = "Fire Crystal";
            soup.RequiredItems = new List<Item>
            {
                new Item() {Name = "Bronze Ingot", Count = 1}
            };

            return soup;
        }
        
        public static CraftingRecipe Hatchet(IGameContext context)
        {
            var soup = new CraftingRecipe();
            soup.ShouldCraft = () => context.Inventory.GetCountOfItemsInContainer("Hatchet") < 12;
            
            soup.Crystal = "Fire Crystal";
            soup.RequiredItems = new List<Item>
            {
                new Item() {Name = "Bronze Ingot", Count = 2},
                new Item() {Name = "Maple Lumber", Count = 1}
            };

            return soup;
        }

        public static CraftingRecipe MapleWand()
        {
            var soup = new CraftingRecipe();
            
            soup.Crystal = "Wind Crystal";
            soup.RequiredItems = new List<Item>
            {
                new Item() {Name = "Chocobo Fthr.", Count = 1},
                new Item() {Name = "Maple Lumber", Count = 1}
            };

            return soup;
        }
        
        public static CraftingRecipe ArrowWoodLumber(IGameContext context)
        {
            var soup = new CraftingRecipe();
            soup.Crystal = "Wind Crystal";
            soup.ShouldCraft = () => context.API.Player.CraftSkills.Woodworking.Skill <= 2;
            
            soup.RequiredItems = new List<Item>
            {
                new Item() {Name = "Arrowwood Log", Count = 1},
            };

            return soup;
        }
        
        public static CraftingRecipe MapleLumber()
        {
            var soup = new CraftingRecipe();
            soup.Crystal = "Wind Crystal";
            soup.RequiredItems = new List<Item>
            {
                new Item() {Name = "Maple Log", Count = 1},
            };

            return soup;
        }
        
        public static CraftingRecipe AshLumber(IGameContext context)
        {
            var soup = new CraftingRecipe();
            soup.Crystal = "Wind Crystal";
            soup.ShouldCraft = () => context.API.Player.CraftSkills.Woodworking.Skill >= 4;
            
            soup.RequiredItems = new List<Item>
            {
                new Item() {Name = "Ash Log", Count = 1},
            };

            return soup;
        }

        public static CraftingRecipe RabbitMantle()
        {
            var soup = new CraftingRecipe();
            soup.Crystal = "Earth Crystal";
            soup.RequiredItems = new List<Item>
            {
                new Item() {Name = "Rabbit Hide", Count = 5},
                new Item() {Name = "Grass Thread", Count = 1},
            };

            return soup;
        }
        public string Crystal { get; set; }
        public List<Item> RequiredItems { get; set; }

        public Func<bool> ShouldCraft { get; set; }

        public CraftingRecipe(Func<bool> shouldCraft = null)
        {
            if (shouldCraft == null)
                shouldCraft = () => true;
            
            ShouldCraft = shouldCraft;
        }
    }
}