﻿// ///////////////////////////////////////////////////////////////////
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

using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;

namespace EasyFarm.Handlers
{
    public class UpdateEliteAPI : EventHandlerBase<LoadedEvent>
    {
        private readonly LibraryUpdater _updater;
        private readonly IDialogCoordinator _dialogCoordinator;

        public UpdateEliteAPI(
            LibraryUpdater updater,
            IDialogCoordinator dialogCoordinator,
            EventMessenger events) : base(events)
        {
            _updater = updater;
            _dialogCoordinator = dialogCoordinator;
        }

        protected override async Task Execute()
        {
            if (_updater.HasUpdate())
            {
                var showDialogResult = await _dialogCoordinator.ShowMessageAsync(
                    Application.Current.MainWindow.DataContext,
                    "Update EliteAPI",
                    "Would you like to update EliteAPI to its newest version?",
                    MessageDialogStyle.AffirmativeAndNegative);

                if (showDialogResult == MessageDialogResult.Affirmative)
                    _updater.Update();
            }
        }
    }
}