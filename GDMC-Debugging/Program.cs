using GDMCHttp;
using GDMCHttp.Commands;
using GDMCHttp.Data.Blocks;
using GDMCHttp.Data.Blocks.Structures;
using GDMCHttp.Data.Position;
using GDMCHttp.Analysis;
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

Surveyor surveyor = new Surveyor(world);

announce("Painting world...");
surveyor.PaintHeightLandscape();
world.Flush();
announce("Painted.");

announce("Selecting good plots");
Block[][] plots = surveyor.BuildablePlots();

for (int i = 0; i < plots.Length; i++)
{
    world.ReplaceBlocks(plots[i], BlockName.lapis_block);
}

world.Flush();

announce("Client done");

announce("Restoring...");
world.Restore();
announce("Restored");