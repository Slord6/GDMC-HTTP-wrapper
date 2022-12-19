# GDMC-HTTP-wrapper
 C# library to interface with the [http server](https://github.com/Niels-NTG/gdmc_http_interface) for the [GDMC](http://gendesignmc.engineering.nyu.edu/) competition

## Library State

The library is simple, functional and usable but does little error checking. It is probably 'good enough' for most uses (but PRs welcome when it isn't)!

The library aims to offer an MVP-level of interaction with the API - enough to get started without having to write an HTTP wrapper and make a best effort at some simple abstractions to make using the resulting data straightforward.

There is some additional functionality available throught the `McWorld` class which gives a local block cache and simple heightmap calculator.

## GDMC HTTP Server Supported Versions

Last version tested against:
- [v0.6.4](https://github.com/Niels-NTG/gdmc_http_interface/releases/tag/v0.6.4)

The client may work with newer versions, as long as there have been no breaking changes,

## Limitations

For simplicity this client no longer support the `/chunks` endpoint.

This client does not support the `dimension` query parameter on any requests - yet.

## Release versioning

Releases are now versioned to match the HTTP mod. eg, HTTP Mod version is `v0.6.2`, then first release targeting that version is `v0.6.2.0`, then `v0.6.2.1` etc

## Usage

All methods are synchronous.

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