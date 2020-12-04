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

namespace FinalFantasyXI.Classes
{
    public interface IUnit
    {
        int Id { get; set; }
        int ClaimedId { get; }
        double Distance { get; }
        Position Position { get; }
        short HppCurrent { get; }
        bool IsActive { get; }
        bool IsClaimed { get; }
        bool IsRendered { get; }
        string Name { get; }
        NpcType NpcType { get; }
        float PosX { get; }
        float PosY { get; }
        float PosZ { get; }
        Status Status { get; }    
        bool MyClaim { get; }
        bool HasAggroed { get; }
        bool IsDead { get; }
        bool PartyClaim { get; }
        double YDifference { get; }
        bool IsPet { get; }
        bool IsValid { get; set; }
    }
}