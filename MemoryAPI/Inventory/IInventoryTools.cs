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
        EliteMMO.API.EliteAPI.InventoryItem GetContainerItem(int containerId, int itemIndex);
        int GetContainerMaxCount(int containerId);
        EliteMMO.API.EliteAPI.InventoryItem GetEquippedItem(int slotId);
        bool SetBazaarPrice(int price);
    }
}