using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using EasyFarm.Context;
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

        public Queue<Vector3> PositionHistory { get; }

        public void WalkToPosition(Vector3 targetPosition)
        {
            var distance = Pathfinder.GridMath.GetDistancePos(CurrentPosition, targetPosition);
            int stuckCounter = 0;
            
            RunForwardWithKeypad();
            
            DateTime duration = DateTime.Now.AddSeconds(5);
            while (distance > DistanceTolerance && DateTime.Now < duration)
            {
                int newDistance = Pathfinder.GridMath.GetDistancePos(CurrentPosition, targetPosition);
                if (newDistance >= distance)
                {
                   // Might be stuck 
                   Debug.WriteLine("Might be stuck #" + stuckCounter);
                   stuckCounter++;
                   Thread.Sleep(100);
                }
                else
                {
                    stuckCounter = 0;
                }

                if (stuckCounter >= 20)
                {
                    var unWalkablePosition = GetPositionInFrontOfMe(targetPosition); 
                    BackupWhenStuck(targetPosition);
                    OnWalkerIsStuck(unWalkablePosition);
                    OnWalkerIsStuck(targetPosition);
                    Debug.WriteLine("Adding an unWalkable Position at:" + unWalkablePosition + " and: " + targetPosition);
                   return;
                }
                
                distance = newDistance; 
                
                Debug.WriteLine("at: " + PlayerPosition + " or (" + CurrentPosition + ") Headed to: " + targetPosition);
                Debug.WriteLine("Distance is: " + distance);
                LookAtTargetPosition(targetPosition);
            }
            _context.API.Navigator.Reset();
        }
        
        private Vector3 GetPositionInFrontOfMe(Vector3 targetPosition)
        {
            int x = GetNewXorY(CurrentPosition.X, targetPosition.X);
            int y = GetNewXorY(CurrentPosition.Z, targetPosition.Z);
                
            var blockedPosition = new Vector3(x, 0, y);
            return blockedPosition;
        }
        
        private int GetNewXorY(float current, float target)
        {
            int currentInt = GridMath.ConvertFromFloatToInt(current);
            int targetInt = GridMath.ConvertFromFloatToInt(target);

            if (currentInt > targetInt)
                return currentInt - 1;

            if (currentInt < targetInt)
                return currentInt + 1;

            return currentInt;
        }

        public void BackupWhenStuck(Vector3 targetPosition)
        {
            TurnAround(targetPosition);
            RunForwardForSeconds(3);
        }

        public void RunForwardForSeconds(int seconds)
        {
            DateTime duration = DateTime.Now.AddSeconds(seconds);
            RunForwardWithKeypad();
            while (DateTime.Now < duration)
            {
             Thread.Sleep(100);   
            }
            _context.API.Navigator.Reset();
        }
        
        public void TurnAround(Vector3 targetPosition)
        {
            var positionBehindMe = GetPositionBehindMe(targetPosition);
            LookAtTargetPosition(positionBehindMe);
        }
        
        private Vector3 GetPositionBehindMe(Vector3 targetPosition)
        {
            int x = GetNewXorYBehind(CurrentPosition.X, targetPosition.X);
            int y = GetNewXorYBehind(CurrentPosition.Z, targetPosition.Z);
                
            var blockedPosition = new Vector3(x, 0, y);
            return blockedPosition;
        }
        private int GetNewXorYBehind(float current, float target)
        {
            int currentInt = GridMath.ConvertFromFloatToInt(current);
            int targetInt = GridMath.ConvertFromFloatToInt(target);

            if (currentInt > targetInt)
                return currentInt + 1;

            if (currentInt < targetInt)
                return currentInt - 1;

            return currentInt;
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

        public virtual void OnWalkerIsStuck(Vector3 currentPosition)
        {
            IsStuck?.Invoke(this, currentPosition);
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
        private Vector3 _currentPosition;
        private IGameContext _context;
    }
}