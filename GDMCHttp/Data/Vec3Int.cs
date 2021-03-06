using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GDMCHttp.Data
{
    public class Vec3Int
    {
        private int x;
        private int y;
        private int z;

        public int Y { get => y; }
        public int X { get => x; }
        public int Z { get => z; }

        public Vec3Int(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString()
        {
            return $"{x} {y} {z}";
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Vec3Int)) return false;
            Vec3Int other = (Vec3Int)obj;

            return x == other.x && y == other.y && z == other.z;
        }

        public override int GetHashCode()
        {
            // https://stackoverflow.com/questions/3404715/c-sharp-hashcode-for-array-of-ints
            int hc = 3;
            hc = unchecked(hc * 17 + X);
            hc = unchecked(hc * 17 + Y);
            hc = unchecked(hc * 17 + Z);
            return hc;
        }

        /// <summary>
        /// Get the combination of the two vectors such that the maximum x,y,z between them is the value
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>Maximised Vec3Int</returns>
        public static Vec3Int MergeToMax(Vec3Int a, Vec3Int b)
        {
            return new Vec3Int(Math.Max(a.x, b.x), Math.Max(a.y, b.y), Math.Max(a.z, b.z));
        }

        /// <summary>
        /// Get the combination of the two vectors such that the minimum x,y,z between them is the value
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>Minimised Vec3Int</returns>
        public static Vec3Int MergeToMin(Vec3Int a, Vec3Int b)
        {
            return new Vec3Int(Math.Min(a.x, b.x), Math.Min(a.y, b.y), Math.Min(a.z, b.z));
        }

        public static Vec3Int AbsoluteDifference(Vec3Int a, Vec3Int b)
        {
            Vec3Int min = MergeToMin(a, b);
            Vec3Int max = MergeToMax(a, b);

            return new Vec3Int(max.x - min.x, max.y - min.y, max.z - min.z);
        }

        public static Vec3Int Max(Vec3Int a, Vec3Int b)
        {
            int totalA = a.X + a.Y + a.Z;
            int totalB = b.X + b.Y + b.Z;

            if (totalA > totalB) return a;
            return b;
        }

        public static Vec3Int Min(Vec3Int a, Vec3Int b)
        {
            Vec3Int max = Vec3Int.Max(a, b);
            if (a == max) return b;
            return a;
        }

        public static Vec3Int Offset(Vec3Int root, Vec3Int offset)
        {
            return new Vec3Int(root.x + offset.x, root.y + offset.y, root.z + offset.z);
        }
    }
}
