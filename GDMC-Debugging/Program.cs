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
        Block b = connection.GetBlockSync(new Vec3Int(x, y, 0));
        Console.WriteLine(b.ToString());
    }
}

announce("Client done", connection);
