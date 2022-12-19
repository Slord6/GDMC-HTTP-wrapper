using GDMCHttp;
using GDMCHttp.Commands;
using GDMCHttp.Data.Blocks;
using GDMCHttp.Data.Position;
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


Block[] paths = GDMCHttp.Pathing.Path.PathJoin(world, (Block b) =>
{
    return b.Name == BlockName.oak_door && b.BlockProperties.Properties[BlockProperty.half] != "upper";
}, false);

world.ReplaceBlocks(paths, BlockName.glass);

world.Flush();


announce("Client done", connection);
