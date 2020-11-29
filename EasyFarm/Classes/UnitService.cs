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
using MemoryAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using EasyFarm.UserSettings;
using MemoryAPI.Memory;

namespace EasyFarm.Classes
{
    /// <summary>
    /// Retrieves the zone's unit data.
    /// </summary>
    public class UnitService : IUnitService
    {
        private static bool _isInitialized;

        public UnitService(IMemoryAPI fface)
        {
            _fface = fface;

            if (_isInitialized) return;

            // Create the UnitArray
            Units = Enumerable.Range(0, UnitArrayMax)
                .Select(x => new Unit(_fface, x))
                .Cast<IUnit>().ToList();

            _isInitialized = true;
        }

        /// <summary>
        /// The zone's unit array.
        /// </summary>
        public static ICollection<IUnit> Units;

        /// <summary>
        /// The unit array's max size: 0 - 2048
        /// </summary>
        private const short UnitArrayMax = Constants.UnitArrayMax;

        /// <summary>
        /// The player's environmental data.
        /// </summary>
        private static IMemoryAPI _fface;

        /// <summary>
        /// Does there exist a mob that has aggroed in general.
        /// </summary>
        public bool HasAggro
        {
            get
            {
                var key = "HasAggro";
                var result = RuntimeCache.Get<bool?>(key);

                if (result.HasValue) return result.Value;
                var hasAggro = MobArray.Any(x => x.HasAggroed);

                RuntimeCache.Set(key, hasAggro, DateTimeOffset.Now.AddSeconds(Constants.UnitArrayCheckRate));
                return hasAggro;
            }
        }

        /// <summary>
        /// Retrieves the list of MOBs.
        /// </summary>
        public ICollection<IUnit> MobArray
        {
            get
            {
                return Units.Where(x => x.NpcType.Equals(NpcType.Mob)).ToList();
            }
        }

        public ICollection<IUnit> NpcUnits
        {
            get
            {
                return Units.Where(x => x.NpcType.Equals(NpcType.NPC)).ToList();
            }
        }
        public ICollection<IUnit> InanimateObjectsUnits
        {
            get
            {
                return Units.Where(x => x.NpcType.Equals(NpcType.InanimateObject)).ToList().FindAll(n => !n.Name.IsNullOrEmpty());
            }
        }

        /// <returns></returns>
        public IUnit GetUnitByName(string name)
        {
            return Units.FirstOrDefault(x => x.Name == name);
        }

        public IUnit GetClosestUnitByPartialName(string name)
        {
            List<IUnit> matchingUnits = Units.Where(x => (x.Name != null && x.Name != ""))
                .Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();
            IUnit unit = matchingUnits.OrderBy(x => x.Distance).FirstOrDefault();
            return unit;

        }
    }
}