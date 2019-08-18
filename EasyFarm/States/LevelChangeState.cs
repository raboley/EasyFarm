﻿using EasyFarm.Context;
using MemoryAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static EliteMMO.API.EliteAPI;

namespace EasyFarm.States
{
    public class LevelChangeState : BaseState
    {
        private int _last_level = 0;
        private static int attemptToBuyFromAH = 1;

        public override bool Check(IGameContext context)
        {

            if (_last_level != context.Player.JobLevel)
            {
                _last_level = context.Player.JobLevel;
                return true;
            }

            return false;
        }

        public override void Run(IGameContext context)
        {
            EquipOptimalGear(context);
        }

        private static void EquipOptimalGear(IGameContext context)
        {

            // Determine Optimal Gear from json or something
            Dictionary<string, string> optimalEquipment;

            optimalEquipment = DetermineOptimalGearForMainJobAndLevel(context);
            if(optimalEquipment.Count == 0)
            {
                return;
            }

            // Figure out what gear is not already equiped
            List<InventoryItem> currentEquipment = context.Memory.EliteApi.Player.Equipment;
            Dictionary<string, string> neededEquipment = GetOptimalGearNotAlreadyEquipped(context, optimalEquipment, currentEquipment);

            
            // loop through all needed items to buy & or equip them depending on config flag
            foreach (KeyValuePair<string, string> equip in neededEquipment.AsEnumerable())
            {
                var slot = equip.Key;
                var name = equip.Value;
                // Buy the gear from AH if flag is set to buy new gear
                if (attemptToBuyFromAH == 1)
                {
                    BuyItemFromAuctionHouseUsingAuctioneerAddOn(context, name);
                }
                EquipItemToSlot(context, slot, name);
            }
            //PrintCurrentEquipmentList(context, currentEquipment);
        }

        private static Dictionary<string, string> GetOptimalGearNotAlreadyEquipped(IGameContext context, Dictionary<string, string> Equips, List<InventoryItem> currentEquipment)
        {
            Dictionary<string, string> neededEquipment = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> equip in Equips.AsEnumerable())
            {
                // Go through each piece of armor in the array and check if that item is already equip. if it isn't buy it and equip it
                var slot = equip.Key;
                var name = equip.Value;
                // determine if this item is already equip
                // get the slot id of the item we are trying to check for.
                EquipSlots slotEnum = (EquipSlots)Enum.Parse(typeof(EquipSlots), slot);
                int slotId = (int)slotEnum;

                string currentlyEquipItemName;
                //// 
                int equipmentId = currentEquipment[slotId].Id;
                if (equipmentId == 0)
                {
                    currentlyEquipItemName = "";
                }
                else
                {
                    // Now get the name from the resources file
                    var item = context.Memory.EliteApi.Resource.GetItem(equipmentId);
                    if (item == null)
                    {
                        currentlyEquipItemName = "";
                    }
                    else
                    {
                        currentlyEquipItemName = item.Name[0].ToString();
                    }
                }
                if (currentlyEquipItemName.ToLower() != name.ToLower())
                {
                    neededEquipment.Add(slot, name);
                }

            }
            return neededEquipment;
        }

        public static Dictionary<string, string> DetermineOptimalGearForMainJobAndLevel(IGameContext context)
        {
            string job = context.Player.Job.ToString();
            int level = context.Player.JobLevel;

            var dancerEquips = new Dictionary<int, Dictionary<string, string>>()
            {
                {1 , new Dictionary<string, string> {
                        //{"Main", "Bronze Knife +1" },
                        //{"Head", "Bronze Cap +1" },
                        //{"Body", "I.R. Jack Coat" },
                        //{"Legs", "Bronze Subligar +1" },
                        //{"Hands", "Bronze Mittens +1" },
                        //{"Feet", "Bronze Leggings +1" },
                        //{"Waist", "Blood Stone" },
                        {"Ear1", "Cassie Earring" },
                        {"Ear2", "Cassie Earring" },
                    }
                }
            };

            var whiteMageEquips = new Dictionary<int, Dictionary<string, string>>()
            {
                {33 , new Dictionary<string, string> {
                    {"Main", "Spiked Club +1" },
                    {"Sub", "Lizard Strap +1" },
                    {"Ammo", "Morion Tathlum" },
                    {"Head", "Trump Crown" },
                    {"Neck", "Holy Phial" },
                    {"Body", "Seer's Tunic +1" },
                    {"Hands", "Savage gauntlets" },
                    {"Legs", "Seer's Slacks +1" },
                    {"Feet", "Seer's Pumps +1" },
                    {"Back", "Talisman Cape" },
                    {"Waist", "Mohbwa Sash +1" },
                    {"Ear1", "Reraise Earring" },
                    {"Ear2", "Magician's Earring" },
                    {"Ring1", "Saintly Ring +1" },
                    {"Ring2", "Saintly Ring +1" },
                    }
                }
            };
            Dictionary<int, Dictionary<string, string>> Equips = new Dictionary<int, Dictionary<string, string>>();

            switch (job)
            {
                case "Dancer":
                    Equips = dancerEquips;
                    break;
                case "WhiteMage":
                    Equips = whiteMageEquips;
                    break;
                default:
                    Equips = new Dictionary<int, Dictionary<string, string>>();
                    break;
            }

            Dictionary<string, string> optimal_equipment = new Dictionary<string, string>();

            try
            {
                optimal_equipment = Equips[level];
            }
            catch (Exception)
            {
                Console.WriteLine("No valid level yet!");
            }
            return optimal_equipment;
        }


        private static void EquipItemToSlot(IGameContext context, string slot, string name)
        {
            Thread.Sleep(1000);
            context.Memory.Executor.SendCommand("/equip " + slot + " " + '"' + name + '"');
        }

        private static void BuyItemFromAuctionHouseUsingAuctioneerAddOn(IGameContext context, string name)
        {
            Thread.Sleep(1000);
            context.Memory.Executor.SendCommand("/buy " + '"' + name + '"' + " single 100");
        }

        private static void PrintCurrentEquipmentList(IGameContext context, List<InventoryItem> head)
        {
            for (int i = 0; i < head.Count; i++)
            {
                InventoryItem slotId = head[i];
                string itemName;
                int equipmentId = slotId.Id;

                if (equipmentId != 0)
                {
                    var item = context.Memory.EliteApi.Resource.GetItem(equipmentId);
                    if (item == null)
                    {
                        itemName = "";
                    }
                    else
                    {
                        itemName = item.Name[0].ToString();
                    }
                }
                else
                {
                    itemName = "";
                }
                Console.WriteLine("slot index:" + i + " has an item id of: " + equipmentId + " which is an item name of " + itemName);
            }
        }
    }
}
