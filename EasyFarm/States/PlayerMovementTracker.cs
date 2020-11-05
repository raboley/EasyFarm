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
using System.IO;
using System.Linq;
using System.Numerics;
using EasyFarm.Context;
using EasyFarm.ViewModels;
using MemoryAPI;
using MemoryAPI.Navigation;
using Pathfinder;
using Pathfinder.Map;
using Pathfinder.People;
using Pathfinder.Persistence;
using Player = EasyFarm.Classes.Player;

namespace EasyFarm.States
{
    public class PlayerMovementTracker
    {
        private readonly IMemoryAPI _fface;
        private readonly Queue<Position> _positionHistory = new Queue<Position>();
        private GameContext _context;

        public PlayerMovementTracker(IMemoryAPI fface)
        {
            _fface = fface;
            _context = new GameContext(fface);
        }

        public void RunComponent()
        {
            string mapName = _context.Player.Zone.ToString();
            if (mapName == "Unknown")
                return;

            _context.ZoneMapFactory.Persister = NewZoneMapPersister();
            _context.Zone.Map = _context.ZoneMapFactory.LoadGridOrCreateNew(mapName);
            var collectionWatcher = new CollectionWatcher<Node>(_context.Zone.Map.UnknownNodes, new PersisterActor {Persister = _context.ZoneMapFactory.Persister});
            
            LogViewModel.Write("Starting To record Player position in zone:" + mapName);
            while (mapName == _context.Player.Zone.ToString())
            {
                var position = TrackPlayerPosition();
                var node = _context.Zone.Map.GetNodeFromWorldPoint(new Vector3(position.X, position.Y, position.X));
                if (_context.Zone.Map.UnknownNodes.Contains(node))
                    _context.Zone.Map.AddKnownNode(node.WorldPosition);
            }
        }

        private Position TrackPlayerPosition()
        {
            var position = _fface.Player.Position;
            _positionHistory.Enqueue(Helpers.ToPosition(position.X, position.Y, position.Z, position.H));
            if (_positionHistory.Count >= 15) _positionHistory.Dequeue();
            Player.Instance.IsMoving = IsMoving();
            return position;
        }

        public bool IsMoving()
        {
            var changeInX = _positionHistory.Average(positon => positon.X) - _fface.Player.PosX;
            var changeInZ = _positionHistory.Average(position => position.Z) - _fface.Player.PosZ;
            return Math.Abs(changeInX) + Math.Abs(changeInZ) > 0;
        }
        
        
        public static FilePersister NewZoneMapPersister()
        {
            var persister = new FilePersister();
            var mapsDirectory = GetMapsDirectory();
            persister.DefaultExtension = "json";
            persister.FilePath = mapsDirectory;
            return persister;
        }

        public static string GetMapsDirectory()
        {
            var repoRoot = GetRepoRoot();

            repoRoot = Path.Combine(repoRoot, "Maps");
            Directory.CreateDirectory(repoRoot);
            return repoRoot;
        }

        public static string GetAndCreateDirectorFromRoot(string directory)
        {
            var repoRoot = GetRepoRoot();

            repoRoot = Path.Combine(repoRoot, directory);
            Directory.CreateDirectory(repoRoot);
            return repoRoot;
        }

        public static string GetRepoRoot()
        {
            string repoRoot = Directory.GetCurrentDirectory();
            for (int i = 0; i < 3; i++)
            {
                repoRoot = Directory.GetParent(repoRoot).FullName;
            }

            return repoRoot;
        }
    }
}