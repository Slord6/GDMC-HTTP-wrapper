# GDMC-HTTP-wrapper
 C# library to interface with the [http server](https://github.com/Niels-NTG/gdmc_http_interface) for the [GDMC](http://gendesignmc.engineering.nyu.edu/) competition

## Library State

The library is simple, functional and usable but does little error checking. It is probably 'good enough' for most uses (but PRs welcome when it isn't)!

The library aims to offer an MVP-level of interaction with the API - enough to get started without having to write an HTTP wrapper and make a best effort at some simple abstractions to make using the resulting data straightforward.

In the future the library may offer some built in utility functions to replace the missing data available from the `/chunks` endpoint, i.e., heightmaps.

## GDMC HTTP Server Supported Versions

- [v0.6.2](https://github.com/Niels-NTG/gdmc_http_interface/releases/tag/v0.6.2)

## Limitations

For simplicity this client no longer support the `/chunks` endpoint.

This client does not support the `dimension` query parameter on any requests - yet.

## Usage

All methods are synchronous.

## Release versioning

Releases are now versioned to match the HTTP mod. eg, HTTP Mod version is `v0.6.2`, then first release targeting that version is `v0.6.2.0`, then `v0.6.2.1` etc

### Examples
``` C#
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

BiomePoint[] biomePoints = connection.GetBiomesSync(new Vec3Int(0, 65, 0), new Vec3Int(5, 5, 1));

announce("Client done", connection);

```