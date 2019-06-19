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
using MemoryAPI;
using System;

namespace EasyFarm.Classes
{
    public class EquipmentSet
    {

        public void Equip(EquipableItem equipableItem)
        {
            if (equipableItem.Slot == EquipSlots.Main)
            {
                Main = equipableItem;
            }
        }

        // EquipmentService.Equip(SomeItem, EquipmentSet);


        // Controller -> Service
        // Services -- biz logic anything with the word IF or switch kind of stuff
        // POCO folder Play OLD C# Object
        // Repo -- this is GET PUT on the DB
        // Use factory to create the store
        // Store -- this is ACTUAL DB access

        //public Equipment(string name = "Empty")
        //{
        //    Main = new EquipableItem(name, EquipSlots.Main);
        //    Sub = new EquipableItem(name, EquipSlots.Sub);
        //    Range = new EquipableItem(name, EquipSlots.Range);
        //    Ammo = new EquipableItem(name, EquipSlots.Ammo);
        //    Head = new EquipableItem(name, EquipSlots.Head);
        //    Neck = new EquipableItem(name, EquipSlots.Neck);
        //    Body = new EquipableItem(name, EquipSlots.Body);
        //    Hands = new EquipableItem(name, EquipSlots.Hands);
        //    Waist = new EquipableItem(name, EquipSlots.Waist);
        //    Back = new EquipableItem(name, EquipSlots.Back);
        //    Legs = new EquipableItem(name, EquipSlots.Legs);
        //    Feet = new EquipableItem(name, EquipSlots.Feet);
        //    Ring1 = new EquipableItem(name, EquipSlots.Ring1);
        //    Ring2 = new EquipableItem(name, EquipSlots.Ring2);
        //    Ear1 = new EquipableItem(name, EquipSlots.Ear1);
        //    Ear2 = new EquipableItem(name, EquipSlots.Ear2);
        //}

        public int Level { get; set; }

        private IEquipableItem _main;
        private IEquipableItem _sub;
        private IEquipableItem _Range;
        private IEquipableItem _Ammo;
        private IEquipableItem _Head;
        private IEquipableItem _Neck;
        private IEquipableItem _Body;
        private IEquipableItem _Hands;
        private IEquipableItem _Waist;
        private IEquipableItem _Back;
        private IEquipableItem _Legs;
        private IEquipableItem _Feet;
        private IEquipableItem _Ring1;
        private IEquipableItem _Ring2;
        private IEquipableItem _Ear1;
        private IEquipableItem _Ear2;


        public IEquipableItem Main
        {
            get => _main ?? (_main = new EquipableItem(EquipSlots.Main));
            set => _main = value;
        }

        public IEquipableItem Sub
        {
            get => _sub ?? (_sub = new EquipableItem(EquipSlots.Sub));
            set => _sub = value;
            }
        public IEquipableItem Range
        {
            get => _Range ?? (_Range = new EquipableItem(EquipSlots.Range));
            set => _Range = value;
            }
        public IEquipableItem Ammo
        {
            get => _Ammo ?? (_Ammo = new EquipableItem(EquipSlots.Ammo));
            set => _Ammo = value;
            }
        public IEquipableItem Head
        {
            get => _Head ?? (_Head = new EquipableItem(EquipSlots.Head));
            set => _Head = value;
            }
        public IEquipableItem Neck
        {
            get => _Neck ?? (_Neck = new EquipableItem(EquipSlots.Neck));
            set => _Neck = value;
            }
        public IEquipableItem Body
        {
            get => _Body ?? (_Body = new EquipableItem(EquipSlots.Body));
            set => _Body = value;
            }
        public IEquipableItem Hands
        {
            get => _Hands ?? (_Hands = new EquipableItem(EquipSlots.Hands));
            set => _Hands = value;
            }
        public IEquipableItem Waist
        {
            get => _Waist ?? (_Waist = new EquipableItem(EquipSlots.Waist));
            set => _Waist = value;
            }
        public IEquipableItem Back
        {
            get => _Back ?? (_Back = new EquipableItem(EquipSlots.Back));
            set => _Back = value;
            }
        public IEquipableItem Legs
        {
            get => _Legs ?? (_Legs = new EquipableItem(EquipSlots.Legs));
            set => _Legs = value;
            }
        public IEquipableItem Feet
        {
            get => _Feet ?? (_Feet = new EquipableItem(EquipSlots.Feet));
            set => _Feet = value;
            }
        public IEquipableItem Ring1
        {
            get => _Ring1 ?? (_Ring1 = new EquipableItem(EquipSlots.Ring1));
            set => _Ring1 = value;
            }
        public IEquipableItem Ring2
        {
            get => _Ring2 ?? (_Ring2 = new EquipableItem(EquipSlots.Ring2));
            set => _Ring2 = value;
            }
        public IEquipableItem Ear1
        {
            get => _Ear1 ?? (_Ear1 = new EquipableItem(EquipSlots.Ear1));
            set => _Ear1 = value;
            }
        public IEquipableItem Ear2
        {
            get => _Ear2 ?? (_Ear2 = new EquipableItem(EquipSlots.Ear2));
            set => _Ear2 = value;
        }



    }
}