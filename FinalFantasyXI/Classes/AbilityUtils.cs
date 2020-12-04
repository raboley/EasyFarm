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

using FinalFantasyXI.Parsing;
using MemoryAPI;

namespace FinalFantasyXI.Classes
{
    public class AbilityUtils
    {
        /// <summary>
        ///     Checks whether a spell or ability can be recasted.
        /// </summary>
        /// <param name="fface"></param>
        /// <param name="ability"></param>
        /// <returns></returns>
        public static bool IsRecastable(IMemoryAPI fface, BattleAbility ability)
        {
            var recast = 0;

            // No recast time on weaponskills. 
            if (ability.AbilityType == AbilityType.Weaponskill) return true;

            // No recast for ranged attacks. 
            if (AbilityType.Range == ability.AbilityType) return true;

            // If a spell get spell recast
            if (ResourceHelper.IsSpell(ability.AbilityType))
            {
                recast = fface.Timer.GetSpellRecast(ability.Index);
            }

            // if ability get ability recast. 
            if (ResourceHelper.IsAbility(ability.AbilityType))
            {
                recast = fface.Timer.GetAbilityRecast(ability.Index);
            }

            return recast <= 0;
        }

    }
}