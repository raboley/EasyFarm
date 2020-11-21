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

namespace EasyFarm.Parsing
{
    public class ResourceHelper
    {
        public static AbilityType ToAbilityType(string abilityType)
        {
            switch (abilityType)
            {
                case "/jobability":
                    return AbilityType.Jobability;
                case "/magic":
                    return AbilityType.Magic;
                case "/monsterskill":
                    return AbilityType.Monsterskill;
                case "/ninjutsu":
                    return AbilityType.Ninjutsu;
                case "/pet":
                    return AbilityType.Pet;
                case "/range":
                    return AbilityType.Range;
                case "/song":
                    return AbilityType.Song;
                case "/weaponskill":
                    return AbilityType.Weaponskill;
                case "/trust":
                    return AbilityType.Trust;
                case "/item":
                    return AbilityType.Item;
                default:
                    return AbilityType.Unknown;
            }
        }

        public static CategoryType ToCategoryType(string categoryType)
        {
            switch (categoryType)
            {
                case "WhiteMagic":
                    return CategoryType.WhiteMagic;
                case "BlackMagic":
                    return CategoryType.BlackMagic;
                case "SummonerPact":
                    return CategoryType.SummonerPact;
                case "Ninjustsu":
                    return CategoryType.Ninjustsu;
                case "Geomancy":
                    return CategoryType.Geomancy;
                case "BlueMagic":
                    return CategoryType.BlueMagic;
                case "BardSong":
                    return CategoryType.BardSong;
                case "Trust":
                    return CategoryType.Trust;
                case "WeaponSkill":
                    return CategoryType.WeaponSkill;
                case "Misc":
                    return CategoryType.Misc;
                case "JobAbility":
                    return CategoryType.JobAbility;
                case "PetCommand":
                    return CategoryType.PetCommand;
                case "CorsairRoll":
                    return CategoryType.CorsairRoll;
                case "CorsairShot":
                    return CategoryType.CorsairShot;
                case "Samba":
                    return CategoryType.Samba;
                case "Waltz":
                    return CategoryType.Waltz;
                case "Jig":
                    return CategoryType.Jig;
                case "Step":
                    return CategoryType.Step;
                case "Flourish1":
                    return CategoryType.Flourish1;
                case "Flourish2":
                    return CategoryType.Flourish2;
                case "Effusion":
                    return CategoryType.Effusion;
                case "Rune":
                    return CategoryType.Rune;
                case "Ward":
                    return CategoryType.Ward;
                case "BloodPactWard":
                    return CategoryType.BloodPactWard;
                case "BloodPactRage":
                    return CategoryType.BloodPactRage;
                case "Monster":
                    return CategoryType.Monster;
                case "JobTrait":
                    return CategoryType.JobTrait;
                case "MonsterSkill":
                    return CategoryType.MonsterSkill;
                default:
                    return CategoryType.Unknown;
            }
        }

        public static ElementType ToElementType(string elementType)
        {
            switch (elementType)
            {
                case "All":
                    return ElementType.All;
                case "Any":
                    return ElementType.Any;
                case "Dark":
                    return ElementType.Dark;
                case "Earth":
                    return ElementType.Earth;
                case "Fire":
                    return ElementType.Fire;
                case "Ice":
                    return ElementType.Ice;
                case "Light":
                    return ElementType.Light;
                case "None":
                    return ElementType.None;
                case "NonElemental":
                    return ElementType.NonElemental;
                case "Thunder":
                    return ElementType.Thunder;
                case "Trigger":
                    return ElementType.Trigger;
                case "Water":
                    return ElementType.Water;
                case "Wind":
                    return ElementType.Wind;
                default:
                    return ElementType.Unknown;
            }
        }

        public static SkillType ToSkillType(string skillType)
        {
            switch (skillType)
            {
                case "HealingMagic":
                    return SkillType.HealingMagic;
                case "DivineMagic":
                    return SkillType.DivineMagic;
                case "EnfeeblingMagic":
                    return SkillType.EnfeeblingMagic;
                case "EnhancingMagic":
                    return SkillType.EnhancingMagic;
                case "ElementalMagic":
                    return SkillType.ElementalMagic;
                case "DarkMagic":
                    return SkillType.DarkMagic;
                case "SummoningMagic":
                    return SkillType.SummoningMagic;
                case "Ninjutsu":
                    return SkillType.Ninjutsu;
                case "Singing":
                    return SkillType.Singing;
                case "BlueMagic":
                    return SkillType.BlueMagic;
                case "Geomancy":
                    return SkillType.Geomancy;
                case "Ability":
                    return SkillType.Ability;
                default:
                    return SkillType.Unknown;
            }
        }

        public static TargetType ToTargetTypeFlags(string targetType)
        {
            if (targetType == null) return TargetType.Unknown;

            int rawTargetType;
            if (!int.TryParse(targetType, out rawTargetType))
                return TargetType.Unknown;

            var value = (TargetType)rawTargetType;
            return value;
        }

        public static TargetType ToTargetType(string targetType)
        {
            switch (targetType)
            {
                case "Enemy":
                    return TargetType.Enemy;
                case "NPC":
                    return TargetType.Npc;
                case "Ally":
                    return TargetType.Ally;
                case "Party":
                    return TargetType.Party;
                case "Player":
                    return TargetType.Player;
                case "Self":
                    return TargetType.Self;
                default:
                    return TargetType.Unknown;
            }
        }

        /// <summary>
        ///     Represents all the types that are spells or casted.
        /// </summary>
        public static bool IsSpell(AbilityType abilityType)
        {
            switch (abilityType)
            {
                case AbilityType.Magic:
                case AbilityType.Ninjutsu:
                case AbilityType.Song:
                case AbilityType.Item:
                case AbilityType.Trust:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        ///     Represents all the types that are not spells or casted.
        /// </summary>
        public static bool IsAbility(AbilityType abilityType)
        {
            switch (abilityType)
            {
                case AbilityType.Weaponskill:
                case AbilityType.Range:
                case AbilityType.Jobability:
                case AbilityType.Pet:
                case AbilityType.Monsterskill:
                    return true;

                default:
                    return false;
            }
        }

        public static string ToPrefix(Resource resource)
        {
            switch (resource.ResourceType)
            {
                case Parsing.Resources.ResourceType.Items:
                    return "/item";
                case Parsing.Resources.ResourceType.MonsterAbilities:
                    return "/monsterskill";
            }

            return resource.Prefix;
        }
    }
}