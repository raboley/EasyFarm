using System.Collections.Generic;
using EliteMMO.API;
using MemoryAPI;
using MemoryAPI.Windower;

namespace EasyFarm.Tests.TestTypes.Mocks
{
    public class MockWindowerTools : IWindowerTools
    {
        private readonly MockEliteAPI _eliteAPI;

        public string LastCommand { get; set; }
        public IList<Keys> KeyPresses { get; set; } = new List<Keys>();
        public Keys LastKeyPress { get; set; }

        public MockWindowerTools(MockEliteAPI eliteAPI)
        {
            _eliteAPI = eliteAPI;
        }

        public void SendString(string stringToSend)
        {
            LastCommand = stringToSend;

            if (stringToSend == Constants.AttackTarget)
            {
                _eliteAPI.Player.Status = Status.Fighting;
            }

            if (stringToSend == Constants.RestingOn)
            {
                _eliteAPI.Player.Status = Status.Healing;
            }

            if (stringToSend == Constants.RestingOff)
            {
                _eliteAPI.Player.Status = Status.Standing;
            }
        }

        public void SendKeyPress(Keys key)
        {
            LastKeyPress = key;
            KeyPresses.Add(key);
        }

        public void SendKeyDown(Keys key)
        {
            throw new System.NotImplementedException();
        }

        public void SendKeyUp(Keys key)
        {
            throw new System.NotImplementedException();
        }
    }
}