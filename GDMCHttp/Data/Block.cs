using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GDMCHttp.Data
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
        public BlockProperties Properties { get => properties; }

        public Block(BlockName name, Vec3Int position)
        {
            this.properties = new BlockProperties(name, position);
        }

        /// <summary>
        /// Create a block
        /// </summary>
        /// <param name="rawString">Raw block string</param>
        /// <param name="position"></param>
        public Block(string rawString)
        {
            this.properties = new BlockProperties(rawString);
        }

        public Block(BlockProperties properties)
        {
            this.properties = properties;
        }

        public override string ToString()
        {
            string fullName = NamespacedName;
            if(Properties != null)
            {
                fullName += Properties.ToString();
            }

            return properties.Position.ToString() + " " + fullName;
        }
    }
}
