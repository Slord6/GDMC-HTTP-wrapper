using GDMCHttp;
using GDMCHttp.Commands;
using GDMCHttp.Data;
using System.Linq;

Func<string, Connection, string> announce = (string msg, Connection connection) =>
{
    return connection.SendCommandSync(new Say(msg));
};

Connection connection = new Connection();
announce("Client connected", connection);

McWorld world = new McWorld(connection);
world.RefreshCache();

Block b = world.GetBlock(world.BuildArea.CornerA);

Block[,] heightmap = world.CalculateHeightMap();
Block[] flatHeightmap = heightmap.Cast<Block>().Where(b => b != null).ToArray();
world.ReplaceBlocks(flatHeightmap, BlockName.diamond_block);

world.Flush();

announce("Client done", connection);
