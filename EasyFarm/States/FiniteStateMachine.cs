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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using EasyFarm.Classes;
using EasyFarm.Context;
using EasyFarm.Logging;
using EasyFarm.Parsing;
using EasyFarm.Pathfinding;
using EasyFarm.Soul;
using EasyFarm.UserSettings;
using EasyFarm.ViewModels;
using EliteMMO.API;
using MemoryAPI;
using MemoryAPI.Chat;
using MemoryAPI.Dialog;
using Pathfinder;
using Pathfinder.Travel;
using StatusEffect = MemoryAPI.StatusEffect;
using Zone = Pathfinder.Map.Zone;

namespace EasyFarm.States
{
    public class FiniteStateMachine
    {
        private readonly TypeCache<bool> _cache = new TypeCache<bool>();
        private readonly IMemoryAPI _fface;
        private readonly List<IState> _states = new List<IState>();
        private CancellationTokenSource _cancellation = new CancellationTokenSource();
        public readonly GameContext _context;
        private readonly ICalling _calling;

        public FiniteStateMachine(IMemoryAPI fface, GameContext gameContext)
        {
            _fface = fface;
            _context = gameContext;
            _calling = new Calling(_context);

            //Create the states
            AddState(new DeadState() {Priority = 7});
            AddState(new ZoneState() {Priority = 7});
            AddState(new SetTargetState() {Priority = 0});
            AddState(new SetFightingState() {Priority = 7});
            AddState(new FollowState() {Priority = 5});
            AddState(new RestState() {Priority = 2});
            AddState(new SummonTrustsState() {Priority = 6});
            AddState(new ApproachState() {Priority = 0});
            AddState(new ManualOverrideState() {Priority = 9999});

            // Fighting States
            AddState(new SetTargetState() {Priority = 10});
            AddState(new SetFightingState() {Priority = 10});
            AddState(new ApproachState() {Priority = 1});
            AddState(new BattleState() {Priority = 3});
            AddState(new WeaponskillState() {Priority = 2});
            AddState(new PullState() {Priority = 4});


            // AddState( new GoFight() {Priority = 8});
            // AddState( new WalkStraight() {Priority = 0});

            AddState(new DeadState() {Priority = 51});
            AddState(new ZoneState() {Priority = 51});
            AddState(new FollowState() {Priority = 5});
            AddState(new RestState() {Priority = 2});
            AddState(new SummonTrustsState() {Priority = 6});
            AddState(new StartState() {Priority = 5});
            AddState(new TravelState() {Priority = 1});
            AddState(new HealingState() {Priority = 2});
            AddState(new EndState() {Priority = 3});
            AddState(new StartEngineState() {Priority = Constants.MaxPriority});
            // AddState(new DumpTreasureState() { Priority = 2 });
            // AddState(new MapState() {Priority = 5});

            AddState(new DoMyCurrentGoalState(gameContext, _calling) {Priority = 0});

            // AddState(new ExploreZone() {Priority = 0});
            // AddState(new HuntNotoriusMonster() {Priority = 10});
            // AddState(new GoChopWood() {Priority = 10});

            // Needs Signet

            // The Finer Things
            AddState(new NeedSignet() {Priority = 21});
            // AddState(new CraftSomething() {Priority = 20});
            AddState(new SellSomeJunk() {Priority = 119});
            // AddState(new DoQuest() {Priority = 0});


            // TODO: Get Equipable By Jobs working,
            // Then ensure weapon types are taken into account
            // Then pick the highest Item level (or damage or def or something) that is available in inventory.
            // AddState(new EquipBestGear() {Priority = 118});


            // AddState(new TestState() {Priority = 999});

            // Inventory Is Full
            // Have some ingredients to craft
            // Hungry
            // Trying to Level Up

            _states.ForEach(x => x.Enabled = true);
        }


        private void AddState(IState component)
        {
            _states.Add(component);
        }

        // Start and stop.
        public void Start()
        {
            ReEnableStartState();
            RunFiniteStateMainWithThread();
        }

        private void ReEnableStartState()
        {
            var startEngineState = _states.FirstOrDefault(x => x.GetType() == typeof(StartEngineState));
            if (startEngineState != null) startEngineState.Enabled = true;
        }

        public void Stop()
        {
            _cancellation.Cancel();
        }

        private void RunFiniteStateMainWithThread()
        {
            _cancellation = new CancellationTokenSource();

            Task.Factory.StartNew(() =>
            {
                using (_cancellation.Token.Register(StopThreadQuickly(Thread.CurrentThread)))
                {
                    try
                    {
                        RunStateMachine();
                    }
                    catch (ThreadInterruptedException ex)
                    {
                        Logger.Log(new LogEntry(LoggingEventType.Information, "FSM thread interrupted", ex));
                    }
                    catch (ThreadAbortException ex)
                    {
                        Logger.Log(new LogEntry(LoggingEventType.Information, "FSM thread aborted", ex));
                    }
                    catch (OperationCanceledException ex)
                    {
                        Logger.Log(new LogEntry(LoggingEventType.Information, "FSM thread cancelled", ex));
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(new LogEntry(LoggingEventType.Error, "FSM error", ex));
                        LogViewModel.Write("An error has occurred: please check easyfarm.log for more information");
                        AppServices.InformUser("An error occurred!");
                        // I do want to write exception message for now.
                        LogViewModel.Write(ex.Message);
                    }
                    finally
                    {
                        _fface.Navigator.Reset();
                    }
                }
            }, _cancellation.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private Action StopThreadQuickly(Thread backgroundThread)
        {
            return () =>
            {
                if (!backgroundThread.Join(500)) backgroundThread.Interrupt();
            };
        }

        private void RunStateMachine()
        {
            while (true)
            {
                // Sort the List, States may have updated Priorities.
                _states.Sort();

                // watch for events that should break running.


                // Find a State that says it needs to run.
                foreach (var mc in _states.Where(x => x.Enabled).ToList())
                {
                    _cancellation.Token.ThrowIfCancellationRequested();

                    var isRunnable = mc.Check(_context);

                    // Run last state's exits method.
                    if (_cache[mc] != isRunnable)
                    {
                        if (isRunnable)
                        {
                            mc.Enter(_context);
                        }
                        else
                        {
                            mc.Exit(_context);
                        }

                        _cache[mc] = isRunnable;
                    }

                    if (isRunnable)
                    {
                        mc.Run(_context);
                        TimeWaiter.Pause(250);
                        // Need to fix up the battle stuff before adding this...
                        // Would be better if you didn't go through every state so mutual exclusion was required in each state...
                        // break;
                    }
                }

                TimeWaiter.Pause(250);
            }

            // ReSharper disable once FunctionNeverReturns
        }
    }

    public class TestState : BaseState
    {
        public override bool Check(IGameContext context)
        {
            return true;
        }

        public override void Run(IGameContext context)
        {
            while (true)
            {
                // Craft mode
                // get synt support
                // context.Craft.GetSynthesisSupport(context, StatusEffect.Woodworking_Imagery);
                // context.Craft.EatCraftSkillUpFood(context);


                // Skill is the weapon type :) 
                // EquipmentManager.EquipBestGear(context); 

                //////////////////////////////
                //Open Doors
                // if within 10 m of a door
                // and headed toward it
                //target it
                // and open it
                if (context.Traveler == null)
                    return;
                
                // TODO: Integrate into walker so it opens doors when close by.
                context.Navigator.OpenDoor(context, context.API);
            }
        }


    }

    public class EquipBestGear : BaseState
    {
        public override bool Check(IGameContext context)
        {
            return true;
        }

        public override void Run(IGameContext context)
        {
            EquipmentManager.EquipBestGear(context);
        }
    }

    public class ManualOverrideState : BaseState
    {
    }
}