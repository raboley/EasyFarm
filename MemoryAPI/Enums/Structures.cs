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

using System.Runtime.InteropServices;
using EliteMMO.API;

namespace MemoryAPI
{
    public class Structures
    {
        /// <summary>
        /// Stats of the player
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct PlayerStats
        {
            public short Str;
            public short Dex;
            public short Vit;
            public short Agi;
            public short Int;
            public short Mnd;
            public short Chr;
        } // @ public struct PlayerStats
        
        
        public class CraftSkill
        {

            public bool Capped;
            public int Rank;
            public int Skill;

            public CraftSkill(EliteAPI.PlayerCraftSkill playerCraftSkill)
            {
                Capped = playerCraftSkill.Capped;
                Rank = playerCraftSkill.Rank;
                Skill = playerCraftSkill.Skill;
            }
        }
        
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct CraftSkills
        {
            public CraftSkill Fishing;
            public CraftSkill Woodworking;
            public CraftSkill Smithing;
            public CraftSkill Goldsmithing;
            public CraftSkill Clothcraft;
            public CraftSkill Leathercraft;
            public CraftSkill Bonecraft;
            public CraftSkill Alchemy;
            public CraftSkill Cooking;
            public CraftSkill Synergy;
            public CraftSkill Riding;
        } // @ public struct PlayerStats
        
    }
}