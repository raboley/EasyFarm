using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using MemoryAPI.Navigation;
using Pathfinder;

namespace EasyFarm
{
    public class ConvertPosition
    {
        public static Vector3 RoundPositionToVector3(Position playerPosition)
        {
            var x = GridMath.ConvertFromFloatToInt(playerPosition.X);
            var z = GridMath.ConvertFromFloatToInt(playerPosition.Z);
            
            return new Vector3(x, 0, z); 
            // return new Vector3(playerPosition.X, playerPosition.Y, playerPosition.Z); 
        }
        
        public static ObservableCollection<MemoryAPI.Navigation.Position> ConvertVectorArrayToObservableCollectionPosition(
            IList<Vector3> path)
        {
            var waypoints = new ObservableCollection<MemoryAPI.Navigation.Position>();
            for (int i = 0; i < path.Count; i++)
            {
                var pos = new MemoryAPI.Navigation.Position();
                pos.X = path[i].X;
                pos.Y = path[i].Y;
                pos.Z = path[i].Z;

                waypoints.Add(pos);
            }

            return waypoints;
        }
    }
}