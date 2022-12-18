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


Block existing = world.GetBlock(BlockName.oak_wall_sign);
Sign sign = new Sign(BlockName.oak_wall_sign, new string[] { "Four", "three", "two", "one!" }, existing.Position);
sign.Facing = BlockProperty.north;

Console.WriteLine(sign.ToString());
world.ReplaceBlock(existing, sign);

world.Flush();

announce("Client done", connection);
