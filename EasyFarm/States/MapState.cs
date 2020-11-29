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

using EasyFarm.Context;

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
            ///// Setup the ALL person watcher
            // var allNpcPersister = new Pathfinder.Persistence.FilePersister();
            // allNpcPersister.FilePath = GetAndCreateDirectorFromRoot("NPCs");
            // allNpcPersister.FileName = "all";
            //
            // var allPersonActor = new PersonActor();
            // allPersonActor.Persister = allNpcPersister;
            //
            // //Watch the NPC collection of the Zone
            // var allPeopleWatcher = new CollectionWatcher<Person>(zone.Npcs, allPersonActor);

            // On Change Save the zone's NPCs to a file for that zone's NPCs
            
            /*
             *
             *
             * result = Unit
             * logging point
 ClaimedId = {int} 0
 Distance = {double} 3.56211256980896
 HasAggroed = {bool} false
 HppCurrent = {short} 100
 Id = {int} 619
 IsActive = {bool} true
 IsClaimed = {bool} false
 IsDead = {bool} false
 IsPet = {bool} false
 IsRendered = {bool} true
 IsValid = {bool} false
 MyClaim = {bool} false
 Name = {string} "Logging Point"
 NpcType = {NpcType} NPC
 PartyClaim = {bool} false
 PosX = {float} 302.062012
 PosY = {float} -49.9350014
 PosZ = {float} 197.003006
 Position = {Position} X: 302.062Z: 197.003
 Status = {Status} Standing
 YDifference = {double} 0.14312362670898438
             */
            
          

            // Also Save to the ALL NPC file
        }

        

        // public override void Run(IGameContext context)
        // {
        //     LogViewModel.Write("Entered Mapping state");
        //     IMemoryAPI fface = context.API;
        //     context.Memory.EliteApi.Navigator.DistanceTolerance = 3;
        //     var gridFactory = new ZoneMapFactory();
        //     var persister = new FilePersister();
        //     var mapsDirectory = GetMapsDirectory();
        //
        //
        //     persister.DefaultExtension = "json";
        //     persister.FilePath = mapsDirectory;
        //     gridFactory.Persister = persister;
        //
        //     gridFactory.DefaultGridSize = new Vector2(600f, 600f);
        //
        //     string mapName = context.Player.Zone.ToString();
        //     var zoneGrid = gridFactory.LoadGridOrCreateNew(mapName);
        //
        //
        //     var test = zoneGrid.NpcList.Find(n => n.Name.Contains("I.M."));
        //
        //     AddNpcsToGrid(context, zoneGrid);
        //     AddInanimateObjectsToGrid(context, zoneGrid);
        //
        //     List<Vector3> walkedNodes = new List<Vector3>();
        //
        //     while (zoneGrid.MapName == context.Player.Zone.ToString())
        //         // for (int i = 0; i < 1000; i++)
        //     {
        //         var player = context.API.Player;
        //         var myPosition = RoundPositionToVector3(player.Position);
        //
        //         // Go get signet
        //         if (context.Player.CurrentGoal == "Signet")
        //         {
        //             // Find Signet Guy
        //             if (zoneGrid.NpcList.Count == 0)
        //             {
        //                 LogViewModel.Write("I need signet, but don't know where any NPCs are in this zone");
        //                 break;
        //             }
        //
        //             var signetPerson = zoneGrid.NpcList.Find(n => n.Name.Contains("I.M."));
        //
        //             if (signetPerson == null)
        //             {
        //                 LogViewModel.Write("I need signet, but can't find the signet person in this zone");
        //                 break;
        //             }
        //
        //             LogViewModel.Write("Found signet person:" + signetPerson.Name + " At position: " +
        //                                signetPerson.Position + " Headed there now");
        //             // Set route to the path to the signet guy
        //             var path = Pathfinder.Pathing.Pathfinding.FindWaypoints(zoneGrid, myPosition, signetPerson.Position);
        //             var obsWaypoints = ConvertVectorArrayToObservableCollectionPosition(path);
        //             context.Config.Route.Waypoints = obsWaypoints;
        //
        //             //// Actually walking
        //             Navigator.StartRoute(context);
        //
        //             // Got to the signet person
        //             LogViewModel.Write("Made it to signet person!");
        //             Config.Instance.Route.Reset();
        //             context.API.Navigator.Reset();
        //
        //             // Get signet
        //
        //
        //             // Got Signet, reset goal
        //             context.Player.CurrentGoal = "Aimless";
        //         }
        //
        //
        //         if (!walkedNodes.Contains(myPosition))
        //         {
        //             walkedNodes.Add(myPosition);
        //             zoneGrid.AddKnownNode(myPosition);
        //             LogViewModel.Write("in map: " + zoneGrid.MapName + " Adding position: " + myPosition);
        //         }
        //     }
        //
        //     var lastPositionBeforeZone = RoundPositionToVector3(context.API.Player.Position);
        //     if (lastPositionBeforeZone != Vector3.Zero)
        //     {
        //         while (context.API.Player.Zone == Zone.Unknown)
        //         {
        //             TimeWaiter.Pause(100);
        //         }
        //
        //         zoneGrid.AddZoneBoundary(context.Player.Zone.ToString(), lastPositionBeforeZone);
        //     }
        //
        //
        //     // // Map the NPCs
        //     // foreach (var unit in context.Units)
        //     // {
        //     //     Vector3 position = new Vector3(unit.PosX, unit.PosY, unit.PosZ);
        //     //     var npc = new NPC(unit.Name, position);
        //     //     npc.ID = unit.Id;
        //     //
        //     //      if (unit.NpcType == NpcType.NPC) 
        //     //          zoneGrid.AddNpc(npc);
        //     //
        //     //      // Map the objects
        //     //      if (unit.NpcType == NpcType.InanimateObject)
        //     //          zoneGrid.AddInanimateObject(npc);
        //     //
        //     //
        //     //      // Map the Mobs
        //     //
        //     // }
        //
        //
        //     gridFactory.Persister.Save(zoneGrid);
        //
        //     // write zones
        //
        //     // Discover all undiscovered nodes.
        //
        //     // Console.WriteLine(zoneGrid.Print());
        // }


 
    }
}