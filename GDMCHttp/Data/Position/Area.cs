using System;
using System.Collections.Generic;
using System.Text;

namespace GDMCHttp.Data.Position
{
    [Serializable]
    public class Area
    {
        public Vec3Int CornerA { get; set; }
        public Vec3Int CornerB { get; set; }
        public Vec3Int OffsetAToB
        {
            get
            {
                return Vec3Int.Sub(CornerB, CornerA);
            }
        }
        public Vec3Int OffsetBToA
        {
            get
            {
                return Vec3Int.Sub(CornerA, CornerB);
            }
        }
        public Vec3Int Size
        {
            get
            {
                Vec3Int offset = OffsetAToB;
                return new Vec3Int(Math.Abs(offset.X), Math.Abs(offset.Y), Math.Abs(offset.Z));
            }
        }
        public Vec3Int CentreOffset
        {
            get
            {
                return (OffsetBToA / 2);
            }
        }
        public Vec3Int Centre
        {
            get
            {
                return CornerA + CentreOffset;
            }
        }

        public int Volume
        {
            get
            {
                Vec3Int size = Size;
                return size.X * size.Y * size.Z;
            }
        }

        public Area() : this(Vec3Int.Zero, Vec3Int.Zero)
        {

        }

        public Area(Vec3Int cornerA, Vec3Int cornerB)
        {
            CornerA = cornerA;
            CornerB = cornerB;
        }

        public bool CouldContain(Area other)
        {
            return other.Size.X <= Size.X
                && other.Size.Y <= Size.Y
                && other.Size.Z <= Size.Z;
        }

        public override string ToString()
        {
            return CornerA + "-->" + CornerB;
        }
    }
}
