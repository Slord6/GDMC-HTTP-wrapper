using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GDMCHttp.Data
{
    public class Block
    {
        private readonly Vec3Int position;
        private readonly BlockName name;

        public Vec3Int Position { get => position; }
        public BlockName Name { get => name; }
        public string NamespacedName { get => $"minecraft:{Name}"; }

        public Block(Vec3Int position, BlockName name)
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
            string baseName = blockName.Split(':')[1];


            if(!Enum.TryParse<BlockName>(baseName, out name))
            {
                name = BlockName.UNKNOWN;
                Debug.WriteLine(blockName + " is unknown ");
            }
            this.position = position;
        }
    }
}
