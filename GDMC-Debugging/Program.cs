using GDMCHttp;
using GDMCHttp.Commands;
using GDMCHttp.Data;

Func<string, Connection, string> announce = (string msg, Connection connection) =>
{
    return connection.SendCommandSync(new Say(msg));
};

Connection connection = new Connection();

announce("Client connected", connection);

for (int y = 65; y < 70; y++)
{
    for (int x = 0; x < 5; x++)
    {
        BiomePoint b = connection.GetBiomeSync(new Vec3Int(x, y, 0));
        Console.WriteLine(b.ToString());
    }
}

BiomePoint[] biomePoints = connection.GetBiomesSync(new Vec3Int(0, 65, 0), new Vec3Int(5, 5, 1));

announce("Client done", connection);
