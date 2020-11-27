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
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using EasyFarm.Classes;
using EasyFarm.Context;
using EasyFarm.ViewModels;
using MemoryAPI;
using Pathfinder;
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
            if (context.Traveler?.Zoning == true)
                return;
            // context.Player.CurrentGoal = "Signet";
            LogViewModel.Write("I don't have signet, Setting my goal to go get Signet");
            while (context.Traveler == null)
            {
                Thread.Sleep(100);
            }

            var nation = context.Player.Nation.ToString();
            var signetNpc = SearchWorldForSignetPerson(context);
            if (signetNpc == null)
            {
                Debug.WriteLine("Can't Find signet NPC in all known Zones for nation: " + nation);
                return;
            }

            while (context.API.Player.Zone.ToString() != signetNpc.MapName)
            {
                context.Traveler.GoToZone(signetNpc.MapName);
            }

            while (context.Traveler.Zoning)
            {
                Thread.Sleep(100);
            }

            if (signetNpc.MapName != context.Traveler.CurrentZone.Name)
                return;

            context.Traveler.PathfindAndWalkToFarAwayWorldMapPosition(signetNpc.Position);

            IMemoryAPI fface = context.API;

            if (GridMath.GetDistancePos(context.Traveler.Walker.CurrentPosition, signetNpc.Position) > 1)
                return;

            AskForSignet(context, fface, signetNpc);
        }

        private static Person SearchWorldForSignetPerson(IGameContext context)
        {
            Person signetNpc;
            string nationString = context.API.Player.Nation.ToString();
            signetNpc = context.Traveler.SearchForClosestSignetNpc(nationString);

            // signet NPC is in this zone
            if (signetNpc != null)
                return signetNpc;

            while (context.NpcOverseer == null)
            {
                Debug.Write(
                    "SearchWorldForSignetPerson is Waiting for NpcOverseer to be non null so it can use it to search all NPCs");
                Thread.Sleep(200);
            }

            List<Person> allPeople = context.NpcOverseer.GetAllPeople();
            string npcpattern = context.Traveler.GetSignetNpcPatternByNation(nationString);
            signetNpc = allPeople.Find(p => p.Name.Contains(npcpattern));

            return signetNpc;
        }

        private static void AskForSignet(IGameContext context, IMemoryAPI fface, Person signetNpc)
        {
            IUnit signetUnit = context.Memory.UnitService.GetClosestUnitByPartialName(signetNpc.Name);
            context.Navigator.InteractWithUnit(context, fface, signetUnit);

            while (!context.API.Chat.LastThingSaid().Contains("I will bestow upon you"))
            {
                context.API.Windower.SendKeyPress(EliteMMO.API.Keys.RETURN);
                TimeWaiter.Pause(1000);
            }

            // wait for signet animation to be over
            TimeWaiter.Pause(7000);
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