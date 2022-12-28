using GDMCHttp;
using GDMCHttp.Data.Blocks;
using GDMCHttp.Data.Position;

namespace GDMC_Debugging.Demos
{
    public class PathingDemo : Demo
    {
        private BlockName pathBlock;
        public McWorld? World { private get; set; }

        public PathingDemo(BlockName pathBlock = BlockName.stone_bricks)
        {
            this.pathBlock = pathBlock;
        }

        public override void Run(McWorld world)
        {
            this.World = world;
            Block[] paths = GDMCHttp.Analysis.Pathing.Path.PathJoin(world, IsPathTerminus, false, false);

            world.ReplaceBlocks(paths, pathBlock);
            world.Flush();
        }

        private bool IsDoorway(Block b)
        {
            if (World == null) throw new ArgumentException("No world set");

            // Doorways are two air blocks above a solid block,
            // each with two neighbouring air and solid blocks at the same level

            Vec3Int upOne = new Vec3Int(0, 1, 0);
            Vec3Int downOne = new Vec3Int(0, -1, 0);

            // First check the block below is solid
            Block below = World.GetBlock(b.Position + downOne);
            if (below == null) return false;
            if (!BlockCategories.IsSolidBlock(below.Name)) return false;

            // Then that the 'doorway' is air
            Block feet = b;
            Block head = World.GetBlock(b.Position + upOne);
            if(head == null) return false;
            if (!BlockCategories.IsAirBlock(feet.Name) || !BlockCategories.IsAirBlock(head.Name)) return false;

            // Finally check each have the correct number of solid/air neighbours
            Block[] space = new Block[] { feet, head };
            for (int i = 0; i < space.Length; i++)
            {
                Block current = space[i];

                Vec3Int[] neighbourPositions = Vec3Int.NeighboursOrthogonal(current.Position).Where(pos => pos.Y == current.Position.Y).ToArray();
                List<Block> air = new List<Block>();
                List<Block> solid = new List<Block>();
                for (int j = 0; j < neighbourPositions.Length; j++)
                {
                    Block neighbour = World.GetBlock(neighbourPositions[j]);
                    if (neighbour == null) return false;
                    if (BlockCategories.IsAirBlock(neighbour.Name)) air.Add(neighbour);
                    if (BlockCategories.IsSolidBlock(neighbour.Name)) solid.Add(neighbour);
                }
                if(air.Count != 2 || solid.Count != 2) return false;

                // Check that the air is on oppsite sides of the space
                // ie, not one in front/behind and one to the side
                if (air[0].Position.X != air[1].Position.X && air[0].Position.Z != air[1].Position.Z) return false;
            }
            return true;
        }

        public bool IsPathTerminus(Block b)
        {
            if (World == null) throw new ArgumentException("No world set");
            bool isDoor = b.Name == BlockName.oak_door && b.BlockProperties.Properties[BlockProperty.half] != "upper";
            isDoor = isDoor || IsDoorway(b);
            // Debug stuff here, to be able to know what are blocks below doors
            if (isDoor)
            {
                Block below = World.GetBlock(b.Position + new Vec3Int(0, -1, 0));
                if (below != null)
                {
                    World.ReplaceBlock(below, new Block(BlockName.copper_block, below.Position));
                }
            }
            return isDoor;
        }
    }
}
