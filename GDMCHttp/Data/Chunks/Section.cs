using Cyotek.Data.Nbt;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GDMCHttp.Data.Chunks
{
    public class Section
    {
        private TagCompound rawData;
        private int y = -1;
        private string[] palette = null;
        private long[] blockStates = null;
        private byte[] skyLight = null;
        private int[,,] paletteIndecies = null;
        private Vec3Int worldPosition;

        public int Y { get => y; }
        public string[] Palette { get => palette.ToArray(); }
        public long[] BlockStates { get => blockStates.ToArray(); }
        public byte[] SkyLight { get => skyLight.ToArray(); }
        public int[,,] PaletteIndecies { get => paletteIndecies; }
        public Vec3Int WorldPosition { get => worldPosition; }

        /// <summary>
        /// Create a new chunk Section
        /// </summary>
        /// <param name="sectionData">NBT tag to parse</param>
        /// <param name="worldPosition">Position of the section in the world. Y component is discarded and replaced with parsed value</param>
        public Section(TagCompound sectionData, Vec3Int worldPosition)
        {
            rawData = sectionData;

            TagDictionary actualRoot = rawData.Value;

            Tag temp = null;
            temp = null;
            if (actualRoot.TryGetValue("Y", out temp)) ParseY((TagByte)temp, worldPosition);
            
            this.worldPosition = new Vec3Int(worldPosition.X, y * 16, worldPosition.Z);

            temp = null;
            if (actualRoot.TryGetValue("Palette", out temp)) ParsePalette((TagList)temp);

            temp = null;
            if (actualRoot.TryGetValue("BlockStates", out temp)) ParseBlockStates((TagLongArray)temp);

            temp = null;
            if (actualRoot.TryGetValue("SkyLight", out temp)) ParseSkyLight((TagByteArray)temp);

        }

        private void ParseY(TagByte Y, Vec3Int worldPosition)
        {
            y = Y.Value;
        }

        private void ParsePalette(TagList rawPalette)
        {
            palette = new string[rawPalette.Value.Count];
            int index = 0;
            foreach (TagCompound paletteItem in rawPalette.Value)
            {
                palette[index] = paletteItem.GetString("Name").Value;
                index++;
            }
        }

        private void ParseBlockStates(TagLongArray rawBlockStates)
        {
            blockStates = rawBlockStates.Value;
            int bitsPerBlock = Math.Max(4, (int)Math.Ceiling(Math.Log(palette.Length, 2)));

            paletteIndecies = BitArray.ParseBlockStates(blockStates, bitsPerBlock);
        }

        private void ParseSkyLight(TagByteArray rawSkyLight)
        {
            skyLight = rawSkyLight.Value;
        }

        public bool isEmpty()
        {
            return palette == null || blockStates == null;
        }

        /// <summary>
        /// The block name at the given local position
        /// </summary>
        /// <param name="position">Position - x,y,z must be between 0->15</param>
        /// <returns>The block name</returns>
        public Block BlockAt(Vec3Int position)
        {
            if (paletteIndecies == null) return null;
            if (paletteIndecies.GetLength(0) <= position.Y ||
                paletteIndecies.GetLength(1) <= position.Z ||
                paletteIndecies.GetLength(2) <= position.X) return null;

            int paletteIndex = paletteIndecies[position.Y, position.Z, position.X];
            if (paletteIndex >= palette.Length)
            {
                return null;
            }
            Vec3Int pos = new Vec3Int(worldPosition.X + position.X, worldPosition.Y + position.Y, worldPosition.Z + position.Z);
            return new Block(palette[paletteIndex], pos);
        }

        public List<Block> AllBlocks()
        {
            List<Block> blocks = new List<Block>();
            if (isEmpty()) return blocks;

            // TODO change this to run through the palette and get the position from that
            // Allows no block == null check
            for (int y = 0; y < 16; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        Block block = BlockAt(new Vec3Int(x, y, z));
                        if(block == null)
                        {
                            //Debug.WriteLine($"No block at {pos} (section offset {new Vec3Int(x, y, z)}). Section y is {Y}, world pos {worldPosition}");
                            continue;
                        }
                        blocks.Add(block);

                    }
                }
            }
            return blocks;
        }
    }
}
