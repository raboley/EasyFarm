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
using System.Threading;
using System.Threading.Tasks;
using EasyFarm.Classes;
using EasyFarm.Context;
using EasyFarm.Logging;
using EasyFarm.Parsing;
using EasyFarm.UserSettings;
using EasyFarm.ViewModels;
using EliteMMO.API;
using MemoryAPI;
using Pathfinder;
using Pathfinder.People;
using Pathfinder.Travel;
using Zone = Pathfinder.Map.Zone;

namespace EasyFarm.States
{
    public class FiniteStateMachine
    {
        private readonly TypeCache<bool> _cache = new TypeCache<bool>();
        private readonly IMemoryAPI _fface;
        private readonly List<IState> _states = new List<IState>();
        private CancellationTokenSource _cancellation = new CancellationTokenSource();
        private readonly GameContext _context;

        public FiniteStateMachine(IMemoryAPI fface, GameContext gameContext)
        {
            _fface = fface;
            _context = gameContext;
            //Create the states
            
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


            // AddState(new ExploreZone() {Priority = 0});
            AddState(new HuntNotoriusMonster() {Priority = 10});

            // Needs Signet
            AddState(new NeedSignet() {Priority = 21});

            // The Finer Things
            // TODO: Uncomment this
            // AddState(new GoChopWood() {Priority = 10});
            AddState(new CraftSomething() {Priority = 20});

            AddState(new SellSomeJunk() {Priority = 119});


            // AddState(new TestMoveState() { Priority = 10 });

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

    public class HuntNotoriusMonster : BaseState
    {
        private string thingToHunt;

        public override bool Check(IGameContext context)
        {
            if (context.Traveler?.CurrentZone?.Map == null)
                return false;
            
            if (new RestState().Check(context)) return false;

            if (new NeedSignet().Check(context)) return false;
            
            if (context.Inventory.InventoryIsFull())
                return false;
            
            if (context.Player.IsDead) return false;

            if (context.Player.HasAggro) return false;
            
            if (context.Target.IsValid) return false;

            return true;
        }


        public override void Run(IGameContext context)
        {
            context.WoodChopper.ChopWoodZone = "Ronfaure_West"; 
            LogViewModel.Write("Going to Hunt Notorious Monster in Zone: " + context.WoodChopper.ChopWoodZone);
            context.WoodChopper.GoToChopWoodZone(context);

            if (context.API.Player.Zone.ToString() != context.WoodChopper.ChopWoodZone)
                return;

            if (context.Traveler.Zoning)
                return;

            if (context.Traveler.CurrentZone.Map.MapName != context.WoodChopper.ChopWoodZone)
                return;

            // Add logging points to queue if empty
            if (context.WoodChopper.LoggingPoints.Count == 0)
            {
                // First add things close by
                thingToHunt = "Hare";
                var closeLoggingPoints =
                    context.Memory.UnitService.MobArray.ToList().FindAll(x => x.Name.Contains(thingToHunt));

                foreach (var loggingPoint in closeLoggingPoints)
                {
                    var tree = new Person(loggingPoint.Id, loggingPoint.Name, GridMath.RoundVector3(loggingPoint.Position.To2DVector3()));
                    context.WoodChopper.LoggingPoints.Enqueue(tree);
                }
                
                // Then all the ones that have ever been known
                var loggingPoints = context.Mobs.ToList().FindAll(x => x.Name.Contains(thingToHunt));
                foreach (var loggingPoint in loggingPoints)
                {
                    context.WoodChopper.LoggingPoints.Enqueue(loggingPoint);
                }
            }

            if (context.WoodChopper.LoggingPoints.Count == 0)
                return;
            
            if (context.WoodChopper.NextPoint == null)
                context.WoodChopper.NextPoint = context.WoodChopper.LoggingPoints.Dequeue();
            
            context.Traveler.PathfindAndWalkToFarAwayWorldMapPosition(context.WoodChopper.NextPoint.Position);
            
            var distanceToLoggingPoint = GridMath.GetDistancePos(context.Traveler.Walker.CurrentPosition,
                context.WoodChopper.NextPoint.Position);
            if (distanceToLoggingPoint > 1)
                return;
            
            context.WoodChopper.NextPoint = null;
            
        }
    }

    public class ManualOverrideState : BaseState
    {
    }
}