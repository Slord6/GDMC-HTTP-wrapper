using GDMCHttp.Data.Chunks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GDMCHttp.Data
{
    public class Block
    {
        private readonly Vec3Int position;
        private BlockName name;
        private BlockProperties properties;

        public Vec3Int Position { get => position; }
        public BlockName Name { get => name; }
        public string NamespacedName { get => $"minecraft:{Name}"; }
        public BlockProperties Properties { get => properties; }


        public Block(BlockName name, Vec3Int position)
        {
            this.position = position;
            this.name = name;
        }

        /// <summary>
        /// Create a block
        /// </summary>
        /// <param name="blockName">Namespaced blockname</param>
        /// <param name="position"></param>
        public Block(string blockName, Vec3Int position)
        {
            
            this.position = position;
        }

        public Block(BlockProperties properties, Vec3Int position)
        {
            SetName(properties.Name);
            this.position = position;
            this.properties = properties;
        }

        /// <summary>
        /// Set the name of this block
        /// </summary>
        /// <param name="fullName">Full block name eg. minecraft:oak_planks</param>
        private void SetName(string fullName)
        {
            string baseName = fullName.Split(':')[1];

            if (!Enum.TryParse<BlockName>(baseName, out name))
            {
                name = BlockName.UNKNOWN;
                Debug.WriteLine(fullName + " is unknown ");
            }
        }
    }
}
