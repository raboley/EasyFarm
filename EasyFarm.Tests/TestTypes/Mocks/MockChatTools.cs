using System.Collections.Generic;
using EliteMMO.API;
using MemoryAPI.Chat;

namespace EasyFarm.Tests.TestTypes.Mocks
{
    public class MockChatTools : IChatTools
    {
        public Queue<EliteAPI.ChatEntry> ChatEntries { get; set; } = new Queue<EliteAPI.ChatEntry>();
    }
}