using System;
using System.Collections.Generic;
using System.Text;

namespace GDMCHttp.Data.Chunks
{
    public static class BitArray
    {
        /// <summary>
        /// Obtain a list of BlockState indices from the BlockState long array.
        /// </summary>
        /// <param name="longArray">The blockstate array</param>
        /// <param name="bitsPerBlock">The number of bits in each compressed block</param>
        /// <returns>Array of palette indicied in a 16x16x16 array</returns>
        public static int[,,] ParseBlockStates(long[] longArray, int bitsPerBlock)
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
            int entriesPerLong = EntriesPerLong(bitsPerBlock);
            int calculatedLength = CalculatedLength(arraySize, entriesPerLong);

            if (longArray.Length != calculatedLength)
            {
                throw new ArgumentException($"Invalid length given for storage, got {longArray.Length} but expected {calculatedLength}");
            }

            int[,,] indicies = new int[16, 16, 16];
            for (int y = 0; y < 16; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    for (int x = 0; x < 16; x++)
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

        private static int EntriesPerLong(int bitsPerBlock)
        {
            return (int)Math.Floor(64 / (double)bitsPerBlock);
        }

        private static int CalculatedLength(int arraySize, int entriesPerLong)
        {
            return (int)Math.Floor((double)(arraySize + entriesPerLong - 1) / entriesPerLong);
        }

        public static int[,] ParseHeightMap(long[] longArray)
        {
            int bitsPerBlock = 9;
            int arraySize = 16 * 16;
            int maxEntryValue = (1 << bitsPerBlock) - 1;
            int entriesPerLong = EntriesPerLong(bitsPerBlock);
            int calculatedLength = CalculatedLength(arraySize, entriesPerLong);

            if (longArray.Length != calculatedLength)
            {
                throw new ArgumentException($"Invalid length given for storage, got {longArray.Length} but expected {calculatedLength}");
            }

            int[,] indicies = new int[16, 16];
            for (int z = 0; z < 16; z++)
            {
                for (int x = 0; x < 16; x++)
                {
                    int index1D = z * 16 + x;
                    int indexOfLong = (int)(index1D / entriesPerLong);
                    long currentLong = longArray[indexOfLong];
                    int k = (index1D - indexOfLong * entriesPerLong) * bitsPerBlock;
                    indicies[x, z] = (int)(currentLong >> k & maxEntryValue);
                }
            }

            return indicies;
        }
    }
}
