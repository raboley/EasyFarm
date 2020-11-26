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
using EasyFarm.Classes;
using EasyFarm.Context;
using EasyFarm.ViewModels;
using EasyFarm.Views;
using EliteMMO.API;
using MemoryAPI;
using Pathfinder.Travel;
using Zone = Pathfinder.Map.Zone;

namespace EasyFarm.States
{
    public class ZoneState : BaseState
    {
        public Action ZoningAction { get; set; } = () => TimeWaiter.Pause(500);

        private bool IsZoning(IGameContext context) => context.Player.Str == 0;

        public override void Enter(IGameContext context)
        {
            // if (context.Zone.Name == Zone.Unknown.ToString())
            // {
            //     // context.Zone = context.Player.Zone;
            // }
        }

        public override bool Check(IGameContext context)
        {
            if (context.Zone?.Map == null)
                return false;

            var zoning = Zoning(context);
            return zoning;
        }

        private bool Zoning(IGameContext context)
        {
            var zone = context.Player.Zone;
            bool zoning = ZoneChanged(zone.ToString(), context) || IsZoning(context);
            return zoning;
        }

        private bool ZoneChanged(string currentZone, IGameContext context)
        {
            if (context.Traveler?.CurrentZone?.Map?.MapName == null)
                return true;
            
            return context.Traveler.CurrentZone.Map.MapName != currentZone;
        }


        public override void Run(IGameContext context)
        {
            // Set new currentZone.
            // context.Zone = context.Player.Zone;
            var lastZone = context.Traveler.CurrentZone.Map.MapName;

            // Stop program from running to next waypoint.
            context.API.Navigator.Reset();

            // Wait until we are done zoning.
            while (Zoning(context))
            {
                ZoningAction();
            }

            // If we don't know the zone boundary to get back to where we were, move backward till we zone again.
            while (context.Traveler?.CurrentZone == null)
            {
                ZoningAction();
            }

            if (context.Traveler.GetBorderZonePosition(lastZone) == null)
            {
                LogViewModel.Write("Don't have the zone border on this sid from: " + lastZone + "To here: " + context.API.Player.Zone);
                LogViewModel.Write("Backing up until I zone again so I can record this zone and can get back.");
                context.API.Windower.SendKeyDown(Keys.NUMPAD2);
                while (!Zoning(context))
                    ZoningAction();
                context.API.Windower.SendKeyUp(Keys.NUMPAD2);
                LogViewModel.Write("Should be zoning Now!");
            }
        }
    }
}