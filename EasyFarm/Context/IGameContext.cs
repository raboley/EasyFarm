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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EasyFarm.Classes;
using EasyFarm.Soul;
using EasyFarm.States;
using EasyFarm.UserSettings;
using MemoryAPI;
using Pathfinder.Map;
using Pathfinder.People;
using Pathfinder.Travel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using Zone = Pathfinder.Map.Zone;

namespace EasyFarm.Context
{
    public interface IGameContext
    {
        IConfig Config { get; set; }
        IDialog Dialog { get; set; }
        IPlayer Player { get; set; }
        IUnit Target { get; set; }
        Boolean IsFighting { get; set; }
        Zone Zone { get; set; }
        IList<IUnit> Units { get; set; }
        IMemoryAPI API { get; set; }
        StateMemory Memory { get; set; }
        IMenu Menu { get; set; }
        IInventory Inventory { get; set; }
        INavigator Navigator { get; set; }
        ZoneMapFactory ZoneMapFactory { get; set; }
        ObservableCollection<Person> Npcs { get; set; }
        ObservableCollection<Person> Mobs { get; set; }
        Traveler Traveler { get; set; }
        PeopleOverseer NpcOverseer { get; set; }
        PersonLooper WoodChopper { get; set; }
        Craft Craft { get; set; }
        IShop Shop { get; set; }
        ITradeMenu Trade { get; set; }
    }


}