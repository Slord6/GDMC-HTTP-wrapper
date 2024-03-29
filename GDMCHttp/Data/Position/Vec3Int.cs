﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GDMCHttp.Data.Position
{
    public class Vec3Int
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Vec3Int() : this(0,0,0) { }

        public Vec3Int(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vec3Int ToAbsolute()
        {
            return Vec3Int.AbsoluteDifference(Vec3Int.Zero, this);
        }

        public override string ToString()
        {
            return $"{X} {Y} {Z}";
        }

        public static Vec3Int Zero { get => new Vec3Int(0, 0, 0); }
        public static Vec3Int One { get => new Vec3Int(1, 1, 1); }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Vec3Int)) return false;
            Vec3Int other = (Vec3Int)obj;

            return other == this;
        }

        public static bool operator ==(Vec3Int a, Vec3Int b)
        {
            if (a is null)
            {
                return b is null;
            }
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static Vec3Int operator *(Vec3Int a, int amount)
        {
            return new Vec3Int(a.X * amount, a.Y * amount, a.Z * amount);
        }

        public static Vec3Int operator /(Vec3Int a, int amount)
        {
            return new Vec3Int(a.X / amount, a.Y / amount, a.Z / amount);
        }

        public static bool operator !=(Vec3Int a, Vec3Int b)
        {
            return !(a == b);
        }

        public static Vec3Int operator +(Vec3Int a, Vec3Int b)
        {
            return Add(a, b);
        }

        public static Vec3Int operator -(Vec3Int a, Vec3Int b)
        {
            return Sub(a, b);
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
            return new Vec3Int(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y), Math.Max(a.Z, b.Z));
        }

        /// <summary>
        /// Get the combination of the two vectors such that the minimum x,y,z between them is the value
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>Minimised Vec3Int</returns>
        public static Vec3Int MergeToMin(Vec3Int a, Vec3Int b)
        {
            return new Vec3Int(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y), Math.Min(a.Z, b.Z));
        }

        public static Vec3Int AbsoluteDifference(Vec3Int a, Vec3Int b)
        {
            Vec3Int min = MergeToMin(a, b);
            Vec3Int max = MergeToMax(a, b);

            return new Vec3Int(max.X - min.X, max.Y - min.Y, max.Z - min.Z);
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
            Vec3Int max = Max(a, b);
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
                        if (x == 0 && y == 0 && z == 0) continue;
                        Vec3Int offset = new Vec3Int(x, y, z);
                        neighbours.Add(Add(pos, offset));
                    }
                }
            }
            return neighbours.ToArray();
        }

        public static Vec3Int[] NeighboursOrthogonal(Vec3Int pos)
        {

            List<Vec3Int> neighbours = new List<Vec3Int>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        if (x == 0 && y == 0 && z == 0) continue;
                        if (x == 0 || z == 0)
                        {
                            Vec3Int offset = new Vec3Int(x, y, z);
                            neighbours.Add(Add(pos, offset));
                        }
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
            return new Vec3Int(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        /// <summary>
        /// a-b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vec3Int Sub(Vec3Int a, Vec3Int b)
        {
            return new Vec3Int(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static int TaxiCabDistance(Vec3Int a, Vec3Int b)
        {
            Vec3Int diff = Sub(b, a);
            return Math.Abs(diff.X) + Math.Abs(diff.Y) + Math.Abs(diff.Z);
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

        private static int Median(int[] values)
        {
            List<int> sorted = new List<int>(values);
            sorted.Sort();
            return sorted[sorted.Count / 2];
        }

        public static Vec3Int MedianPosition(Vec3Int[] positions)
        {
            int[] xVals = new int[positions.Length];
            int[] yVals = new int[positions.Length];
            int[] zVals = new int[positions.Length];
            for (int i = 0; i < positions.Length; i++)
            {
                Vec3Int pos = positions[i];
                xVals[i] = pos.X;
                yVals[i] = pos.Y;
                zVals[i] = pos.Z;
            }

            return new Vec3Int(Median(xVals), Median(yVals), Median(zVals));
        }

        public static Vec3Int Parse(string value)
        {
            Vec3Int position;
            if (!TryParse(value, out position))
            {
                throw new ArgumentException(value + " is not a valid Vec3Int representation");
            }
            return position;
        }
    }
}
