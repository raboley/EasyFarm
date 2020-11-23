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
using EasyFarm.Context;
using EasyFarm.Parsing;
using EasyFarm.States;
using EasyFarm.Tests.Context;
using MemoryAPI;
using Xunit;

namespace EasyFarm.Tests.States
{
    public class ZoneStateTests
    {
        private readonly Zone startingZoneName = Zone.Konschtat_Highlands;
        private readonly Zone newZoneName = Zone.Valkurm_Dunes;
        private readonly Pathfinder.Map.Zone StartingZone = new Pathfinder.Map.Zone(MemoryAPI.Zone.Konschtat_Highlands.ToString());
        private readonly Pathfinder.Map.Zone NewZone = new Pathfinder.Map.Zone(Zone.Valkurm_Dunes.ToString());

        private readonly ZoneState _zoneState = new ZoneState();
        private readonly TestContext context = new TestContext();

        [Fact]
        public void CheckIsTrueWhenZoneChanges()
        {
            // Fixture setup
            context.Player.Zone = startingZoneName;
            context.Zone = NewZone;
            context.Player.Str = 100;
            // Exercise system
            bool result = _zoneState.Check(context);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void CheckIsTrueWhenPlayersStatsAreZero()
        {
            // Fixture setup
            context.Player.Str = 0;
            // Exercise system
            bool result = _zoneState.Check(context);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void CheckIsFalseWhenNotZoning()
        {
            // Fixture setup
            context.Player.Zone = startingZoneName;
            context.Player.Str = 100;
            context.Zone = StartingZone;
            // Exercise system
            bool result = _zoneState.Check(context);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void RunOnZoningSetsZoneToNewZone()
        {
            // Fixture setup
            context.Player.Zone = newZoneName;
            context.Player.Str = 100;
            // Exercise system
            _zoneState.Run(context);
            // Verify outcome
            Assert.Equal(NewZone, context.Zone);
            // Teardown
        }

        [Fact]
        public void RunOnZoningStopsPlayerFromRunning()
        {
            // Fixture setup
            _zoneState.ZoningAction = ForceMoveToNextZone(context);
            context.Player.Zone = newZoneName;
            // Exercise system
            _zoneState.Run(context);
            // Verify outcome
            Assert.False(context.MockAPI.Navigator.IsRunning);
            // Teardown
        }

        [Fact]
        public void RunWhileZoningWaits()
        {
            // Fixture setup
            context.Zone = StartingZone;
            context.Player.Str = 0;
            context.Player.Zone = newZoneName;
            _zoneState.ZoningAction = ForceMoveToNextZone(context);
            // Exercise system
            _zoneState.Run(context);
            // Verify outcome
            Assert.Equal(100, context.Player.Str);
            // Teardown
        }

        private Action ForceMoveToNextZone(IGameContext context)
        {
            return () => context.Player.Str = 100;
        }
    }
}
