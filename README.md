# GDMC-HTTP-wrapper
 C# library to interface with the [http server](https://github.com/nilsgawlik/gdmc_http_interface) for the [GDMC](http://gendesignmc.engineering.nyu.edu/) competition

## Library State

The library is simple, functional and usable but does little error checking. It is probably 'good enough' for most uses (but PRs welcome when it isn't)!

## Usage

The API is covers all endpoints supported by the server as of 16/30/20. All methods are synchronous.

### Examples
``` C#
Connection connection = new Connection();
Vec3Int point = new Vec3Int(20, 64, 15);
// Get 3x3 chunk area starting from the chunk containing 20, 64, 15
Chunk[] chunks = connection.GetChunksSync(point, 3, 3);

// Get an individual block
Block block = connection.GetBlockSync(point);

// Send commands to the server
string[] returns = connection.SendCommandsSync(new string[]
        {
            "say hello!",
            "data get entity @p Pos"
        });

// Set a block on the server
Block goldBlock = new Block(new Vec3Int(0, 70, 0), BlockName.gold_block);
bool blockChanged = SetBlockSync(Block block)

// Create some blocks and then push them to the server
List<Block> blocks = new List<Block>(30);
for (int x = 0; x < 10; x++)
{
    for (int z = 0; z < 3; z++)
    {
        blocks.Add(new Block(new Vec3Int(x, 70, z), BlockName.gold_block));
    }
}
connection.SetBlocksSync(blocks.ToArray());

// Get build area set on the server
Vec3Int[] buildArea = connection.GetBuildArea();
```