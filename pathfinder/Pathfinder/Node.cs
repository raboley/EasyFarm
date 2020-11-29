using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using EasyFarm.Pathfinding;

namespace EasyFarm.Pathfinding
{
    public class Node : IHeapItem<Node>
    {
        public bool walkable;
        public Vector3 worldPosition;
        public int gridX;
        public int gridY;

        public int gCost;
        public int hCost;
        public Node parent;
        int heapIndex;

        public Node(Vector3 _worldPos, bool _walkable)
        {
            walkable = _walkable;
            worldPosition = _worldPos;
            gridX = MappingConversion.ConvertFromFloatToInt(_worldPos.X);
            gridY = MappingConversion.ConvertFromFloatToInt(_worldPos.Z);

        }

        public int fCost
        {
            get { return gCost + hCost; }
        }

        public int HeapIndex
        {
            get { return heapIndex; }
            set { heapIndex = value; }
        }

        public int CompareTo(Node nodeToCompare)
        {
            int compare = fCost.CompareTo(nodeToCompare.fCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(nodeToCompare.hCost);
            }

            return -compare;
        }
    }
}
