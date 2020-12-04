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

using MemoryAPI;
using MemoryAPI.Navigation;
using Player = FinalFantasyXI.Classes.Player;

namespace FinalFantasyXI.States
{
    public class PlayerMovementTracker
    {
        private readonly IMemoryAPI _fface;
        // private readonly Queue<Position> _positionHistory = new Queue<Position>();

        public PlayerMovementTracker(IMemoryAPI fface)
        {
            _fface = fface;
        }

        public void RunComponent()
        {
            // // string mapName = _context.Player.Zone.ToString();
            // string mapName = _fface.Player.Zone.ToString();
            // if (mapName == "Unknown" || mapName.IsNullOrEmpty())
            //     return;
            //
            // _context.ZoneMapFactory.Persister = NewZoneMapPersister();
            // _context.Zone.Map = _context.ZoneMapFactory.LoadGridOrCreateNew(mapName);
            // _context.Zone.Name = mapName;
            // var collectionWatcher = new CollectionWatcher<Node>(_context.Zone.Map.UnknownNodes, new KnownNodeActor(_context.ZoneMapFactory.Persister, _context.Zone.Map));
            //
            // // LogViewModel.Write("Starting To record Player position in zone:" + mapName);
            // while (mapName == _context.Player.Zone.ToString())
            // {
            TrackPlayerPosition();
            //     var node = _context.Zone.Map.GetNodeFromWorldPoint(new Vector3(position.X, position.Y, position.X));
            //     if (_context.Zone.Map.UnknownNodes.Contains(node))
            //     {
            //         _context.Zone.Map.AddKnownNode(node.WorldPosition);
            //         // TODO do this async or something? It takes a long time.
            //         _context.ZoneMapFactory.Persister.Save(_context.Zone.Map);
            //     }
            // }
        }

        private void TrackPlayerPosition()
        {
            Position unUsedValue;  
            
            var position = _fface.Player.Position;
            _fface.Player.PositionHistory.Enqueue(Helpers.ToPosition(position.X, position.Y, position.Z, position.H)); 
            if (_fface.Player.PositionHistory.Count >= 15) _fface.Player.PositionHistory.TryDequeue(out unUsedValue);
            
        }
    }
}