﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GDMCHttp.Data
{
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

        public Area(Vec3Int cornerA, Vec3Int cornerB)
        {
            CornerA = cornerA;
            CornerB = cornerB;
        }
    }
}
