using GDMCHttp;
using GDMCHttp.Commands;
using GDMCHttp.Data.Blocks;
using GDMCHttp.Data.Position;
using GDMCHttp.Pathing;
using System.Linq;

Connection connection = new Connection();
Func<string, string> announce = (string msg) =>
{
    Console.WriteLine(msg);
    return connection.SendCommandSync(new Say(msg));
};

announce("Client connected");

BlockName pathBlock = BlockName.stone_bricks;

announce("Loading world cache");
McWorld world = new McWorld(connection);
announce("Cache loaded, tidying up from last time...");
world.ReplaceBlock(pathBlock, BlockName.stone, false);
world.Flush();
announce("Tidied");


announce("Calculating paths");

System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

sw.Start();
Block[] paths = GDMCHttp.Pathing.Path.PathJoin(world, (Block b) =>
{
    bool isDoor = b.Name == BlockName.oak_door && b.BlockProperties.Properties[BlockProperty.half] != "upper";

    // Debug stuff here, to be able to know what are blocks below doors
    if (isDoor)
    {
        Block below = world.GetBlock(b.Position + new Vec3Int(0, -1, 0));
        if (below != null)
        {
            world.ReplaceBlock(below, new Block(BlockName.copper_block, below.Position));
        }
    }

    return isDoor;
}, false, false);
sw.Stop();

announce($"Paths calculated ({paths.Length} blocks, in {sw.Elapsed}), updating world");
world.ReplaceBlocks(paths, pathBlock);
announce("Flushing");
world.Flush();


announce("Client done");
