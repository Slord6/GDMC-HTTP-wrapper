using Cyotek.Data.Nbt;
using Cyotek.Data.Nbt.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace GDMCHttp.Data.Chunks
{
    public class Chunk
    {
        private Vec3Int chunkPosition;
        private Section[] sections;
        private Dictionary<HeightmapTypes, Block[,]> heightmaps;

        public Section[] Sections { get => sections; }
        public Vec3Int WorldPosition
        {
            get
            {
                return Chunk.ChunkToWorldPosition(chunkPosition);
            }
        }
        /// <summary>
        /// Position in "chunk coordinates"
        /// </summary>
        public Vec3Int ChunkPosition { get => chunkPosition; }
        public Dictionary<HeightmapTypes, Block[,]> Heightmaps { get => heightmaps; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sections"></param>
        /// <param name="chunkPosition">Chunk position, ie normalposition/16</param>
        /// <param name="rawHeightmaps">Heightmaps, indexed by HeightmapNames value</param>
        public Chunk(Section[] sections, Vec3Int chunkPosition, Dictionary<HeightmapTypes, int[,]> rawHeightmaps)
        {
            this.sections = sections;
            this.chunkPosition = chunkPosition;
            this.rawHeightmaps = rawHeightmaps;
            heightmaps = new Dictionary<HeightmapTypes, Block[,]>();
            foreach (KeyValuePair<HeightmapTypes, int[,]> heightmapPair in rawHeightmaps)
            {
                heightmaps.Add(heightmapPair.Key, BlockHeightmap(heightmapPair.Value));
            }
        }

        public static Vec3Int ChunkToWorldPosition(Vec3Int chunkPosition)
        {
            return new Vec3Int(chunkPosition.X * 16, 0, chunkPosition.Z * 16);
        }

        public Block[,] BlockHeightmap(int[,] rawHeightmap)
        {
            int chunkSideLen = 16;
            Block[,] blocks = new Block[chunkSideLen, chunkSideLen];
            for (int x = 0; x < chunkSideLen; x++)
            {
                for (int z = 0; z < chunkSideLen; z++)
                {
                    int y = rawHeightmap[x, z];
                    Block block;
                    foreach (Section section in sections)
                    {
                        // offset from world position height to get y between 0 and 15
                        // -1 as minecraft gives position of air block above
                        int offsetY = y - section.WorldPosition.Y - 1;
                        Vec3Int pos = new Vec3Int(x, offsetY, z);
                        block = section.BlockAt(pos);
                        if (block == null) continue;

                        blocks[x, z] = block;
                        break;
                    }
                }
            }
            return blocks;
        }

        /// <summary>
        /// Parse NBT data into Chunk objects
        /// </summary>
        /// <param name="rawNbtData">NBT data</param>
        /// <returns>Chunks</returns>
        public static Chunk[] ParseToChunks(byte[] rawNbtData)
        {
            NbtDocument document = Chunk.ParseToDoc(rawNbtData);
            TagCompound[] chunkLevels = Chunk.ExtractChunkLevels(document);
            return LevelsToChunks(chunkLevels);
        }

        /// <summary>
        /// Given a Level NBT tag, parse out a Chunk
        /// </summary>
        /// <param name="levels">The Level tags</param>
        /// <returns>Chunks</returns>
        private static Chunk[] LevelsToChunks(TagCompound[] levels)
        {
            Chunk[] chunks = new Chunk[levels.Length];

            for (int i = 0; i < levels.Length; i++)
            {
                TagCompound level = levels[i];

                Vec3Int chunkPosition = LevelPosition(level);
                Section[] sections = LevelToSections(level, Chunk.ChunkToWorldPosition(chunkPosition));

                Tag heightmapTag;
                level.Value.TryGetValue("Heightmaps", out heightmapTag);
                Dictionary<HeightmapTypes, int[,]> rawHeightmaps = GetRawHeightmaps((TagCompound)heightmapTag); 
                chunks[i] = new Chunk(sections, chunkPosition, rawHeightmaps);
            }
            return chunks;
        }

        private static Dictionary<HeightmapTypes, int[,]> GetRawHeightmaps(TagCompound heightmapTag)
        {
            TagDictionary heightmaps = heightmapTag.Value;
            HeightmapTypes[] heightmapTypes = Enum.GetValues(typeof(HeightmapTypes)).Cast<HeightmapTypes>().ToArray();
            Dictionary<HeightmapTypes, int[,]> parsedHeightmaps = new Dictionary<HeightmapTypes, int[,]>();
            for (int i = 0; i < heightmapTypes.Length; i++)
            {
                Tag temp = null;
                if (!heightmaps.TryGetValue(heightmapTypes[i].ToString(), out temp))
                {
                    Debug.WriteLine($"No {heightmapTypes[i]} heightmap in chunk - {heightmapTag.ToString()}");
                    continue;
                }
                TagLongArray sectionsTagList = (TagLongArray)temp;
                int[,] heightmapIndicies = BitArray.ParseHeightMap(sectionsTagList.Value);
                parsedHeightmaps.Add(heightmapTypes[i], heightmapIndicies);
            }
            return parsedHeightmaps;
        }

        /// <summary>
        /// Extract the x and z components of a Level tag   
        /// </summary>
        /// <param name="level"></param>
        /// <returns>Vec3Int with X and Z components matching that of the Level, Y is always 0</returns>
        private static Vec3Int LevelPosition(TagCompound level)
        {
            int x = level.GetInt("xPos").Value;
            int z = level.GetInt("zPos").Value;
            return new Vec3Int(x, 0, z);
        }

        /// <summary>
        /// Parse out the Sections from a Level NBT tag
        /// </summary>
        /// <param name="level">The Level NBT tag</param>
        /// <param name="chunkWorldPosition">The position of the chunk in the world</param>
        /// <returns>Sections</returns>
        private static Section[] LevelToSections(TagCompound level, Vec3Int chunkWorldPosition)
        {
            TagDictionary levelChildren = level.Value;

            Tag temp = null;
            levelChildren.TryGetValue("Sections", out temp);
            TagList sectionsTagList = (TagList)temp;

            Section[] sections = new Section[sectionsTagList.Count];
            int index = 0;
            foreach (TagCompound section in sectionsTagList.Value)
            {
                sections[index] = new Section(section, chunkWorldPosition);
                index++;
            }
            return sections;
        }

        /// <summary>
        /// Read raw bytes into an NbtDocument
        /// </summary>
        /// <param name="rawData">The raw NBT bytes</param>
        /// <returns>An NbtDocument</returns>
        private static NbtDocument ParseToDoc(byte[] rawData)
        {
            BinaryTagReader reader = new BinaryTagReader(new MemoryStream(rawData), true);

            TagCompound root = (TagCompound)reader.ReadTag();

            return new NbtDocument(root);
        } 

        /// <summary>
        /// Parse out Levels from an NbtDocument
        /// </summary>
        /// <param name="doc">The document to parse</param>
        /// <returns>Level NBT tags</returns>
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

        /// <summary>
        /// Given a world position translate into a chunk position
        /// </summary>
        /// <param name="position">Position in the world</param>
        /// <returns>Equivalent chunk position</returns>
        public static Vec3Int ToChunkCoords(Vec3Int position)
        {
            return new Vec3Int((int)Math.Floor(position.X / 16.0), position.Y, (int)Math.Floor(position.Z / 16.0));

        }

    }
}
