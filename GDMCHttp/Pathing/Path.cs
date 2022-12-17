using GDMCHttp.Data;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace GDMCHttp.Pathing
{
    public class Path
    {
        private Block[] route;
        public Block[] Route
        {
            get => route;
        }

        public Path(Block start, Block end, McWorld world)
        {
            Node pathEnd = Calculate(start, end, world);
            if(pathEnd == null)
            {
                route = null;
            }
            else
            {
                route = CompletedPathNodeToBlocks(pathEnd);
            }
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
            bool isGround = BlockCategories.IsSolidBlock(block.Name);
            if (!isGround) return false;


            bool airAbove = true;
            for (int y = 1; y <= 2; y++)
            {
                Vec3Int offset = new Vec3Int(0, y, 0);
                Block above = world.GetBlock(block.Position + offset);
                if (above == null)
                {
                    airAbove = true;
                    break;
                } else if (!BlockCategories.IsAirBlock(above.Name))
                {
                    airAbove = false;
                    break;
                }
            }

            return airAbove;
        }

        public Node Calculate(Block start, Block end, McWorld world)
        {
            List<Node> openSet = new List<Node>();
            openSet.Add(new Node(start));
            HashSet<Node> visited = new HashSet<Node>();

            while (openSet.Count > 0)
            {
                int lowest = openSet.Min(n => n.TotalScore);
                Node current = openSet.First(n => n.TotalScore == lowest);
                
                visited.Add(current);
                openSet.Remove(current);

                if (current.Block.Position == end.Position)
                {
                    return current;
                }

                Block[] neighbours = world.GetNeighboursOrthogonal(current.Block).Where(b => IsWalkableBlock(b, world)).ToArray();

                for (int i = 0; i < neighbours.Length; i++)
                {
                    Node neighbour = new Node(neighbours[i]);
                    neighbour.Parent = current;
                    if (visited.Contains(neighbour)) continue;
                    neighbour.EstimatedDistanceToEnd = neighbour.DistanceTo(end.Position);

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
                //world.ReplaceBlocks(visited.Select(n => n.Block).ToArray(), BlockName.gold_block);
                //world.ReplaceBlocks(openSet.Select(n => n.Block).ToArray(), BlockName.diamond_block);
                //world.Flush();
                System.Console.Write(".");
            }
            return null;
        }
    }
}
