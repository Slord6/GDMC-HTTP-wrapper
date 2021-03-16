using Cyotek.Data.Nbt;
using System;
using System.Collections.Generic;
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

            paletteIndecies = ReadBlockStates(blockStates);
        }

        private void ParseSkyLight(TagByteArray rawSkyLight)
        {
            skyLight = rawSkyLight.Value;
        }

        public bool isEmpty()
        {
            return palette == null || blockStates == null;
        }

        /**
	     * Obtain a list of BlockState indices from the BlockState long array.
	     * @param longArray Long array to parse.
	     * @return Indices of BlockStates in the palette.
         * SOURCE: https://github.com/Sam54123/Scaffold/blob/3972661986f84737876567c9aa4c419b2b0fcf4a/scaffold-nbt/src/main/java/org/scaffoldeditor/nbt/io/ChunkParser.java#L316
         * MIT License
            Copyright (c) 2019 Sam Bradley

            Permission is hereby granted, free of charge, to any person obtaining a copy
            of this software and associated documentation files (the "Software"), to deal
            in the Software without restriction, including without limitation the rights
            to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
            copies of the Software, and to permit persons to whom the Software is
            furnished to do so, subject to the following conditions:

            The above copyright notice and this permission notice shall be included in all
            copies or substantial portions of the Software.

            THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
            IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
            FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
            AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
            LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
            OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
            SOFTWARE.
	     */
        private int[,,] ReadBlockStates(long[] longArray)
        {

            /*
             * The size of an index in bits.
             * One section always stores 16*16*16 = 4096 blocks,
             * therefore the amount of bits per block can be calculated like that: 
             * size of BlockStates-Tag * 64 / 4096 (64 = bit of a long value),
             * which simplifies to longArrayLength/64. 
             */
            int indexSize = Math.Max(4, longArray.Length / 64);
            long maxEntryValue = (1L << indexSize) - 1;

            // Convert into int array.
            int[,,] indices = new int[16, 16, 16];

            for (int y = 0; y < 16; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        int arrayIndex = y << 8 | z << 4 | x;
                        int bitIndex = arrayIndex * indexSize;
                        int startIndex = bitIndex / 64;
                        int endIndex = ((arrayIndex + 1) * indexSize - 1) / 64;
                        int startBitSubIndex = bitIndex % 64;

                        int val;

                        if (startIndex == endIndex)
                        {
                            val = (int)((ulong)longArray[startIndex] >> startBitSubIndex & (ulong)maxEntryValue);
                        }
                        else
                        {
                            int endBitSubIndex = 64 - startBitSubIndex;
                            val = (int)(((ulong)longArray[startIndex] >> startBitSubIndex | (ulong)longArray[endIndex] << endBitSubIndex) & (ulong)maxEntryValue);
                        }

                        indices[y, z, x] = val;
                    }
                }
            }

            return indices;
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
            for (int y = 0; y < 16; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        Vec3Int pos = new Vec3Int(worldPosition.X + x, worldPosition.Y + y, worldPosition.Z + z);
                        blocks.Add(new Block(BlockAt(new Vec3Int(x, y, z)), pos));

                    }
                }
            }
            return blocks;
        }
    }
}
