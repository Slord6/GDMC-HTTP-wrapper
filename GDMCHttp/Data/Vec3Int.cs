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

            return other == this;
        }

        public static bool operator ==(Vec3Int a, Vec3Int b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        public static bool operator !=(Vec3Int a, Vec3Int b)
        {
            return !(a == b);
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

        /// <summary>
        /// Get the neighbouring positions of the given one
        /// </summary>
        /// <param name="pos">Center position</param>
        /// <returns>All neighbour positions of the position</returns>
        public static Vec3Int[] Neighbours(Vec3Int pos)
        {
            List<Vec3Int> neighbours = new List<Vec3Int>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        Vec3Int offset = new Vec3Int(x, y, z);
                        neighbours.Add(Vec3Int.Add(pos, offset));
                    }
                }
            }
            return neighbours.ToArray();
        }

        /// <summary>
        /// a+b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vec3Int Add(Vec3Int a, Vec3Int b)
        {
            return new Vec3Int(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        /// <summary>
        /// a-b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vec3Int Sub(Vec3Int a, Vec3Int b)
        {
            return new Vec3Int(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static bool TryParse(string value, out Vec3Int position)
        {
            position = null;
            try
            {
                value = value.Replace(",", " ");
                string[] values = value.Split(' ');
                if (values.Length < 3) return false;
                int[] intValues = new int[3];
                for (int i = 0; i < 3; i++)
                {
                    intValues[i] = int.Parse(values[i]);
                }
                position = new Vec3Int(intValues[0], intValues[1], intValues[2]);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Vec3Int Parse(string value)
        {
            Vec3Int position;
            if(!TryParse(value, out position))
            {
                throw new ArgumentException(value + " is not a valid Vec3Int representation");
            }
            return position;
        }
    }
}
