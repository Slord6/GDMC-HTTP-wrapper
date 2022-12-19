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

BlockName pathBlock = BlockName.glass;

McWorld world = new McWorld(connection);
world.RefreshCache();
world.ReplaceBlock(pathBlock, BlockName.stone, false);
world.Flush();


Block[,] heightmap = world.CalculateHeightMap();
Block start = heightmap[0, 0];
Block end = heightmap[20,25];
GDMCHttp.Pathing.Path path = new GDMCHttp.Pathing.Path(start, end, world, -4);
if (path.Route == null)
{
    Console.WriteLine("No path");
    world.ReplaceBlock(start, new Block(pathBlock, start.Position));
    world.ReplaceBlock(end, new Block(pathBlock, end.Position));
    world.Flush();
}
else
{
    world.ReplaceBlocks(path.Route, pathBlock);
    world.Flush();
}


announce("Client done", connection);
