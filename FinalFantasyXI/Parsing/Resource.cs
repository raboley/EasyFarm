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

namespace FinalFantasyXI.Parsing
{
    public class Resource
    {
        public Resources.ResourceType ResourceType { get; set; }
        public string Id { get; set; }
        public string En { get; set; }
        public string Ja { get; set; }
        public string ActionId { get; set; }
        public string Color { get; set; }
        public string Suffix { get; set; }
        public string Prefix { get; set; }
        public string Access { get; set; }
        public string Command { get; set; }
        public string Equippable { get; set; }
        public string Enl { get; set; }
        public string Jal { get; set; }
        public string Incoming { get; set; }
        public string Outgoing { get; set; }
        public string Element { get; set; }
        public string Alternative { get; set; }
        public string Flags { get; set; }
        public string Stack { get; set; }
        public string Type { get; set; }
        public string Targets { get; set; }
        public string Category { get; set; }
        public string CastTime { get; set; }
        public string Level { get; set; }
        public string Slots { get; set; }
        public string Races { get; set; }
        public string Jobs { get; set; }
        public string MaxCharges { get; set; }
        public string CastDelay { get; set; }
        public string RecastDelay { get; set; }
        public string ShieldSize { get; set; }
        public string Damage { get; set; }
        public string Delay { get; set; }
        public string Skill { get; set; }
        public string ItemLevel { get; set; }
        public string SuperiorLevel { get; set; }
        public string Ens { get; set; }
        public string Jas { get; set; }
        public string IconId { get; set; }
        public string MpCost { get; set; }
        public string RecastId { get; set; }
        public string TpCost { get; set; }
        public string Duration { get; set; }
        public string Range { get; set; }
        public string Endesc { get; set; }
        public string Jadesc { get; set; }
        public string MonsterLevel { get; set; }
        public string TpMoves { get; set; }
        public string Gender { get; set; }
        public string Recast { get; set; }
        public string Levels { get; set; }
        public string IconIdNq { get; set; }
        public string Requirements { get; set; }
        public string Unlearnable { get; set; }
        public string SkillchainA { get; set; }
        public string SkillchainC { get; set; }
        public string SkillchainB { get; set; }
        public string Intensity { get; set; }
        public string Search { get; set; }

        public object As(Type type)
        {
            object resource = Activator.CreateInstance(type);
            ResourceMapper.Map(this, resource);
            return resource;
        }

        public T As<T>()
        {
            return (T)As(typeof(T));
        }
    }
}
