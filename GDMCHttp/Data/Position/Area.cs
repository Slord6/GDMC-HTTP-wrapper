using System;
using System.Collections.Generic;
using System.Text;

namespace GDMCHttp.Data.Position
{
    [Serializable]
    public class Area
    {
        public Vec3Int MinCorner { get; set; }
        public Vec3Int MaxCorner { get; set; }
        public Vec3Int OffsetMinToMax
        {
            get
            {
                return MaxCorner - MinCorner;
            }
        }
        public Vec3Int OffsetMaxToMin
        {
            get
            {
                return MinCorner - MaxCorner;
            }
        }
        public Vec3Int Size
        {
            get
            {
                Vec3Int offset = OffsetMinToMax;
                // offset doesn't include the min corner, so we add one to all for the size
                return new Vec3Int(
                    Math.Abs(offset.X) + 1,
                    Math.Abs(offset.Y) + 1,
                    Math.Abs(offset.Z) + 1
                );
            }
        }
        public Vec3Int CentreOffset
        {
            get
            {
                return (OffsetMinToMax / 2);
            }
        }
        public Vec3Int Centre
        {
            get
            {
                return MinCorner + CentreOffset;
            }
        }
        public int Volume
        {
            get
            {
                Vec3Int size = Size;
                return FootprintSize * size.Y;
            }
        }
        public int FootprintSize
        {
            get
            {
                Vec3Int size = Size;
                return size.X * size.Z;
            }
        }

        public Area() : this(Vec3Int.Zero, Vec3Int.Zero)
        {

        }

        public Area(Vec3Int cornerA, Vec3Int cornerB)
        {
            MinCorner = Vec3Int.MergeToMin(cornerA, cornerB);
            MaxCorner = Vec3Int.MergeToMax(cornerA, cornerB);
        }

        public bool Contains(Vec3Int point)
        {
            return MinCorner.X <= point.X && point.X <= MaxCorner.X
                && MinCorner.Y <= point.Y && point.Y <= MaxCorner.Y
                && MinCorner.Z <= point.Z && point.Z <= MaxCorner.Z;
        }

        public bool Contains(Area other)
        {
            return Contains(other.MinCorner) && Contains(other.MaxCorner);
        }

        public bool CouldContain(Area other)
        {
            return other.Size.X <= Size.X
                && other.Size.Y <= Size.Y
                && other.Size.Z <= Size.Z;
        }

        public override string ToString()
        {
            return MinCorner + "-->" + MaxCorner;
        }
    }
}
