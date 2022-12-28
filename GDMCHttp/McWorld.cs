using GDMCHttp.Data;
using GDMCHttp.Data.Blocks;
using GDMCHttp.Data.Blocks.Structures;
using GDMCHttp.Data.Position;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GDMCHttp
{
    public class McWorld
    {
        public Connection Connection { get; set; }
        public Area BuildArea { get; set; }
        private Block[] blockCache;
        private BiomePoint[] biomeCache;
        private List<Block> changedBlocks;

        private Dictionary<Vec3Int, Block> blockPositionDict = null;
        private Dictionary<Vec3Int, Block> BlockPositionDict
        {
            get
            {
                if(blockPositionDict == null)
                {
                    blockPositionDict = new Dictionary<Vec3Int, Block>();
                    for (int i = 0; i < blockCache.Length; i++)
                    {
                        blockPositionDict.Add(blockCache[i].Position, blockCache[i]);
                    }
                }
                return blockPositionDict;
            }
        }

        private Block[] originalState;

        public McWorld(Connection connection)
        {
            this.Connection = connection;
            RefreshCache();
            originalState = GetBlocks();
        }

        /// <summary>
        /// Discard any local changes, query the sever for the current build area and update the cache for the entire area
        /// </summary>
        public void RefreshCache()
        {
            BuildArea = Connection.GetBuildAreaSync();
            blockCache = Connection.GetBlocksSync(BuildArea.MinCorner, BuildArea.Size);
            biomeCache = Connection.GetBiomesSync(BuildArea.MinCorner, BuildArea.Size);
            changedBlocks = new List<Block>();
            blockPositionDict = null;
        }

        /// <summary>
        /// Submit the cache to the server to update the world
        /// </summary>
        public void Flush(bool refreshCache = false)
        {
            if(changedBlocks.Count == 0) return;
            Connection.SetBlocksSync(changedBlocks.ToArray());
            changedBlocks = new List<Block>();

            if (refreshCache)
            {
                RefreshCache();
            }
        }

        /// <summary>
        /// Set a block in the cache. The new block must be at the same position as the old one.
        /// </summary>
        /// <param name="currentBlock">A block currently in the cache to be replaced</param>
        /// <param name="newBlock">A new block to replace it with</param>
        /// <returns>True if the current block was found and replaced, false otherwise</returns>
        public bool ReplaceBlock(Block currentBlock, Block newBlock)
        {
            if (currentBlock.Position != newBlock.Position) return false;
            int index = Array.IndexOf(blockCache, currentBlock);
            if (index == -1) return false;
            blockCache[index] = newBlock;
            changedBlocks.Add(newBlock);
            BlockPositionDict[newBlock.Position] = newBlock;
            return true;
        }

        /// <summary>
        /// Set a block in the cache, replacing the block in that position. The new block must be at the same position as the old one.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="newBlock"></param>
        /// <returns></returns>
        public bool ReplaceBlock(Vec3Int position, Block newBlock)
        {
            for (int i = 0; i < blockCache.Length; i++)
            {
                if(position == blockCache[i].Position)
                {
                    blockCache[i] = newBlock;
                    changedBlocks.Add(blockCache[i]);
                    BlockPositionDict[blockCache[i].Position] = newBlock;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Replace the block with a given name with that of a new name
        /// </summary>
        /// <param name="current">The current name to find</param>
        /// <param name="newType">The new name to apply</param>
        /// <param name="firstOnly">Should this only occur on the first found block</param>
        /// <param name="keepProperties">Should the properties of the existing block be copied to the new one?</param>
        /// <returns>True if any replacement occured, false otherwise</returns>
        public bool ReplaceBlock(BlockName current, BlockName newType, bool firstOnly = true, bool keepProperties = false)
        {
            bool didReplacement = false;
            for (int i = 0; i < blockCache.Length; i++)
            {
                Block block = blockCache[i];
                if(block.Name == current)
                {
                    if (keepProperties)
                    {
                        blockCache[i] = new Block(new BlockProperties(newType, block.Position, block.BlockProperties));
                    }
                    else
                    {
                        blockCache[i] = new Block(newType, block.Position);
                    }
                    changedBlocks.Add(blockCache[i]);
                    BlockPositionDict[blockCache[i].Position] = blockCache[i];
                    didReplacement = true;
                    if (firstOnly) break;
                }
            }

            return didReplacement;
        }

        /// <summary>
        /// Replace all the blocks with new ones of the given type
        /// </summary>
        /// <param name="blocks">Blocks to replace</param>
        /// <param name="newType">New block type</param>
        public void ReplaceBlocks(Block[] blocks, BlockName newType)
        {
            for (int i = 0; i < blocks.Length; i++)
            {
                ReplaceBlock(blocks[i], new Block(newType, blocks[i].Position));
            }
        }

        /// <summary>
        /// Checks if a block in the cache exists that has a matching position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool IsBlockAt(Vec3Int position)
        {
            for (int i = 0; i < blockCache.Length; i++)
            {
                if (blockCache[i].Position == position) return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the block from the cache with matching position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Block GetBlock(Vec3Int position)
        {
            if (BlockPositionDict.ContainsKey(position))
            {
                return BlockPositionDict[position];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the first instance of a block with the same name as given
        /// </summary>
        /// <param name="name">The block type to find</param>
        /// <returns>Found block, or null</returns>
        public Block GetBlock(BlockName name)
        {
            for (int i = 0; i < blockCache.Length; i++)
            {
                if (blockCache[i].Name == name) return blockCache[i];
            }
            return null;
        }

        /// <summary>
        /// Get all blocks in the cache
        /// </summary>
        /// <returns></returns>
        public Block[] GetBlocks()
        {
            return new List<Block>(blockCache).ToArray();
        }

        /// <summary>
        /// Get any blocks in the cache passing the filter
        /// </summary>
        /// <param name="filter">Function that returns true if the given block should pass the filter</param>
        /// <returns>Filtered blocks</returns>
        public Block[] GetBlocks(Func<Block, bool> filter)
        {
            return blockCache.Where(b => filter(b)).ToArray();
        }

        /// <summary>
        /// Get all blocks in the cache of the given type
        /// </summary>
        /// <param name="name">Block type to find</param>
        /// <returns></returns>
        public Block[] GetBlocks(BlockName name)
        {
            List<Block> found = new List<Block>();
            for (int i = 0; i < blockCache.Length; i++)
            {
                if (blockCache[i].Name == name) found.Add(blockCache[i]);
            }
            return found.ToArray();
        }

        public Block[] GetNeighbours(Block block)
        {
            return GetNeighbours(block.Position, Vec3Int.Neighbours);
        }

        public Block[] GetNeighbours(Vec3Int position)
        {
            return GetNeighbours(position, Vec3Int.Neighbours);
        }

        public Block[] GetNeighboursOrthogonal(Block block)
        {
            return GetNeighboursOrthogonal(block.Position);
        }

        public Block[] GetNeighboursOrthogonal(Vec3Int position)
        {
            return GetNeighbours(position, Vec3Int.NeighboursOrthogonal);
        }

        /// <summary>
        /// Get the neighbouring 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private Block[] GetNeighbours(Vec3Int position, Func<Vec3Int, Vec3Int[]> getNeighbourPosFunc)
        {
            List<Block> neighbours = new List<Block>();
            Vec3Int[] neighbourPositions = getNeighbourPosFunc(position);
            for (int i = 0; i < neighbourPositions.Length; i++)
            {
                Vec3Int neighbourPosition = neighbourPositions[i];
                Block neighbourBlock = GetBlock(neighbourPosition);
                if (neighbourBlock == null) continue;
                neighbours.Add(neighbourBlock);
            }
            return neighbours.ToArray();
        }

        /// <summary>
        /// Arrange the cache into an x,y,z array
        /// </summary>
        /// <returns>Cache blocks</returns>
        public Block[,,] DimensionalRepresentation()
        {
            Vec3Int size = BuildArea.Size;
            Vec3Int minCorner = Vec3Int.MergeToMin(BuildArea.MinCorner, BuildArea.MaxCorner);
            Block[,,] dimensional = new Block[size.X, size.Y, size.Z];
            for (int i = 0; i < blockCache.Length; i++)
            {
                Block block = blockCache[i];

                Vec3Int offset = Vec3Int.Sub(block.Position, minCorner);
                dimensional[offset.X, offset.Y, offset.Z] = block;
            }
            return dimensional;
        }

        /// <summary>
        /// Search the local biome cache to find all biomes in the buildarea
        /// </summary>
        /// <returns>The biomes in the build area</returns>
        public Biome[] BiomesInArea()
        {
            List<Biome> biomes = new List<Biome>();
            for (int i = 0; i < biomeCache.Length; i++)
            {
                if (!biomes.Contains(biomeCache[i].Biome)) biomes.Add(biomeCache[i].Biome);
            }
            return biomes.ToArray();
        }

        /// <summary>
        /// Push structures into the world.
        /// WARNING: This does not affect the cache and so it is strongly recommended to refresh
        /// the cache afterwards otherwise subsequent GetBlock calls may be incorrect. As such it is also
        /// recommended to batch structure placements so that cache refreshes are limited
        /// </summary>
        /// <param name="refreshCache">Should the cache be refreshed after sending all the structures?
        /// If false, the cache will be out of sync with the remote</param>
        /// <param name="structures"></param>
        public void PushStructures(Structure[] structures, bool refreshCache = true, bool checkPosition = true)
        {
            for (int i = 0; i < structures.Length; i++)
            {
                if (checkPosition && !BuildArea.Contains(structures[i].Position))
                {
                    Console.WriteLine("Failed to place, out of zone");
                    continue;
                }
                Connection.SetStructureSync(structures[i]);
            }

            if (refreshCache)
            {
                RefreshCache();
            }
        }

        /// <summary>
        /// Export the world area (not the cache) as a structure
        /// </summary>
        /// <returns>A new structure</returns>
        public Structure ToStructure()
        {
            return Connection.GetStructureSync(BuildArea.MinCorner, BuildArea.Size);
        }


        /// <summary>
        /// Restore the remote world to as it was when this instance was created
        /// Pushes the entire original cache and then refreshes
        /// </summary>
        public void Restore(bool refreshCache = true)
        {
            Connection.SetBlocksSync(originalState);
            if(refreshCache) {
            RefreshCache();
                }
        }
    }
}
