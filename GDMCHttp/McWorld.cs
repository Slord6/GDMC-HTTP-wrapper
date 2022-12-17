using GDMCHttp.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace GDMCHttp
{
    public class McWorld
    {
        public Connection Connection { get; set; }
        public Area BuildArea { get; set; }
        private Block[] blockCache;

        public McWorld(Connection connection)
        {
            this.Connection = connection;
        }

        /// <summary>
        /// Discard any local changes, query the sever for the current build area and update the cache for the entire area
        /// </summary>
        public void RefreshCache()
        {
            BuildArea = Connection.GetBuildAreaSync();
            blockCache = Connection.GetBlocksSync(BuildArea.CornerA, BuildArea.OffsetAToB);
        }

        /// <summary>
        /// Submit the cache to the server to update the world
        /// </summary>
        public void Flush()
        {
            Connection.SetBlocksSync(blockCache);
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
                        blockCache[i] = new Block(new BlockProperties(newType, block.Position, block.Properties));
                    }
                    else
                    {
                        blockCache[i] = new Block(newType, block.Position);
                    }
                    didReplacement = true;
                    if (firstOnly) break;
                }
            }
            return didReplacement;
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
            for (int i = 0; i < blockCache.Length; i++)
            {
                if (blockCache[i].Position == position) return blockCache[i];
            }
            return null;
        }
    }
}
