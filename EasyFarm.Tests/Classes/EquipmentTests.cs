// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013 Mykezero
//  
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//  
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////
using EasyFarm.Classes;
using EasyFarm.Tests.Context;
using EasyFarm.Tests.TestTypes;
using MemoryAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EasyFarm.Tests.Classes
{
    public class EquipmentTests : AbstractTestBase
    {
        private readonly TestContext _context = new TestContext();

        [Fact]
        public void NewEmptyEquipmentCanBeCreatedWithDefaultOptimalLevelOf1()
        {
            var equipment = new EquipmentSet();
            Assert.Equal("1", equipment.Level.ToString());
        }

        [Fact]
        public void NewEmptyEquipmentCanBeCreatedWithAllEmptySlots()
        {
            var equipment = new EquipmentSet();
            Assert.Equal("Empty", equipment.Main.Name);
            Assert.Equal("Empty", equipment.Sub.Name);
            Assert.Equal("Empty", equipment.Range.Name);
            Assert.Equal("Empty", equipment.Ammo.Name);
            Assert.Equal("Empty", equipment.Head.Name);
            Assert.Equal("Empty", equipment.Neck.Name);
            Assert.Equal("Empty", equipment.Body.Name);
            Assert.Equal("Empty", equipment.Hands.Name);
            Assert.Equal("Empty", equipment.Waist.Name);
            Assert.Equal("Empty", equipment.Back.Name);
            Assert.Equal("Empty", equipment.Legs.Name);
            Assert.Equal("Empty", equipment.Feet.Name);
            Assert.Equal("Empty", equipment.Ring1.Name);
            Assert.Equal("Empty", equipment.Ring2.Name);
            Assert.Equal("Empty", equipment.Ear1.Name);
            Assert.Equal("Empty", equipment.Ear2.Name);
        }

        [Fact]
        public void EquipmentCanEquipAnEquipableItemAndPutItInTheCorrectSlot()
        {
            // given
            var equipment = new EquipmentSet();
            EquipableItem dagger = new EquipableItem("Gandring", EquipSlots.Main);

            // when
            equipment.Equip(dagger);

            // then
            Assert.Equal("Gandring", equipment.Main.Name);
        }


        //[Fact]
        //public void DetermineOptimalGearForJobAndLevelReturnsCorrectDictionaryWithValidLevelAndJob()
        //{

            // GivenItems
            // WhenItemEquiped
            // ThenItemIsEquiped

        //    // Fixture setup
        //    _context.Player.JobLevel = 33;
        //    _context.Player.Job = Job.WhiteMage;

        // GivenCha

        //    Dictionary<string, string> expected = new Dictionary<string, string> {
        //            {"Main", "Spiked Club +1" },
        //            {"Sub", "Lizard Strap +1" },
        //            {"Ammo", "Morion Tathlum" },
        //            {"Head", "Trump Crown" },
        //            {"Neck", "Holy Phial" },
        //            {"Body", "Seer's Tunic +1" },
        //            {"Hands", "Savage gauntlets" },
        //            {"Legs", "Seer's Slacks +1" },
        //            {"Feet", "Seer's Pumps +1" },
        //            {"Back", "Talisman Cape" },
        //            {"Waist", "Mohbwa Sash +1" },
        //            {"Ear1", "Reraise Earring" },
        //            {"Ear2", "Magician's Earring" },
        //            {"Ring1", "Saintly Ring +1" },
        //            {"Ring2", "Saintly Ring +1" },
        //          };

        //    // Excercise system
        //    var result = LevelChangeState.DetermineOptimalGearForMainJobAndLevel(_context);

        //    // Verify outcome
        //    Assert.Equal(expected.Count, result.Count);
        //    foreach (KeyValuePair<string, string> equip in expected)
        //    {
        //        Assert.Equal(expected[equip.Key], result[equip.Key]);
        //    }
        //}
    }
}
