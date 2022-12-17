using GDMCHttp;
using GDMCHttp.Commands;
using GDMCHttp.Data;
using GDMCHttp.Pathing;
using System.Linq;

Func<string, Connection, string> announce = (string msg, Connection connection) =>
{
    return connection.SendCommandSync(new Say(msg));
};

Connection connection = new Connection();
announce("Client connected", connection);

McWorld world = new McWorld(connection);
world.RefreshCache();
world.ReplaceBlock(BlockName.diamond_block, BlockName.stone, false);
world.ReplaceBlock(BlockName.dirt_path, BlockName.stone, false);
world.ReplaceBlock(BlockName.gold_block, BlockName.stone, false);
world.ReplaceBlock(BlockName.sandstone, BlockName.stone, false);
world.ReplaceBlock(BlockName.glass, BlockName.stone, false);
world.Flush();


Block b = world.GetBlock(new Vec3Int(-19, -61, 1));
Block[] neighbours = world.GetNeighboursOrthogonal(b);

Block[,] heightmap = world.CalculateHeightMap();

GDMCHttp.Pathing.Path path = new GDMCHttp.Pathing.Path(heightmap[0, 0], heightmap[15, 15], world);

if(path.Route != null)
{
    world.ReplaceBlocks(path.Route, BlockName.glass);
}

world.Flush();

announce("Client done", connection);
