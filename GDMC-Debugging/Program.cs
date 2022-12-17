using GDMCHttp;
using GDMCHttp.Commands;
using GDMCHttp.Data;

Func<string, Connection, string> announce = (string msg, Connection connection) =>
{
    return connection.SendCommandSync(new Say(msg));
};

Connection connection = new Connection();
announce("Client connected", connection);

McWorld world = new McWorld(connection);
world.RefreshCache();

Block b = world.GetBlock(world.BuildArea.CornerA);
world.ReplaceBlock(BlockName.gold_block, BlockName.grass_block, false);
world.Flush();

announce("Client done", connection);
