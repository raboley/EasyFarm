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

using System.Collections.Generic;
using EliteMMO.API;

namespace MemoryAPI.Inventory
{
    public interface IInventoryTools
    {
        string SelectedItemName { get; }
        uint SelectedItemId { get; }
        uint SelectedItemIndex { get; }
        uint ShopItemCount { get; }
        uint ShopItemCountMax { get; set; }
        int GetContainerCount(int containerId);
        EliteAPI.InventoryItem GetContainerItem(int containerId, int itemIndex);
        int GetContainerMaxCount(int containerId);
        EliteAPI.InventoryItem GetEquippedItem(int slotId);
        bool SetBazaarPrice(int price);
        bool InventoryIsFull(int InventoryContainerId = 0);
        bool HaveItemInInventoryContainer(string itemPattern, int InventoryContainerId = 0);
        List<EliteAPI.IItem> GetMatchingItemsFromContainer(string itemPattern, int InventoryContainerId = 0);
        List<EliteAPI.IItem> GetItemsFromContainer(int InventoryContainerId = 0);
        List<EliteAPI.InventoryItem> GetInventoryItemsFromContainer(int InventoryContainerId = 0);
        int GetCountOfItemsInContainer(string itemPattern, int InventoryContainerId = 0);
    }
}