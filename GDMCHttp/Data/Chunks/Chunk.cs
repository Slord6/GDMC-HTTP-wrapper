using Cyotek.Data.Nbt;
using Cyotek.Data.Nbt.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GDMCHttp.Data.Chunks
{
    public class Chunk
    {
        private TagCompound root;
        private Vec3Int worldPosition;
        private List<Section> sections;

        public List<Section> Sections { get => sections; }
        public Vec3Int WorldPosition { get => worldPosition; }

        public Chunk(byte[] data, Vec3Int worldPosition)
        {
            this.worldPosition = worldPosition;

            BinaryTagReader reader = new BinaryTagReader(new MemoryStream(data), true);

            root = (TagCompound)reader.ReadTag();

            NbtDocument doc = new NbtDocument(root);
            TagList chunkList = doc.Query<TagList>("Chunks");

            foreach (TagCompound levelItem in chunkList.Value)
            {
                Tag temp = null;
                levelItem.Value.TryGetValue("Level", out temp);
                TagCompound level = (TagCompound)temp;

                TagDictionary levelChildren = level.Value;

                temp = null;
                levelChildren.TryGetValue("Sections", out temp);
                TagList sections = (TagList)temp;

                this.sections = new List<Section>();
                foreach (TagCompound section in sections.Value)
                {
                    this.sections.Add(new Section(section, this.worldPosition));
                }
            }
        }

    }
}
