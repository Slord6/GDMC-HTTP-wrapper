using GDMCHttp;
using GDMCHttp.Commands;
using GDMCHttp.Data.Blocks;
using GDMCHttp.Data.Position;
using GDMCHttp.Pathing;
using System.Linq;

Func<string, Connection, string> announce = (string msg, Connection connection) =>
{
    return connection.SendCommandSync(new Say(msg));
};

Connection connection = new Connection();
announce("Client connected", connection);

BlockName pathBlock = BlockName.glass;

McWorld world = new McWorld(connection);
world.RefreshCache();
world.ReplaceBlock(pathBlock, BlockName.stone, false);
world.Flush();


Block[,] heightmap = world.CalculateHeightMap();
Block[] doors = world.GetBlocks(BlockName.oak_door);

for (int i = 0; i < doors.Length; i++)
{
    for (int j = 0; j < doors.Length; j++)
    {
        if (i == j) continue;
        if (doors[i].BlockProperties.Properties[BlockProperty.half] == "upper"
            || doors[j].BlockProperties.Properties[BlockProperty.half] == "upper") continue;
        Vec3Int downOne = new Vec3Int(0, -1, 0);
        Block belowDoorOne = world.GetBlock(doors[i].Position + downOne);
        Block belowDoorTwo = world.GetBlock(doors[j].Position + downOne);

        GDMCHttp.Pathing.Path path = new GDMCHttp.Pathing.Path(belowDoorOne, belowDoorTwo, world, 1);
        if (path.Route == null)
        {
            Console.WriteLine("No path (" + doors[i].Position + "->" + doors[j].Position + ")");
            world.Flush();
        }
        else
        {
            Console.WriteLine("Path!(" + doors[i].Position + "->" + doors[j].Position + ")");
            world.ReplaceBlocks(path.Route, pathBlock);
            world.Flush();
        }
        break;
    }
}



announce("Client done", connection);
