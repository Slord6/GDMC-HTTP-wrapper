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

Dictionary<BlockName, int> availableResources = world.AvailableResources();
Biome[] biomes = world.BiomesInArea();

world.Flush();

announce("Client done", connection);
