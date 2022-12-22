using GDMCHttp;
using GDMCHttp.Commands;
using GDMCHttp.Data.Blocks;
using GDMCHttp.Data.Blocks.Structures;
using GDMCHttp.Data.Position;
using GDMCHttp.Pathing;
using System.Linq;

Connection connection = new Connection();
Func<string, string> announce = (string msg) =>
{
    Console.WriteLine(msg);
    return connection.SendCommandSync(new Say(msg));
};

announce("Client connected");

BlockName pathBlock = BlockName.stone_bricks;

announce("Loading world cache");
McWorld world = new McWorld(connection);
announce("Cache loaded");

announce("Loading structure");
Structure hut = Structure.ReadFromXmlFile("./hut.xml");

hut.MoveTo(world.BuildArea.CornerA);

int hutCount = 5;
Structure[] huts = new Structure[hutCount];
Vec3Int offset = new Vec3Int(0, 0, 15);
for (int i = 0; i < hutCount; i++)
{
    Vec3Int hutOffset =  offset * (i + 1);
    huts[i] = new Structure(hut);
    huts[i].Translate(hutOffset);
}

world.PushStructures(huts, true);

world.Flush();


announce("Client done");
