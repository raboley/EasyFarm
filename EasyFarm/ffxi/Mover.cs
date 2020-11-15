using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using EasyFarm.Context;
using EasyFarm.Pathfinding;
using EliteMMO.API;
using MemoryAPI.Memory;
using MemoryAPI.Navigation;
using Pathfinder;
using Pathfinder.Map;

namespace EasyFarm.ffxi
{
    public class Mover : Pathfinder.Travel.IWalker, INotifyPropertyChanged
    {
        public Mover(IGameContext context)
        {
            _context = context;
        }

        public Zone CurrentZone { get; set; }
        public Position PlayerPosition
        {
            get => _context.API.Player.Position;
        }

        public void WalkToPosition(Vector3 targetPosition)
        {
            var distance = Pathfinder.GridMath.GetDistancePos(CurrentPosition, targetPosition);
            RunForwardWithKeypad();
            while (distance > DistanceTolerance)
            {
                distance = Pathfinder.GridMath.GetDistancePos(CurrentPosition, targetPosition);
                Debug.WriteLine("at: " + PlayerPosition + " or (" + CurrentPosition + ") Headed to: " + targetPosition);
                Debug.WriteLine("Distance is: " + distance);
                LookAtTargetPosition(targetPosition);
                // Thread.Sleep(100);
            }
            _context.API.Navigator.Reset();
        }
        
        

        private void RunForwardWithKeypad()
        {
            _context.API.Windower.SendKeyDown(Keys.NUMPAD8);
        }

        /// <summary>
        /// The distance that the mover can be from the destination to consider
        /// itself there. A tolerance of 1 would mean 1 point away is close enough. Ex:
        /// (1, 0, 0) would be considered close enough to (0, 0, 0)
        /// </summary>
        public int DistanceTolerance { get; set; } = 1;
        private void LookAtTargetPosition(Vector3 targetPosition)
        {
            _context.API.Navigator.SetViewMode(EliteMmoWrapper.ViewMode.FirstPerson);
            _context.API.Navigator.FaceHeading(new Position(targetPosition));
        }

        public void OnWalkerIsStuck(Vector3 currentPosition)
        {
            throw new NotImplementedException();
        }

        

        public Vector3 CurrentPosition
        {
            get => GridMath.RoundVector3(PlayerPosition.To2DVector3());
            set
            {
                var pos = GridMath.RoundVector3(PlayerPosition.To2DVector3());
                if (pos.Equals(_currentPosition)) return;
                _currentPosition = value;
                PositionHistory.Enqueue(value);
                if (PositionHistory.Count >= 15) PositionHistory.Dequeue();
                OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool Zoning { get; set; }

        public event EventHandler<Vector3> IsStuck;
        public event PropertyChangedEventHandler PropertyChanged;
        public readonly Queue<Vector3> PositionHistory = new Queue<Vector3>();
        private Vector3 _currentPosition;
        private IGameContext _context;
    }
}