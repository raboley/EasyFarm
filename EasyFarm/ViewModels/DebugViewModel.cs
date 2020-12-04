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

using EasyFarm.Infrastructure;
using EasyFarm.Services;
using FinalFantasyXI.Classes;
using FinalFantasyXI.UserSettings;

namespace EasyFarm.ViewModels
{
    public class DebugViewModel : ViewModelBase, IViewModel
    {
        public DebugViewModel()
        {
            ViewName = "Debug";
        }

        public Config ObjectToVisualize
        {
            get { return Config.Instance; }
        }

        public string Name
        {
            get { return Config.Instance.FollowedPlayer; }
            set
            {
                Config.Instance.FollowedPlayer = value;
                AppServices.InformUser("Now following {0}.", value);
            }
        }

        public double Distance
        {
            get { return Config.Instance.FollowDistance; }
            set
            {
                Set(ref Config.Instance.FollowDistance, value);
                AppServices.InformUser($"Follow Distance: {value}.");
            }
        }
    }
}
