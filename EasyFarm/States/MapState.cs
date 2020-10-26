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
using System.Numerics;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media.Media3D;
using EasyFarm.Classes;
using EasyFarm.Context;
using EasyFarm.UserSettings;
using EasyFarm.ViewModels;
using MemoryAPI;
using Pathfinder;
using Player = EasyFarm.Classes.Player;

namespace EasyFarm.States
{
    /// <summary>
    ///     Maps the Current zone.
    /// </summary>
    public class MapState : BaseState
    {
        public override bool Check(IGameContext context)
        {
            return true;
        }

        public override void Run(IGameContext context)
        { 
           LogViewModel.Write("Entered Mapping state");
           var gridFactory = new GridFactory();
           var persister = new FilePersister();
           gridFactory.Persister = persister;

           string mapName = context.Player.Zone.ToString();
           var zoneGrid = gridFactory.LoadGridOrCreateNew(mapName);
           List<Vector3> walkedNodes = new List<Vector3>();

            while (zoneGrid.MapName == context.Player.Zone.ToString())
           // for (int i = 0; i < 1000; i++)
           {

               var player = context.API.Player;
               var myPosition = RoundPlayerPositionToGridPosition(player);


               if (!walkedNodes.Contains(myPosition))
               {
                   walkedNodes.Add(myPosition);
                   zoneGrid.AddKnownNode(myPosition);
                   LogViewModel.Write("in map: " + zoneGrid.MapName + " Adding position: " + myPosition);
               }
           }

           // // Map the NPCs
           // foreach (var unit in context.Units)
           // {
           //     Vector3 position = new Vector3(unit.PosX, unit.PosY, unit.PosZ);
           //     var npc = new NPC(unit.Name, position);
           //     npc.ID = unit.Id;
           //
           //      if (unit.NpcType == NpcType.NPC) 
           //          zoneGrid.AddNpc(npc);
           //
           //      // Map the objects
           //      if (unit.NpcType == NpcType.InanimateObject)
           //          zoneGrid.AddInanimateObject(npc);
           //
           //
           //      // Map the Mobs
           //
           // }


           

           gridFactory.Persister.Save(zoneGrid);

           // Discover all undiscovered nodes.

           // Console.WriteLine(zoneGrid.Print());

        }

        private static Vector3 RoundPlayerPositionToGridPosition(IPlayerTools player)
        {
            Vector3 gridPosition = new Vector3
            {
                X = GridMath.ConvertFromFloatToInt(player.PosX),
                Y = 0,
                Z = GridMath.ConvertFromFloatToInt(player.PosZ)
            };
            return gridPosition;
        }
    }
}