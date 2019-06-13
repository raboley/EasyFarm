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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static EliteMMO.API.EliteAPI;

namespace EasyFarm.States
{
    public class SetSearchNpcState : BaseState
    {
        //private DateTime? _lastTargetCheck;

        public override bool Check(IGameContext context)
        {
            // None of this is ready, so don't ever run it currently.
            return true;
        }

        public override void Run(IGameContext context)
        {

            string json = JsonConvert.SerializeObject(context);
            JToken jt = JToken.Parse(json);
            string formattedJson = jt.ToString();
            System.IO.File.WriteAllText(@"D:\context.txt", json);
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

            /*
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
            */

            // Try to move to an NPC
            //const bool _shouldKeepRunningToNextWaypoint = false;
            //var npc = context.Memory.UnitService.GetUnitByName("Rashid");
            //var goal = npc.Position;

            //LogViewModel.Write("Want to meet " + npc.Name + " at " + npc.Position.ToString());
            //context.API.Navigator.GotoWaypoint(
            //    goal,
            //    context.Config.IsObjectAvoidanceEnabled,
            //    _shouldKeepRunningToNextWaypoint);

            //context.API.Navigator.Reset();


            //var npc2 = context.Memory.UnitService.GetUnitByName("Rashid");
            //var goal2 = npc.Position;

            //LogViewModel.Write("Want to meet " + npc2.Name + " at " + npc2.Position.ToString());
            //context.API.Navigator.GotoWaypoint(
            //    goal2,
            //    context.Config.IsObjectAvoidanceEnabled,
            //    _shouldKeepRunningToNextWaypoint);
            //context.API.Navigator.Reset();

            // buy things from auction house using auctioneer
        }

        //private bool ShouldCheckTarget()
        //{
        //    if (_lastTargetCheck == null) return true;
        //    return DateTime.Now >= _lastTargetCheck.Value.AddSeconds(Constants.UnitArrayCheckRate);
        //}
    }
}