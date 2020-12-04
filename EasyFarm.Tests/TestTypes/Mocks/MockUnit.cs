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

using MemoryAPI;
using MemoryAPI.Navigation;

namespace EasyFarm.Tests.TestTypes.Mocks
{
    public class MockUnit : IUnit
    {
        public int Id { get; set; }
        public int ClaimedId { get; set; }
        public double Distance { get; set; }
        public Position Position { get; set; }
        public short HppCurrent { get; set; }
        public bool IsActive { get; set; }
        public bool IsClaimed { get; set; }
        public bool IsRendered { get; set; }
        public string Name { get; set; }
        public NpcType NpcType { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public Status Status { get; set; }
        public bool MyClaim { get; set; }
        public bool HasAggroed { get; set; }
        public bool IsDead { get; set; }
        public bool PartyClaim { get; set; }
        public double YDifference { get; set; }
        public bool IsPet { get; set; }
        public bool IsValid { get; set; }
    }
}