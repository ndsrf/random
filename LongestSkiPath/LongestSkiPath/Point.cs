using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LongestSkiPath
{
    internal class Point: IComparable<Point>
    {
        protected bool Equals(Point other)
        {
            return X == other.X && Y == other.Y && Height == other.Height;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = X;
                hashCode = (hashCode*397) ^ Y;
                hashCode = (hashCode*397) ^ Height;
                return hashCode;
            }
        }

        public int X { get; private set; }
        public int Y { get; private set; }
        public int Height { get; private set; }

        public Point(int x, int y, int height)
        {
            X = x;
            Y = y;
            Height = height;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Point) obj);
        }

        public static bool operator >(Point p1, Point p2)
        {
            return p1.Height > p2.Height;
        }

        public static bool operator <(Point p1, Point p2)
        {
            return p1.Height < p2.Height;
        }

        public int CompareTo(Point other)
        {
            if (X == other.X && Y == other.Y && Height == other.Height)
                return 0;
            else
                return 1;
        }
    }
}
