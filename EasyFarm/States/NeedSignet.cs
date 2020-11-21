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
using System.Threading;
using EasyFarm.Classes;
using EasyFarm.Context;
using EasyFarm.ViewModels;
using MemoryAPI;
using Pathfinder.People;
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

            const string signetNpcZone = "Bastok_Markets";
            // context.Player.CurrentGoal = "Signet";
            LogViewModel.Write("I don't have signet, Setting my goal to go get Signet");
            while (context.Traveler == null)
            {
               Thread.Sleep(100); 
            }

            var signetNpc = context.Traveler.SearchForClosestSignetNpc("Bastok");
            while (signetNpc == null && context.API.Player.Zone.ToString() != signetNpcZone)
            {
                context.Traveler.GoToZone(signetNpcZone);
            }

            while (context.Traveler.Zoning)
            {
               Thread.Sleep(100); 
            }

            signetNpc = context.Traveler.SearchForClosestSignetNpc("Bastok");

            if (signetNpc.MapName != context.Traveler.CurrentZone.Name)
                return;
            
            context.Traveler.PathfindAndWalkToFarAwayWorldMapPosition(signetNpc.Position);

            IMemoryAPI fface = context.API;
            AskForSignet(context, fface, signetNpc);
            
        }
        private static void AskForSignet(IGameContext context, IMemoryAPI fface, Person signetNpc)
        {

            IUnit signetUnit = context.Memory.UnitService.GetClosestUnitByPartialName(signetNpc.Name);
            context.Navigator.InteractWithUnit(context, fface, signetUnit);
            TimeWaiter.Pause(2000);
            context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
            TimeWaiter.Pause(5000);
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