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
using System.Linq;
using EasyFarm.Classes;
using EasyFarm.Context;
using EasyFarm.UserSettings;
using EasyFarm.ViewModels;
using MemoryAPI;
using Player = EasyFarm.Classes.Player;

namespace EasyFarm.States
{
    /// <summary>
    ///     Moves to target enemies.
    /// </summary>
    public class NeedSignet : BaseState
    {
        public override bool Check(IGameContext context)
        {
            if (HasSignet(context.API.Player))
                return false;

            // or if they have the goal of getting signet
            if (context.Player.CurrentGoal == "Signet")
                return false;

            return true;
        }

        public override void Run(IGameContext context)
        {

            context.Player.CurrentGoal = "Signet";
            LogViewModel.Write("I don't have signet, Setting my goal to go get Signet");
            
            // Go to Signet NPC



            // Get Signet

            // Set goal to not signet
        }

        private static bool HasSignet(IPlayerTools player)
        {
            bool hasSignet = false;
            if (player.StatusEffects != null)
            {
                var signet = player.StatusEffects.ToList().Find(n => n == StatusEffect.Signet);
                if (signet != null)
                    hasSignet = true;
            }

            return hasSignet;
        }
    }
}