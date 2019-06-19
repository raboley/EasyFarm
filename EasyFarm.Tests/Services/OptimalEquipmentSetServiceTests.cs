using EasyFarm.Classes;
using EasyFarm.Tests.Context;
using EasyFarm.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace EasyFarm.Tests.Services
{
    public class OptimalEquipmentSetServiceTests
    {
        private readonly TestContext _context = new TestContext();
        private readonly OptimalEquipmentSetService _optimalEquipmentSetService = new OptimalEquipmentSetService();

        [Fact]
        public void ShouldReturnListOfAllEquipments()
        {
            //GivenJsonFileExistsAtPath();
            List<EquipmentSet> result = WhenGetAllFromStoreIsCalled(@"C:\Users\rboley\Source\Repos\EasyFarm\EasyFarm.Tests\Services\DancerOptimalEquipment.json");
            ThenAListOfAllEquipmentFromFileIsReturned(result);
        }

        private void GivenJsonFileExistsAtPath(string v)
        {

            //List<EquipmentSet> result = LoadJson(v);
            Console.WriteLine("HI");

            //var json = File.ReadAllText(v);
            //try
            //{
            //    var jObject = JObject.Parse(json);
            //    Console.WriteLine(jObject);
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }



        private List<EquipmentSet> WhenGetAllFromStoreIsCalled(string jsonFileFullPath)
        {
            List<EquipmentSet> result = _optimalEquipmentSetService.GetAllEquipmentSets(jsonFileFullPath);
            return result;
        }

        private void ThenAListOfAllEquipmentFromFileIsReturned(List<EquipmentSet> result)
        {
            List<EquipmentSet> expectedEquipmentSets = new List<EquipmentSet>();

            EquipmentSet levelOne = new EquipmentSet();
            levelOne.Main = new EquipableItem("Bronze Knife +1", MemoryAPI.EquipSlots.Main);
            levelOne.Head = new EquipableItem("Bronze Cap +1", MemoryAPI.EquipSlots.Head);
            levelOne.Body = new EquipableItem("I.R. Jack Coat", MemoryAPI.EquipSlots.Body);
            levelOne.Legs = new EquipableItem("Bronze Subligar +1", MemoryAPI.EquipSlots.Legs);
            levelOne.Hands = new EquipableItem("Bronze Mittens +1", MemoryAPI.EquipSlots.Hands);
            levelOne.Feet = new EquipableItem("Bronze Leggings +1", MemoryAPI.EquipSlots.Feet);
            levelOne.Waist = new EquipableItem("Blood Stone", MemoryAPI.EquipSlots.Waist);
            levelOne.Ear1 = new EquipableItem("Cassie Earring", MemoryAPI.EquipSlots.Ear1);

            EquipmentSet levelTwo = new EquipmentSet();
            levelTwo.Level = 2;
            EquipmentSet levelThree = new EquipmentSet();
            levelTwo.Level = 3;
            EquipmentSet levelFour = new EquipmentSet();
            levelTwo.Level = 4;
            levelFour.Back = new EquipableItem("Rabbit Mantle", MemoryAPI.EquipSlots.Back);

            expectedEquipmentSets.Add(levelOne);
            expectedEquipmentSets.Add(levelTwo);
            expectedEquipmentSets.Add(levelThree);
            expectedEquipmentSets.Add(levelFour);

            Assert.Equal(expectedEquipmentSets.Count, result.Count);

            int i = 0;
            while (i < expectedEquipmentSets.Count)
            {
                Assert.Equal(expectedEquipmentSets[i].Main.Name, result[i].Main.Name);
                Assert.Equal(expectedEquipmentSets[i].Sub.Name, result[i].Sub.Name);
                Assert.Equal(expectedEquipmentSets[i].Range.Name, result[i].Range.Name);
                Assert.Equal(expectedEquipmentSets[i].Ammo.Name, result[i].Ammo.Name);
                Assert.Equal(expectedEquipmentSets[i].Head.Name, result[i].Head.Name);
                Assert.Equal(expectedEquipmentSets[i].Neck.Name, result[i].Neck.Name);
                Assert.Equal(expectedEquipmentSets[i].Body.Name, result[i].Body.Name);
                Assert.Equal(expectedEquipmentSets[i].Hands.Name, result[i].Hands.Name);
                Assert.Equal(expectedEquipmentSets[i].Waist.Name, result[i].Waist.Name);
                Assert.Equal(expectedEquipmentSets[i].Back.Name, result[i].Back.Name);
                Assert.Equal(expectedEquipmentSets[i].Legs.Name, result[i].Legs.Name);
                Assert.Equal(expectedEquipmentSets[i].Feet.Name, result[i].Feet.Name);
                Assert.Equal(expectedEquipmentSets[i].Ring1.Name, result[i].Ring1.Name);
                Assert.Equal(expectedEquipmentSets[i].Ring2.Name, result[i].Ring2.Name);
                Assert.Equal(expectedEquipmentSets[i].Ear1.Name, result[i].Ear1.Name);
                Assert.Equal(expectedEquipmentSets[i].Ear2.Name, result[i].Ear2.Name);
                i++;
            }
        }


        //[Fact]
        //public void ShouldReturnEquipmentSetWhenGivenAJobAndLevel()
        //{
        //    GivenJobIsThiefAndLevelIsOne();

        //    EquipmentSet result = WhenGetOptimalEquipmentIsCalled();

        //    ThenTheEquipmentSetForThiefLevelOneIsReturned(result);
        //}
        private void GivenJobIsThiefAndLevelIsOne()
        {
            _context.Player.Job = MemoryAPI.Job.Thief;
            _context.Player.JobLevel = 1;
        }

        private EquipmentSet WhenGetOptimalEquipmentIsCalled()
        {

            return _optimalEquipmentSetService.GetOptimalEquipment(_context);
        }

        private void ThenTheEquipmentSetForThiefLevelOneIsReturned(EquipmentSet result)
        {
            var equipment = new EquipmentSet();
            equipment.Main = new EquipableItem("Bronze Knife +1", MemoryAPI.EquipSlots.Main);

            Assert.Equal(equipment.Main.Name, result.Main.Name);
            Assert.Equal(equipment.Sub.Name, result.Sub.Name);
            Assert.Equal(equipment.Range.Name, result.Range.Name);
            Assert.Equal(equipment.Ammo.Name, result.Ammo.Name);
            Assert.Equal(equipment.Head.Name, result.Head.Name);
            Assert.Equal(equipment.Neck.Name, result.Neck.Name);
            Assert.Equal(equipment.Body.Name, result.Body.Name);
            Assert.Equal(equipment.Hands.Name, result.Hands.Name);
            Assert.Equal(equipment.Waist.Name, result.Waist.Name);
            Assert.Equal(equipment.Back.Name, result.Back.Name);
            Assert.Equal(equipment.Legs.Name, result.Legs.Name);
            Assert.Equal(equipment.Feet.Name, result.Feet.Name);
            Assert.Equal(equipment.Ring1.Name, result.Ring1.Name);
            Assert.Equal(equipment.Ring2.Name, result.Ring2.Name);
            Assert.Equal(equipment.Ear1.Name, result.Ear1.Name);
            Assert.Equal(equipment.Ear2.Name, result.Ear2.Name);
        }
    }
}
