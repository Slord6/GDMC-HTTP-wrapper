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

            paletteIndecies = ReadBlockStates(blockStates, bitsPerBlock);
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
        /// Obtain a list of BlockState indices from the BlockState long array.
        /// </summary>
        /// <param name="longArray">The blockstate array</param>
        /// <param name="bitsPerBlock">The number of bits in each compressed block</param>
        /// <returns>Array of palette indicied in a 16x16x16 array</returns>
        private int[,,] ReadBlockStates(long[] longArray, int bitsPerBlock)
        {
            /// Based on code from https://github.com/nilsgawlik/gdmc_http_client_python/blob/375b1839160a69466685b765c3a187607d5b3ea2/bitarray.py#L18
            /// MIT License
            /// Copyright (c) 2020-2021 Nils Gawlik
            /// Copyright (c) 2021 Blinkenlights
            /// 
            /// Permission is hereby granted, free of charge, to any person obtaining a copy
            /// of this software and associated documentation files (the "Software"), to deal
            /// in the Software without restriction, including without limitation the rights
            /// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
            /// copies of the Software, and to permit persons to whom the Software is
            /// furnished to do so, subject to the following conditions:
            /// 
            /// The above copyright notice and this permission notice shall be included in all
            /// copies or substantial portions of the Software.
            /// 
            /// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
            /// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
            /// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
            /// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
            /// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
            /// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
            /// SOFTWARE.
            
            int arraySize = 16 * 16 * 16;
            int maxEntryValue = (1 << bitsPerBlock) - 1;
            int entriesPerLong = (int)Math.Floor(64 / (double)bitsPerBlock);
            int calculatedLength = (int)Math.Floor((double)(arraySize + entriesPerLong - 1) / entriesPerLong);
            
            if (longArray.Length != calculatedLength)
            {
                throw new ArgumentException($"Invalid length given for storage, got {longArray.Length} but expected {calculatedLength}");
            }

            int[,,] indicies = new int[16, 16, 16];
            for (int x = 0; x < 16; x++)
            {
                for (int y = 0; y < 16; y++)
                {
                    for (int z = 0; z < 16; z++)
                    {
                        int index1D = y * 16 * 16 + z * 16 + x;
                        int indexOfLong = (int)(index1D / entriesPerLong);
                        long currentLong = longArray[indexOfLong];
                        int k = (index1D - indexOfLong * entriesPerLong) * bitsPerBlock;
                        indicies[y, z, x] = (int)(currentLong >> k & maxEntryValue);
                    }
                }
            }

            return indicies;
        }

        /// <summary>
        /// The block name at the given local position
        /// </summary>
        /// <param name="position">Position - x,y,z must be between 0->15</param>
        /// <returns>The block name</returns>
        public string BlockAt(Vec3Int position)
        {
            int paletteIndex = paletteIndecies[position.Y, position.Z, position.X];
            if(paletteIndex >= palette.Length)
            {
                return null;
            }
            return palette[paletteIndex];
        }

        public List<Block> AllBlocks()
        {
            List<Block> blocks = new List<Block>();
            if (isEmpty()) return blocks;

            // TODO change this to run through the palette and get the position from that
            // Allows no blockName == null check
            for (int y = 0; y < 16; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        Vec3Int pos = new Vec3Int(worldPosition.X + x, worldPosition.Y + y, worldPosition.Z + z);
                        string blockName = BlockAt(new Vec3Int(x, y, z));
                        if(blockName == null)
                        {
                            //Debug.WriteLine($"No block at {pos} (section offset {new Vec3Int(x, y, z)}). Section y is {Y}, world pos {worldPosition}");
                            continue;
                        }
                        blocks.Add(new Block(blockName, pos));

                    }
                }
            }
            return blocks;
        }
    }
}
