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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media.Media3D;
using EasyFarm.Classes;
using EasyFarm.Context;
using EasyFarm.UserSettings;
using EasyFarm.ViewModels;
using MahApps.Metro.Controls;
using MemoryAPI;
using Pathfinder;
using Navigator = EasyFarm.Classes.Navigator;
using Player = EasyFarm.Classes.Player;
using Position = MemoryAPI.Navigation.Position;

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
           IMemoryAPI fface = context.API;
           context.Memory.EliteApi.Navigator.DistanceTolerance = 1;
            var gridFactory = new GridFactory();
           var persister = new FilePersister();
           var mapsDirectory = GetMapsDirectory();


           persister.DefaultExtension = "json";
           persister.FilePath = mapsDirectory;
           gridFactory.Persister = persister;

           gridFactory.DefaultGridSize = new Vector2(600f, 600f);

           string mapName = context.Player.Zone.ToString();
           var zoneGrid = gridFactory.LoadGridOrCreateNew(mapName);
           var pathfinder = new Pathfinding {Grid = zoneGrid};
           
           AddNpcsToGrid(context, zoneGrid);
           AddInanimateObjectsToGrid(context, zoneGrid);

           List<Vector3> walkedNodes = new List<Vector3>();

            while (zoneGrid.MapName == context.Player.Zone.ToString())
           // for (int i = 0; i < 1000; i++)aaaaaa
           {
               var player = context.API.Player;
                var myPosition = RoundPositionToVector3(player.Position); 
               // var path = pathfinder.FindWaypoints(myPosition, pathfinder.Grid.UnknownNodes[0].WorldPosition);
               // var obsWaypoints = ConvertVectorArrayToObservableCollectionPosition(path);
               // context.Config.Route.Waypoints = obsWaypoints;

               // Actually walking
               // Navigator.StartRoute(context);
               //



               if (!walkedNodes.Contains(myPosition))
               {
                   walkedNodes.Add(myPosition);
                   zoneGrid.AddKnownNode(myPosition);
                   LogViewModel.Write("in map: " + zoneGrid.MapName + " Adding position: " + myPosition);
               }
           }
            
            var lastPositionBeforeZone = RoundPositionToVector3(context.API.Player.Position);
            if (lastPositionBeforeZone != Vector3.Zero)
            {
                while (context.API.Player.Zone == Zone.Unknown)
                {
                    TimeWaiter.Pause(100);
                }
                zoneGrid.AddZoneBoundary(context.Player.Zone.ToString(), lastPositionBeforeZone);
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

           // write zones

           // Discover all undiscovered nodes.

           // Console.WriteLine(zoneGrid.Print());

        }

        private static string GetMapsDirectory()
        {
            string repoRoot = Directory.GetCurrentDirectory();
            for (int i = 0; i < 3; i++)
            {
                repoRoot = Directory.GetParent(repoRoot).FullName;
            }

            repoRoot = Path.Combine(repoRoot, "Maps");
            Directory.CreateDirectory(repoRoot);
            return repoRoot;
        }

        private static void AddInanimateObjectsToGrid(IGameContext context, Grid zoneGrid)
        {
            foreach (var unit in context.Memory.UnitService.InanimateObjectsUnits)
            {
                Vector3 pos = RoundPositionToVector3(unit.Position);
                var thing = new NPC(unit.Name, pos);
                zoneGrid.AddInanimateObject(thing);
            }
        }

        private static void AddNpcsToGrid(IGameContext context, Grid zoneGrid)
        {
            foreach (var unit in context.Memory.UnitService.NpcUnits)
            {
                Vector3 pos = RoundPositionToVector3(unit.Position);
                var npc = new NPC(unit.Name, pos);
                zoneGrid.AddNpc(npc);
            }
        }

        private ObservableCollection<MemoryAPI.Navigation.Position> ConvertVectorArrayToObservableCollectionPosition(Vector3[] path)
        {
            var waypoints = new ObservableCollection<MemoryAPI.Navigation.Position>();
            for (int i = 0; i < path.Length; i++)
            {
                var pos = new MemoryAPI.Navigation.Position();
                pos.X = path[i].X;
                pos.Y = path[i].Y;
                pos.Z = path[i].Z;

                waypoints.Add(pos);
            }

            return waypoints;
        }

        public static Vector3 RoundPositionToVector3(Position position)
        {
            Vector3 gridPosition = new Vector3
            {
                X = GridMath.ConvertFromFloatToInt(position.X),
                Y = 0,
                Z = GridMath.ConvertFromFloatToInt(position.Z)
            };
            return gridPosition;
        }
    }
}