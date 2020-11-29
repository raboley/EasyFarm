﻿// ///////////////////////////////////////////////////////////////////
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
using EasyFarm.Classes;

namespace EasyFarm.Tests.TestTypes.Mocks
{
    public class MockUnitService : IUnitService
    {
        public bool HasAggro { get; }
        public ICollection<IUnit> MobArray { get; } = new List<IUnit>();
        public ICollection<IUnit> NpcUnits { get; }
        public ICollection<IUnit> InanimateObjectsUnits { get; }

        public IUnit GetClosestUnitByPartialName(string name)
        {
            throw new System.NotImplementedException();
        }

        public IUnit GetUnitByName(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}