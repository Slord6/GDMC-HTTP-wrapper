using GDMCHttp;
using GDMCHttp.Commands;
using GDMC_Debugging.Demos;

Connection connection = new Connection();
Func<string, string> announce = (string msg) =>
{
    Console.WriteLine(msg);
    return connection.SendCommandSync(new Say(msg));
};

announce("Client connected");
// Start rain, so in snowy biomes structures will have some snow on them
connection.SendCommandSync(new Weather(WeatherType.Rain));

McWorld world = new McWorld(connection);
new PathingDemo().Run(world);

// Make clear afterwards
connection.SendCommandSync(new Weather(WeatherType.Clear));
announce("Client done");

announce("Restoring...");
world.Restore(false);
announce("Restored");