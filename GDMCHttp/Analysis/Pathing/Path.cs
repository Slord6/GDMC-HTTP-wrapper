using System.Linq;
using System.Collections.Generic;
using System.Text;
using GDMCHttp.Data.Blocks;
using GDMCHttp.Data.Position;
using System;

namespace GDMCHttp.Analysis.Pathing
{
    public class Path
    {
        private Block[] route;
        public Block[] Route
        {
            get => route;
        }

        /// <summary>
        /// Create a new path between two blocks
        /// </summary>
        /// <param name="start">Starting block</param>
        /// <param name="ends">Valid ending blocks</param>
        /// <param name="world">McWorld to query for additional information</param>
        /// <param name="flatnessPreference">Preference for flatness, equal to how many blocks out-of-the-way to go to avoid/prefer a height change
        /// 0 means height changes have no effect, negative values will use hills and positive values will avoid them
        /// </param>
        public Path(Block start, Block[] ends, McWorld world, int flatnessPreference = 0)
        {
            Node pathEnd = Calculate(start, ends, world, flatnessPreference);
            if (pathEnd == null)
            {
                route = null;
            }
            else
            {
                route = CompletedPathNodeToBlocks(pathEnd);
            }
        }

        /// <summary>
        /// Create a new path between two blocks
        /// </summary>
        /// <param name="start">Starting block</param>
        /// <param name="end">End block</param>
        /// <param name="world">McWorld to query for additional information</param>
        /// <param name="flatnessPreference">Preference for flatness, equal to how many blocks out-of-the-way to go to avoid/prefer a height change
        /// 0 means height changes have no effect, negative values will use hills and positive values will avoid them
        /// </param>
        public Path(Block start, Block end, McWorld world, int flatnessPreference = 0) : this(start, new Block[] { end }, world, flatnessPreference)
        {

        }

        public Block[] CompletedPathNodeToBlocks(Node node)
        {
            List<Block> path = new List<Block>();
            do
            {
                path.Add(node.Block);
                node = node.Parent;
            } while (node != null);
            return path.ToArray();
        }

        private bool IsWalkableBlock(Block block, McWorld world)
        {
            if (block == null) return false;
            bool isSolid = BlockCategories.IsSolidBlock(block.Name);
            if (!isSolid) return false;

            bool isTooTall = BlockCategories.IsTallerThanOneBlock(block.Name);
            if (isTooTall) return false;

            bool walkableAbove = true;
            for (int y = 1; y <= 2; y++)
            {
                Vec3Int offset = new Vec3Int(0, y, 0);
                Block above = world.GetBlock(block.Position + offset);
                if (above == null)
                {
                    walkableAbove = true;
                    break;
                }
                else if(above.Name == BlockName.snow)
                {
                    int layerCount = int.Parse(above.BlockProperties.Properties[BlockProperty.layers]);
                    walkableAbove = layerCount <= 3;
                    if (!walkableAbove) break;
                }
                else if (!BlockCategories.IsAirBlock(above.Name)
                    && !BlockCategories.IsDoorBlock(above.Name)
                    && !BlockCategories.IsGateBlock(above.Name))
                {
                    walkableAbove = false;
                    break;
                }
            }

            return walkableAbove;
        }

        private Node Calculate(Block start, Block[] ends, McWorld world, int flatnessPreference)
        {
            IEnumerable<Vec3Int> endPositions = ends.Select(b => b.Position);
            List<Node> openSet = new List<Node>();
            openSet.Add(new Node(start, flatnessPreference));
            HashSet<Node> visited = new HashSet<Node>();

            while (openSet.Count > 0)
            {
                int lowest = openSet.Min(n => n.TotalScore);
                Node current = openSet.First(n => n.TotalScore == lowest);

                visited.Add(current);
                openSet.Remove(current);

                if (endPositions.Contains(current.Block.Position))
                {
                    return current;
                }

                Block[] neighbours = world.GetNeighboursOrthogonal(current.Block).Where(b => IsWalkableBlock(b, world)).ToArray();

                for (int i = 0; i < neighbours.Length; i++)
                {
                    Node neighbour = new Node(neighbours[i], flatnessPreference);
                    neighbour.Parent = current;
                    if (visited.Contains(neighbour)) continue;

                    int lowestDistanceToEnd = ends.Select(block => neighbour.DistanceTo(block.Position)).Min();
                    neighbour.EstimatedDistanceToEnd = lowestDistanceToEnd;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Insert(0, neighbour);
                    }
                    else
                    {
                        Node existing = openSet.First(n => n.Block.Position == n.Block.Position);
                        if (current.TotalScore + neighbour.EstimatedDistanceToEnd < existing.TotalScore)
                        {
                            existing.Parent = current;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Join a group of blocks by paths
        /// </summary>
        /// <param name="world">The world</param>
        /// <param name="BlockShouldJoin">Function that returns true if the given block should be pathed to others</param>
        /// <param name="oneToAll">If true, the first block is pathed to the others, if false each block is pathed to every other block</param>
        /// <param name="individualPaths">If true an entire path is calculated, if false, previously found paths are treated as end positions
        /// which can be more performant and generate simpler paths</param>
        /// <returns>All unique blocks in the joined paths</returns>
        public static Block[] PathJoin(McWorld world, Func<Block, bool> BlockShouldJoin, bool oneToAll = true, bool individualPaths = false)
        {
            Block[] joiners = FilterBlocks(world.GetBlocks(), BlockShouldJoin);
            List<Block> foundPathBlocks = new List<Block>();
            Vec3Int downOne = new Vec3Int(0, -1, 0);
            for (int i = 0; i < joiners.Length; i++)
            {
                for (int j = 0; j < joiners.Length; j++)
                {
                    if (i == j) continue;
                    Block belowOne = world.GetBlock(joiners[i].Position + downOne);
                    Block belowTwo = world.GetBlock(joiners[j].Position + downOne);

                    Path path;
                    if(individualPaths)
                    {
                        path = new Path(belowOne, belowTwo, world, 1);
                    }
                    else
                    {
                        List<Block> endPoints = new List<Block>(foundPathBlocks);
                        endPoints.Add(belowTwo);

                        path = new Path(belowOne, endPoints.ToArray(), world, 1);
                    }
                    
                    if (path.Route == null)
                    {
                        continue;
                    }
                    else
                    {
                        foundPathBlocks.AddRange(path.Route);
                    }
                }
                if (oneToAll) break;
            }

            Block[] pathBlocks = foundPathBlocks.Distinct().ToArray();
            return pathBlocks;
        }

        /// <summary>
        /// Filter a selection of blocks with the given function
        /// </summary>
        /// <param name="selection">Blocks to filter</param>
        /// <param name="Filter">The filter function, returns true if the block passes the filter</param>
        /// <returns>Filtered blocks</returns>
        public static Block[] FilterBlocks(Block[] selection, Func<Block, bool> Filter)
        {
            List<Block> passed = new List<Block>();
            for (int i = 0; i < selection.Length; i++)
            {
                if (Filter(selection[i])) passed.Add(selection[i]);
            }
            return passed.ToArray();
        }
    }
}
