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
using System.Collections.Generic;
using MemoryAPI;
using MemoryAPI.Chat;

namespace EasyFarm.Tests.TestTypes.Mocks
{
    public class MockEliteAPI
    {
        public MockEliteAPI()
        {
            Player = new MockPlayerTools();
            NPC = new MockNPCTools();
            Windower = new MockWindowerTools(this);
            Navigator = new MockNavigatorTools();
            Timer = new MockTimerTools();
            Target = new MockTargetTools();
            PartyMember = new Dictionary<byte, MockPartyMemberTools>()
            {
                {0, new MockPartyMemberTools()}
            };
            Chat = new MockChatTools();
        }

        public MockNavigatorTools Navigator { get; set; }
        public MockNPCTools NPC { get; set; }
        public Dictionary<byte, MockPartyMemberTools> PartyMember { get; set; }
        public MockPlayerTools Player { get; set; }
        public MockTargetTools Target { get; set; }
        public MockTimerTools Timer { get; set; }
        public MockWindowerTools Windower { get; set; }
        public IChatTools Chat { get; set; }

        public static MockEliteAPI Create()
        {
            return new MockEliteAPI();
        }

        public IMemoryAPI AsMemoryApi()
        {
            return new MockEliteAPIAdapter(this);
        }
    }
}
