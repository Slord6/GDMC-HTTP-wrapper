using GDMCHttp;
using GDMCHttp.Commands;
using GDMCHttp.Data.Blocks;
using GDMCHttp.Data.Blocks.Structures;
using GDMCHttp.Data.Position;
using GDMCHttp.Analysis;
using System.Linq;
using GDMC_Debugging;

Connection connection = new Connection();
Func<string, string> announce = (string msg) =>
{
    Console.WriteLine(msg);
    return connection.SendCommandSync(new Say(msg));
};

announce("Client connected");

StructureProcessing structureProcessing = new StructureProcessing(connection);

structureProcessing.ExportStructuresInteractive();
//structureProcessing.PrepareStructuresInteractive();


announce("Client done");

//announce("Restoring...");
//world.Restore();
//announce("Restored");