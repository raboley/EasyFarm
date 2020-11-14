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
using EliteMMO.API;
using MemoryAPI;
using Player = EasyFarm.Classes.Player;
using StatusEffect = MemoryAPI.StatusEffect;

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

            // context.Player.CurrentGoal = "Signet";
            LogViewModel.Write("I don't have signet, Setting my goal to go get Signet");
            
            // Go to Signet NPC
            // Find Signet Guy
            if (context.Npcs == null)
            {
                return;
            }
            if (context.Npcs.Count == 0)
            {
                LogViewModel.Write("I need signet, but don't know where any NPCs are in this zone");
                return;
            }

            // while (true)
            // {
            //     context.API.Windower.SendKeyPress(Keys.NUMPAD2);
            // }
            //
            var signetPerson = context.Npcs.FirstOrDefault(n => n.Name.Contains("I.M."));
            if (signetPerson == null)
            {
                LogViewModel.Write("I need signet, but can't find the signet person in this zone");
                return;
            }
            
            LogViewModel.Write("Found signet person:" + signetPerson.Name + " At position: " +
                               signetPerson.Position + " Headed there now");
            
            
            // Set route to the path to the signet guy
            var player = context.API.Player;
            var myPosition = ConvertPosition.RoundPositionToVector3(player.Position);

            if (context.Zone.Map == null)
            {
                LogViewModel.Write("Finding path, but zone map is null");
                return;
            }
            
            var path = Pathfinder.Pathing.Pathfinding.FindWaypoints(context.Zone.Map, myPosition, signetPerson.Position);
            if (path.Length > 0)
            {
                var obsWaypoints = ConvertPosition.ConvertVectorArrayToObservableCollectionPosition(path);
                context.Config.Route.Waypoints = obsWaypoints;
            
                //// Actually walking
                Navigator.StartRoute(context);
            }
            
            // Got to the signet person
            LogViewModel.Write("Made it to signet person!");
            Config.Instance.Route.Reset();
            context.API.Navigator.Reset();
            
            // Get signet
            
            
            // Got Signet, reset goal
            // context.Player.CurrentGoal = "Aimless";


            // Get Signet

            // Set goal to not signet
        }

        private static bool HasSignet(IPlayerTools player)
        {
            bool hasSignet = false;
            if (player.StatusEffects != null)
            {
                hasSignet = player.StatusEffects.ToList().Contains(StatusEffect.Signet);
                return hasSignet;
            }

            return hasSignet;
        }
    }
}