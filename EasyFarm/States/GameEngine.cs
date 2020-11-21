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
using EasyFarm.Classes;
using EasyFarm.Context;
using EasyFarm.Monitors;
using EasyFarm.UserSettings;
using MemoryAPI;

namespace EasyFarm.States
{
    /// <summary>
    ///     Controls whether or not the bot should run. Basically anything that can pause or resume the
    ///     bot's engine should be here.
    /// </summary>
    public class GameEngine
    {
        /// <summary>
        ///     Provides information about game data.
        /// </summary>
        private readonly IMemoryAPI _fface;

        private readonly PlayerMonitor _playerMonitor;
        private readonly ZoneMapMonitor _zoneMapMonitor;

        /// <summary>
        ///     The engine that controls player actions.
        /// </summary>
        private readonly FiniteStateMachine _stateMachine;

        /// <summary>
        ///     Tells us whether the bot is working or not.
        /// </summary>
        public bool IsWorking;

        private NpcMonitor _npcMonitor;
        private MobMonitor _mobMonitor;
        private TravelerMonitor _travelerMonitor;

        public GameEngine(IMemoryAPI fface)
        {
            _fface = fface;
            var context = new GameContext(fface);
            
            _stateMachine = new FiniteStateMachine(fface, context);
            _playerMonitor = new PlayerMonitor(fface);
            _npcMonitor = new NpcMonitor(fface, context);
            _mobMonitor = new MobMonitor(fface, context);
            _travelerMonitor = new TravelerMonitor(fface, context);
            _zoneMapMonitor = new ZoneMapMonitor(fface, context);
        }

        /// <summary>
        ///     Start the bot up
        /// </summary>
        public bool Start()
        {
            var route = Config.Instance.Route;
            var isPathReachable = !route.IsPathSet || route.IsPathUnreachable(_fface);

            if (isPathReachable)
            {
                IsWorking = true;
                _stateMachine.Start();
                _playerMonitor.Start();
                _npcMonitor.Start();
                _mobMonitor.Start();
                _travelerMonitor.Start();
                _zoneMapMonitor.Start();
                return true;
            }

            AppServices.InformUser("The route is not reachable");
            return false;
        }

        /// <summary>
        ///     Stop the bot from going any further
        /// </summary>
        public void Stop()
        {
            IsWorking = false;
            _stateMachine.Stop();
            _playerMonitor.Stop();
            _npcMonitor.Stop();
            _mobMonitor.Stop();
            _travelerMonitor.Stop();
            _zoneMapMonitor.Stop();
        }
    }
}