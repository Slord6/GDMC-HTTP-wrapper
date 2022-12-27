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

Action exportStructures = () =>
{
    while (true)
    {
        announce("Any key in console when world bounds are ready");
        Console.ReadKey();

        announce("Loading world cache");
        McWorld _world = new McWorld(connection);
        announce("Cache loaded");

        Structure structure = _world.ToStructure();

        announce("Waiting on name");
        Console.Write("Structure name: ");
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        string name = Console.ReadLine();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        announce("Exporting...");
        structure.WriteToXmlFile("./" + name + ".xml");
        announce("Exported");

        Console.WriteLine();
    }
};

Action placeAllStructures = () =>
{
    IEnumerable<string> structureFilepaths = Directory.EnumerateFiles("./", "*.xml");
    List<Structure> structures = new List<Structure>(structureFilepaths.Count());
    foreach (string filepath in structureFilepaths)
    {
        structures.Add(Structure.ReadFromXmlFile(filepath));
    }


    McWorld world = new McWorld(connection);
    Vec3Int offset = Vec3Int.Zero;
    Vec3Int movement = new Vec3Int(15, 0, 0);
    Vec3Int root = Vec3Int.MergeToMin(world.BuildArea.CornerA, world.BuildArea.CornerB);
    int height = -61;
    root.Y = height;
    foreach (Structure structure in structures)
    {
        Vec3Int position = root + offset;
        structure.MoveTo(position);
        offset += movement;
    }

    world.PushStructures(structures.ToArray());
};

announce("Client connected");





announce("Client done");

//announce("Restoring...");
//world.Restore();
//announce("Restored");