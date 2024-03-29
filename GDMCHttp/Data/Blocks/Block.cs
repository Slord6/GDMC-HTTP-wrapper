﻿using GDMCHttp.Data.Position;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GDMCHttp.Data.Blocks
{
    public class Block
    {
        private BlockProperties properties;

        public Vec3Int Position { get => properties.Position; }
        public BlockName Name { get => properties.Name; }
        /// <summary>
        /// The fully qualified blockname, with leading "minecraft:"
        /// </summary>
        public string NamespacedName { get => $"minecraft:{Name}"; }
        public BlockProperties BlockProperties { get => properties; }

        public Block(BlockName name, Vec3Int position)
        {
            properties = new BlockProperties(name, position);
        }

        /// <summary>
        /// Create a block
        /// </summary>
        /// <param name="rawString">Raw block string</param>
        /// <param name="position"></param>
        public Block(string rawString)
        {
            properties = new BlockProperties(rawString);
        }

        public Block(BlockProperties properties)
        {
            this.properties = properties;
        }

        public override string ToString()
        {
            string fullName = NamespacedName;
            if (BlockProperties != null)
            {
                fullName += BlockProperties.ToString();
            }

            return fullName;
        }
    }
}
