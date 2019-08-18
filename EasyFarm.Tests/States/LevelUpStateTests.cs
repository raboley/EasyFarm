using EasyFarm.Classes;
using EasyFarm.States;
using EasyFarm.Tests.Context;
using EasyFarm.Tests.TestTypes;
using EasyFarm.Tests.TestTypes.Mocks;
using MemoryAPI;
using System;
using System.Collections.Generic;
using Xunit;
using static EasyFarm.States.SetSearchNpcState;

namespace EasyFarm.Tests.States
{
    public class LevelUpStateTests
    {
        private readonly WeaponskillState _sut = new WeaponskillState();
        private readonly TestContext _context = new TestContext();
        private readonly MockGameAPI _mockApi = new MockGameAPI();
        private readonly LevelChangeState _levelChangeState = new LevelChangeState();

        [Fact]
        public void EnumParseCanFigureOutSlotIdOfCurrentlyEquipedItems_Weapon()
        {
            // A.
            // Possible Optimal Equipment Dictionary slot:
            string slot = "Main";

            // B.
            // Try to convert the string to an enum:
            EquipSlots slotEnum = (EquipSlots)Enum.Parse(typeof(EquipSlots), slot);

            // C.
            // See if the conversion succeeded:
            Assert.Equal(EquipSlots.Main, slotEnum);
        }

        [Fact]
        public void EnumParseCanFigureOutSlotIdOfCurrentlyEquipedItems_Legs()
        {
            // A.
            // Possible Optimal Equipment Dictionary slot:
            string slot = "Legs";

            // B.
            // Try to convert the string to an enum:
            EquipSlots slotEnum = (EquipSlots)Enum.Parse(typeof(EquipSlots), slot);

            // C.
            // See if the conversion succeeded:
            Assert.Equal(EquipSlots.Legs, slotEnum);
        }

        [Fact]
        public void CanGetIdFromSlotEnum()
        {
            int expectedId = 0;

            int actualId = (int)EquipSlots.Main;

            Assert.Equal(expectedId, actualId);
        } 

        [Fact]
        public void LevelChangeStateTriggersOnFirstRun()
        {
            // Fixture setup            
            // Excercise system
            _context.Player.JobLevel = 1;
            //_context.Memory.EliteApi.Player.JobLevel = 2;
            // Verify outcome
            bool result = _levelChangeState.Check(_context);
            Assert.True(result);
            // Teardown	
        }

        [Fact]
        public void LevelChangeStateDoesNotTriggerWhenLevelIsTheSame()
        {
            // Fixture setup            
            _context.Player.JobLevel = 1;
            _levelChangeState.Check(_context);
            // Excercise system
            _context.Player.JobLevel = 1;
            bool result = _levelChangeState.Check(_context);
            // Verify outcome
            Assert.False(result);
            // Teardown	
        }

        [Fact]
        public void LevelChangeStateDoesTriggerWhenLevelUpHappens()
        {
            // Fixture setup            
            _context.Player.JobLevel = 1;
            _levelChangeState.Check(_context);
            // Excercise system
            _context.Player.JobLevel = 2;
            bool result = _levelChangeState.Check(_context);
            // Verify outcome
            Assert.True(result);
            // Teardown	
        }

        [Fact]
        public void LevelChangeStateDoesTriggerWhenChangingJobs()
        {
            // Fixture setup
            _context.Player.JobLevel = 37;
            _levelChangeState.Check(_context);
            // Excercise system
            _context.Player.JobLevel = 5;
            bool result = _levelChangeState.Check(_context);
            // Verify outcome
            Assert.True(result);
            // Teardown	
        }

        [Fact]
        public void DetermineOptimalGearForJobAndLevelReturnsCorrectDictionaryWithValidLevelAndJob()
        {
            // Fixture setup
            _context.Player.JobLevel = 33;
            _context.Player.Job = Job.WhiteMage;

            Dictionary<string, string> expected =  new Dictionary<string, string> {
                    {"Main", "Spiked Club +1" },
                    {"Sub", "Lizard Strap +1" },
                    {"Ammo", "Morion Tathlum" },
                    {"Head", "Trump Crown" },
                    {"Neck", "Holy Phial" },
                    {"Body", "Seer's Tunic +1" },
                    {"Hands", "Savage gauntlets" },
                    {"Legs", "Seer's Slacks +1" },
                    {"Feet", "Seer's Pumps +1" },
                    {"Back", "Talisman Cape" },
                    {"Waist", "Mohbwa Sash +1" },
                    {"Ear1", "Reraise Earring" },
                    {"Ear2", "Magician's Earring" },
                    {"Ring1", "Saintly Ring +1" },
                    {"Ring2", "Saintly Ring +1" },
                  };

            // Excercise system
            var result = LevelChangeState.DetermineOptimalGearForMainJobAndLevel(_context);

            // Verify outcome
            Assert.Equal(expected.Count, result.Count);
            foreach (KeyValuePair<string, string> equip in expected)
            {
                Assert.Equal(expected[equip.Key], result[equip.Key]);
            }
        }

        [Fact]
        public void DetermineOptimalGearForJobAndLevelReturnsBlankDictionaryWithoutErrorIfNoMatchingEquipsForLevel()
        {
            // Fixture setup
            _context.Player.JobLevel = 1;
            _context.Player.Job = Job.WhiteMage;

            Dictionary<string, string> expected = new Dictionary<string, string>();

            // Excercise system
            var result = LevelChangeState.DetermineOptimalGearForMainJobAndLevel(_context);

            // Verify outcome
            Assert.Equal(expected.Count, result.Count);
            foreach (KeyValuePair<string, string> equip in expected)
            {
                Assert.Equal(expected[equip.Key], result[equip.Key]);
            }
        }

        [Fact]
        public void DetermineOptimalGearForJobAndLevelReturnsBlankDictionaryWithoutErrorIfNoMatchingEquipsForJob()
        {
            // Fixture setup
            _context.Player.JobLevel = 1;
            _context.Player.Job = Job.PuppetMaster;

            Dictionary<string, string> expected = new Dictionary<string, string>();

            // Excercise system
            var result = LevelChangeState.DetermineOptimalGearForMainJobAndLevel(_context);

            // Verify outcome
            Assert.Equal(expected.Count, result.Count);
            foreach (KeyValuePair<string, string> equip in expected)
            {
                Assert.Equal(expected[equip.Key], result[equip.Key]);
            }
        }


        [Fact]
        public void CanGetItemObjectFromEquipmentId()
        {
            string expectedName = "Bronze Knife +1";
            int id = 16491;

            var item = _context.Memory.EliteApi.Resource.GetItem(id);
            string actualName = item.Name.ToString();

            Assert.Equal(expectedName, actualName);
        }
    }
}
