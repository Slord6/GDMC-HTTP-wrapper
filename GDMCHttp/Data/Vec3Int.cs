using System;
using System.Collections.Generic;
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
    }
}
