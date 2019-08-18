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
using System;
using System.Collections.Generic;
using EasyFarm.Classes;
using EasyFarm.Infrastructure;
using EasyFarm.Tests.TestTypes.Mocks;
using EasyFarm.UserSettings;
using MemoryAPI;
using static EliteMMO.API.EliteAPI;

namespace EasyFarm.Tests.TestTypes
{
    public class AbstractTestBase
    {
        protected readonly MockGameAPI MockGameAPI;

        protected HashSet<Type> Events { get; set; } = new HashSet<Type>();

        protected AbstractTestBase()
        {
            MockGameAPI = new MockGameAPI();
            StartRecordingEvents();
        }

        private void StartRecordingEvents()
        {
            AppServices.RegisterEvent<Events.PauseEvent>(this, x =>
            {
                Events.Add(typeof(Events.PauseEvent));
            });
        }

        public static BattleAbility FindAbility()
        {
            var battleAbility = new BattleAbility();
            battleAbility.IsEnabled = true;
            battleAbility.Name = "valid";
            return battleAbility;
        }

        protected static IUnit FindUnit()
        {
            var unit = new MockUnit
            {
                Name = "Mandragora",
                ClaimedId = 0,
                Distance = 3.0,
                HasAggroed = false,
                HppCurrent = 100,
                Id = 200,
                IsActive = true,
                IsClaimed = false,
                IsDead = false,
                IsPet = false,
                IsRendered = true,
                MyClaim = false,
                NpcType = NpcType.Mob,
                PartyClaim = false,
                Status = Status.Standing,
                YDifference = 2.0,
                IsValid = true
            };

            return unit;
        }

        //protected static IItem GetItem()
        //{
        //    var item = new MockItem
        //    {

        //    }
        //}
    }
}