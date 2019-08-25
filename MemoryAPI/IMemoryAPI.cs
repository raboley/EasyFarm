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
using MemoryAPI.Chat;
using MemoryAPI.Dialog;
using MemoryAPI.Inventory;
using MemoryAPI.Menu;
using MemoryAPI.Resources;
using MemoryAPI.Windower;

namespace MemoryAPI
{
    public interface IMemoryAPI
    {
        INavigatorTools Navigator { get; set; }
        INPCTools NPC { get; set; }
        System.Collections.Generic.Dictionary<byte, IPartyMemberTools> PartyMember { get; set; }
        IPlayerTools Player { get; set; }
        ITargetTools Target { get; set; }
        ITimerTools Timer { get; set; }
        IWindowerTools Windower { get; set; }
        IChatTools Chat { get; set; }
        IResourcesTools Resource { get; set; }
        IMenuTools Menu { get; set; }
        IDialogTools Dialog { get; set; }
        IInventoryTools Inventory { get; set; }
        string ConfigFilePath { get; set; }
    }
}