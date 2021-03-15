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
        private Vec3Int chunkPosition;
        private Section[] sections;

        public Section[] Sections { get => sections; }
        public Vec3Int WorldPosition
        {
            get
            {
                return Chunk.ChunkToWorldPosition(chunkPosition);
            }
        }
        public TagCompound Root { get => root; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sections"></param>
        /// <param name="chunkPosition">Chunk position, ie normalposition/16</param>
        public Chunk(Section[] sections, Vec3Int chunkPosition)
        {
            this.sections = sections;
            this.chunkPosition = chunkPosition;
        }

        public static Vec3Int ChunkToWorldPosition(Vec3Int chunkPosition)
        {
            return new Vec3Int(chunkPosition.X * 16, 0, chunkPosition.Y * 16);
        }

        public static Chunk[] ParseToChunks(byte[] rawNbtData)
        {
            NbtDocument document = Chunk.ParseToDoc(rawNbtData);
            TagCompound[] chunkLevels = Chunk.ExtractChunkLevels(document);
            return LevelsToChunks(chunkLevels);
        }

        private static Chunk[] LevelsToChunks(TagCompound[] levels)
        {
            Chunk[] chunks = new Chunk[levels.Length];

            for (int i = 0; i < levels.Length; i++)
            {
                TagCompound level = levels[i];

                Vec3Int chunkPosition = LevelPosition(level);
                Section[] sections = LevelToSections(level, Chunk.ChunkToWorldPosition(chunkPosition));
                chunks[i] = new Chunk(sections, chunkPosition);
            }
            return chunks;
        }

        private static Vec3Int LevelPosition(TagCompound level)
        {
            int x = level.GetInt("xPos").Value;
            int z = level.GetInt("zPos").Value;
            return new Vec3Int(x, 0, z);
        }

        private static Section[] LevelToSections(TagCompound level, Vec3Int chunkPosition)
        {
            TagDictionary levelChildren = level.Value;

            Tag temp = null;
            levelChildren.TryGetValue("Sections", out temp);
            TagList sectionsTagList = (TagList)temp;

            Section[] sections = new Section[sectionsTagList.Count];
            int index = 0;
            foreach (TagCompound section in sectionsTagList.Value)
            {
                sections[index] = new Section(section, chunkPosition);
                index++;
            }
            return sections;
        }

        private static NbtDocument ParseToDoc(byte[] rawData)
        {
            BinaryTagReader reader = new BinaryTagReader(new MemoryStream(rawData), true);

            TagCompound root = (TagCompound)reader.ReadTag();

            return new NbtDocument(root);
        } 

        private static TagCompound[] ExtractChunkLevels(NbtDocument doc)
        {
            TagList chunkList = doc.Query<TagList>("Chunks");
            TagCompound[] levels = new TagCompound[chunkList.Count];

            int index = 0;
            foreach (TagCompound chunkData in chunkList.Value)
            {
                Tag temp = null;
                chunkData.Value.TryGetValue("Level", out temp);
                levels[index] = (TagCompound)temp;
                index++;
            }
            return levels;
        }

        public static Vec3Int ToChunkCoords(Vec3Int position)
        {
            return new Vec3Int((int)Math.Floor(position.X / 16.0), position.Y, (int)Math.Floor(position.Z / 16.0));

        }

    }
}
