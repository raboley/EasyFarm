using EasyFarm.Tests.TestTypes.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EasyFarm.Tests.Classes
{
    class DialogTests
    {

        private readonly MockGameAPI _mockApi = new MockGameAPI();

        [Fact]
        public void DisengageWhenFightingWillStopAttacking()
        {
            //// Fixture setup            
            //_mockApi.Dialog.Text = "Select a type of equipment.\a\vReturn to previous selection.\aItem level 119 weapons/shields.\aItem level 119 headgear.\aItem level 119 chestgear.\aItem level 119 handgear.\aItem level 119 leggear.\aItem level 119 footgear.\aLevel 99 equipment.\aLevel 98 and lower equipment.\aAccessories.\u007f1";
            //// Excercise system
            //Player.Disengage(_mockApi);
            //// Verify outcome
            //Assert.Equal(Constants.AttackOff, _mockApi.Mock.Windower.LastCommand);
            //// Teardown	
        }

        //"Select a type of equipment.\a\vReturn to previous selection.\aItem level 119 weapons/shields.\aItem level 119 headgear.\aItem level 119 chestgear.\aItem level 119 handgear.\aItem level 119 leggear.\aItem level 119 footgear.\aLevel 99 equipment.\aLevel 98 and lower equipment.\aAccessories.\u007f1"
    }
}
