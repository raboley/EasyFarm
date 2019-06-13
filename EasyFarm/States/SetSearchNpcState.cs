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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EasyFarm.Classes;
using EasyFarm.Context;
using EasyFarm.UserSettings;
using EasyFarm.ViewModels;
using MemoryAPI;
using static EliteMMO.API.EliteAPI;

namespace EasyFarm.States
{
    public class SetSearchNpcState : BaseState
    {
        //private DateTime? _lastTargetCheck;

        public override bool Check(IGameContext context)
        {
            //    // Currently fighting, do not change target. 
            //    if (!context.Target.IsValid)
            //    {
            //        // Still not time to update for new target. 
            //        if (!ShouldCheckTarget()) return false;

            //        // First get the first mob by distance.
            //        var mobs = context.Units.Where(x => x.IsValid).ToList();
            //        mobs = TargetPriority.Prioritize(mobs).ToList();

            //        // Set our new target at the end so that we don't accidentally cast on a new target.
            //        context.Target = mobs.FirstOrDefault() ?? new NullUnit();

            //        // Update last time target was updated. 
            //        _lastTargetCheck = DateTime.Now;
            const int TEST = 0;
            if (TEST == 1)
            {
                var signet = context.Memory.EliteApi.Player.StatusEffects.Where(x => x.ToString() == "Signet");
                if (signet.Count() != 1)
                {
                    LogViewModel.Write("Need signet!");

                }
                else
                {
                    LogViewModel.Write("Have signet!");
                }
                //    }

                // Getting party info for players other than ourself
                var party = context.Memory.EliteApi.PartyMember.Where(x => x.Value.Name != "").Skip(1);
                foreach (KeyValuePair<byte, MemoryAPI.IPartyMemberTools> kvp in party)
                {
                    LogViewModel.Write("We are in a Party with " + kvp.Value.Name);
                }

                // Get the nearby mobs and check for aggro on my party members
                // Might have to have some communication to make the logic for links to work.


                // chat relayer
                const int TELL_CHAT_ID = 12;
                var tellChat = context.Memory.EliteApi.Chat.ChatEntries.Where(x => x.ChatType == TELL_CHAT_ID);

                foreach (var chat in tellChat)
                {
                    LogViewModel.Write("TELL" + chat.Text);
                    // TODO: Only write out tells once instead of every time this is run
                }
            }

            // Try to move to an NPC
            const bool _shouldKeepRunningToNextWaypoint = false;
            //var npc = context.Memory.UnitService.GetUnitByName("Rashid");
            //var goal = npc.Position;

            //LogViewModel.Write("Want to meet " + npc.Name + " at " + npc.Position.ToString());
            //context.API.Navigator.GotoWaypoint(
            //    goal,
            //    context.Config.IsObjectAvoidanceEnabled,
            //    _shouldKeepRunningToNextWaypoint);

            context.API.Navigator.Reset();
            string job = context.Memory.EliteApi.Player.Job.ToString();

            var level = context.Memory.EliteApi.Player.JobLevel;
            var head = context.Memory.EliteApi.Player.Equipment;

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
            var Equips = new Dictionary<int, Dictionary<string, string>>();

            if (job == "Dancer")
            {
                Equips = dancerEquips;
            }

            switch (job)
            {
                case "Dancer":
                    Equips = dancerEquips;
                    break;
                case "WhiteMage":
                    Equips = whiteMageEquips;
                    break;
                default:
                    Equips = whiteMageEquips;
                    break;
            }

            //PrintCurrentEquipmentList(context, head);

            foreach (KeyValuePair<string, string> equip in Equips[level].AsEnumerable())
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
                int equipmentId = head[slotId].Id;
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
                    //Console.WriteLine("Need to equip:'" + name + "' in slot:" + slot);
                    BuyItemFromAuctionHouseAndEquipIt(context, slot, name);

                    //}
                    //else
                    //{
                    //    context.Memory.Executor.SendCommand("/s I already have: " + name + " Equiped");
                    //}
                } 
                }

            PrintCurrentEquipmentList(context, head);

            //var npc2 = context.Memory.UnitService.GetUnitByName("Rashid");
            //var goal2 = npc.Position;

            //LogViewModel.Write("Want to meet " + npc2.Name + " at " + npc2.Position.ToString());
            //context.API.Navigator.GotoWaypoint(
            //    goal2,
            //    context.Config.IsObjectAvoidanceEnabled,
            //    _shouldKeepRunningToNextWaypoint);
            //context.API.Navigator.Reset();

            // buy things from auction house using auctioneer


            return false;
        }

        private static void BuyItemFromAuctionHouseAndEquipIt(IGameContext context, string slot, string name)
        {
            context.Memory.Executor.SendCommand("/s hi I need to buy:" + name + " For my: " + slot);
            Thread.Sleep(1000);
            context.Memory.Executor.SendCommand("/buy " + '"' + name + '"' + " single 100");
            Thread.Sleep(1000);
            context.Memory.Executor.SendCommand("/equip " + slot + " " + '"' + name + '"');
            Thread.Sleep(1000);
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


        //private bool ShouldCheckTarget()
        //{
        //    if (_lastTargetCheck == null) return true;
        //    return DateTime.Now >= _lastTargetCheck.Value.AddSeconds(Constants.UnitArrayCheckRate);
        //}
    }
}