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
using System.Numerics;

namespace MemoryAPI.Navigation
{
    public class Position
    {
        public Position(Vector3 position)
        {
            X = position.X;
            Y = position.Y;
            Z = position.Z;
        }

        public Position()
        {
            
        }

        public float H { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public Vector3 ToVector3()
        {
            return new Vector3(X, Y, Z);
            
        }
        
        public Vector3 To2DVector3()
        {
            return new Vector3(X, 0, Z);
        }

        public override string ToString()
        {
            return "X: " + X + "Z: " + Z;
        }

        public override int GetHashCode()
        {
            return (X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode() ^ H.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            var other = obj as Position;
            if (other == null) return false;

            var deviation = Math.Abs(this.X - other.X) + 
                Math.Abs(this.Y - other.Y) + 
                Math.Abs(this.Z - other.Z) + 
                Math.Abs(this.H - other.H);

            return Math.Abs(deviation) <= 0;
        }
    }
}