using GDMCHttp.Data.Blocks;
using GDMCHttp.Data.Position;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System;

namespace GDMCHttp.Analysis
{
    public class Surveyor
    {
        McWorld world;

        /// <summary>
        /// Create a new Surveyor for analysing the environment
        /// </summary>
        /// <param name="world"></param>
        public Surveyor(McWorld world)
        {
            this.world = world;
        }

        /// <summary>
        /// Calculate the heightmap for the cached area
        /// </summary>
        /// <param name="waterIsGround">Should water blocks be counted as ground</param>
        /// <returns>The 2D block array of the heighest solid blocks in the cache area, (x,z).
        /// Null values mean there were no solid blocks in that column in the cache
        /// </returns>
        public Block[,] CalculateHeightMap(Func<BlockName, bool> TreatAsAir = null, bool waterIsGround = false)
        {
            Block[,,] dimensional = world.DimensionalRepresentation();
            Vec3Int size = world.BuildArea.Size;
            Block[,] heightmap = new Block[size.X, size.Z];
            for (int x = 0; x < size.X; x++)
            {
                for (int z = 0; z < size.Z; z++)
                {
                    Block heighest = null;
                    for (int y = size.Y - 1; y >= 0; y--)
                    {
                        Block current = dimensional[x, y, z];
                        if (!BlockCategories.IsSolidBlock(current.Name) || (TreatAsAir != null && TreatAsAir(current.Name))) continue;
                        heighest = current;
                        break;
                    }
                    heightmap[x, z] = heighest;
                }
            }
            return heightmap;
        }

        /// <summary>
        /// Calculate the average height of each block in the world heightmap from its neighbours
        /// </summary>
        /// <returns>Average height of each block</returns>
        public double[,] HeightmapFlatness()
        {
            Block[,] heightmap = HeightmapNoPlants();

            Vec3Int areaSize = world.BuildArea.Size;
            double[,] averageHeights = new double[areaSize.X, areaSize.Z];

            for (int x = 0; x < areaSize.X; x++)
            {
                for (int z = 0; z < areaSize.Z; z++)
                {
                    if (heightmap[x,z] == null)
                    {
                        averageHeights[x, z] = 0;
                        continue;
                    }

                    IEnumerable<Block> neighbours = LateralNeighbours(heightmap, x, z).Where(n => n != null);
                    double totalHeight = neighbours.Select(block => block.Position.Y).Sum();
                    double localAverage = totalHeight / neighbours.Count();
                    averageHeights[x, z] = localAverage - heightmap[x,z].Position.Y;
                }
            }
            return averageHeights;
        }

        /// <summary>
        /// Get all the blocks that have a low average height
        /// </summary>
        /// <returns>Flat area blocks</returns>
        public Block[] FlatAreaBlocks()
        {
            List<Block> flatAreaBlocks = new List<Block>();
            Vec3Int areaSize = world.BuildArea.Size;
            double[,] heights = HeightmapFlatness();
            Block[,] heightmap = HeightmapNoPlants();

            for (int x = 0; x < areaSize.X; x++)
            {
                for (int z = 0; z < areaSize.Z; z++)
                {
                    if (heights[x,z] >= 0 && heights[x,z] < 0.2 && heightmap[x,z] != null)
                    {
                        flatAreaBlocks.Add(heightmap[x, z]);
                    }
                }
            }
            return flatAreaBlocks.ToArray();
        }

        /// <summary>
        /// Get all the flat area blocks and then group them into contiguous areas
        /// </summary>
        /// <returns>Plots that might be good candidates for building</returns>
        public Block[][] BuildablePlots()
        {
            return ContiguousBlocks(FlatAreaBlocks().ToList());
        }

        /// <summary>
        /// Get all caves in the area
        /// </summary>
        /// <returns>Array of caves containing all the air in each cave</returns>
        public Block[][] Caves()
        {
            List<Block> caveBlocks = new List<Block>();
            Block[] caveAir = world.GetBlocks(BlockName.cave_air);
            caveBlocks.AddRange(caveAir);
            Block[,] heightmap = HeightmapNoPlants();
            Vec3Int worldMinCorner = Vec3Int.MergeToMin(world.BuildArea.CornerA, world.BuildArea.CornerB);
            Block[] enclosedAir = world.GetBlocks((Block b) =>
            {
                // Only air blocks can be a cave
                if (!BlockCategories.IsAirBlock(b.Name)) return false;
                Vec3Int buildAreaOffset = b.Position - worldMinCorner;
                Block heightmapBlock = heightmap[buildAreaOffset.X, buildAreaOffset.Z];
                if (heightmapBlock == null) return false;
                // If this is an air block below the heightmap height then must be a cave
                return heightmapBlock.Position.Y > b.Position.Y;
            });
            caveBlocks.AddRange(enclosedAir);

            return ContiguousBlocks(caveBlocks.Distinct().ToList());
        }

        /// <summary>
        /// Given a selection of blocks, group them spacially
        /// </summary>
        /// <param name="selection">The blocks to group</param>
        /// <returns>The groups of groups of touching blocks</returns>
        public Block[][] ContiguousBlocks(List<Block> selection)
        {
            List<List<Block>> visited = new List<List<Block>>();

            while (selection.Count > 0)
            {
                // Get next unvisited block
                Block current = selection.First();

                List<Block> group = new List<Block>();
                List<Block> neighbours = new List<Block>();
                neighbours.Add(current);
                do
                {
                    // Get next neighbour (of neighbour of neighbour...)
                    Block neighbour = neighbours.FirstOrDefault();
                    // ~Pop from neighbours list and flatBlocks so we don't revisit
                    neighbours.Remove(neighbour);
                    selection.Remove(neighbour);
                    
                    if (neighbour == null) continue;

                    // Add this neighbour to the current group
                    if (!group.Contains(neighbour))
                    {
                        group.Add(neighbour);
                    }
                    // Add any of their neighbours that we still want to visit
                    // and filter unwanted blocks as a byproduct
                    IEnumerable<Block> surrounding = world.GetNeighbours(neighbour).Where(n => n != null);
                    IEnumerable<Block> toVisit = surrounding
                        .Where(n => selection.Contains(n) && !group.Contains(n) && !neighbours.Contains(n));
                    neighbours.AddRange(toVisit);
                } while (neighbours.Count > 0);

                // We exhausted that plot, so store and start over
                visited.Add(group);
            }

            return visited.Select(x => x.ToArray()).ToArray();
        }

        /// <summary>
        /// Get the world heightmap, treating foliage as non-surface
        /// </summary>
        /// <returns></returns>
        public Block[,] HeightmapNoPlants()
        {
            return CalculateHeightMap((BlockName name) =>
            {
                return BlockCategories.PlantBlocks.Contains(name);
            });
        }

        /// <summary>
        /// Visualisation function that paints the world heighmap based on the slope near each block
        /// </summary>
        public void PaintHeightLandscape()
        {
            Block[,] heightmap = HeightmapNoPlants();
            double[,] heights = HeightmapFlatness();
            Vec3Int areaSize = world.BuildArea.Size;
            for (int x = 0; x < areaSize.X; x++)
            {
                for (int z = 0; z < areaSize.Z; z++)
                {
                    if (heightmap[x, z] == null) continue;

                    BlockName newBlockType;
                    double avrHeight = heights[x, z];
                    if (avrHeight > 1)
                    {
                        newBlockType = BlockName.emerald_block;
                    }
                    else if (avrHeight > 0.5)
                    {
                        newBlockType = BlockName.diamond_block;
                    }
                    else if (avrHeight > 0.12)
                    {
                        newBlockType = BlockName.iron_block;
                    }
                    else if (avrHeight > 0)
                    {
                        newBlockType = BlockName.bone_block;
                    }
                    else if (avrHeight == 0)
                    {
                        newBlockType = BlockName.glass;
                    }
                    else if (avrHeight > -0.5)
                    {
                        newBlockType = BlockName.copper_block;
                    }
                    else if (avrHeight > -1.01)
                    {
                        newBlockType = BlockName.coal_block;
                    }
                    else
                    {
                        newBlockType = BlockName.clay;
                    }
                    world.ReplaceBlock(heightmap[x, z], new Block(newBlockType, heightmap[x, z].Position));
                }
            }
        }

        private Block[] LateralNeighbours(Block[,] heightmap, int x, int z)
        {
            List<Block> neighbours = new List<Block>();
            for (int currX = x - 1; currX <= x + 1; currX++)
            {
                for(int currZ = z - 1; currZ <= z + 1; currZ++)
                {
                    if (!IsInBounds(currX, currZ, heightmap)) continue;
                    neighbours.Add(heightmap[currX, currZ]);
                }
            }
            return neighbours.ToArray();
        }

        /// <summary>
        /// Is the x,z position within the bounds of the array
        /// </summary>
        /// <param name="x">0th dimension value</param>
        /// <param name="z">1st dimension value</param>
        /// <param name="arr">The array to check</param>
        /// <returns>True if the x,z pair fall within the bounds of the array</returns>
        private bool IsInBounds(int x, int z, object[,] arr) {
            if(x < 0 || z < 0) return false;
            if (x >= arr.GetLength(0) || z >= arr.GetLength(1)) return false;

            return true;
        }

        /// <summary>
        /// Check if a block is underwater
        /// </summary>
        /// <param name="block">Block to check</param>
        /// <returns>True if the block immediately above is a water block, false otherwise</returns>
        public bool IsSubmerged(Block block)
        {
            Block above = world.GetBlock(block.Position + new Vec3Int(0, 1, 0));
            if (above == null) return false;
            return above.Name == BlockName.water;
        }
    }
}
