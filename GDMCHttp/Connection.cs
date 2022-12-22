using GDMCHttp.Commands;
using GDMCHttp.Data;
using GDMCHttp.Data.Blocks;
using GDMCHttp.Data.Blocks.Structures;
using GDMCHttp.Data.Position;
using System;
using System.Net;
using System.Text.RegularExpressions;

namespace GDMCHttp
{
    public class Connection
    {
        private static string blockEndpoint = "/blocks";
        private static string biomeEndpoint = "/biomes";
        private static string commandsEndpoint = "/command";
        private static string buildAreaEndpoint = "/buildarea";
        private static string structureEndpoint = "/structure";
        private Uri root;

        /// <summary>
        /// Endpoint for getting and setting blocks as a URI
        /// </summary>
        private Uri BlockEndpoint
        {
            get
            {
                return new Uri(root, blockEndpoint);
            }
        }

        /// <summary>
        /// Endpoint for getting the biome for positions
        /// </summary>
        private Uri BiomeEndpoint
        {
            get
            {
                return new Uri(root, biomeEndpoint);
            }
        }

        private Uri CommandEndpoint
        {
            get
            {
                return new Uri(root, commandsEndpoint);
            }
        }

        private Uri BuildAreaEndpoint
        {
            get
            {
                return new Uri(root, buildAreaEndpoint);
            }
        }

        private Uri StructureEndpoint
        {
            get
            {
                return new Uri(root, structureEndpoint);
            }
        }

        /// <summary>
        /// Create a new connection
        /// </summary>
        /// <param name="host">Hostname, eg. 'localhost'</param>
        /// <param name="port">Port number, eg. </param>
        public Connection(string host = "localhost", int port = 9000)
        {
            root = new Uri($"http://{host}:{port}");
        }

        /// <summary>
        /// Transform a Vector3Int into x,y,z query parameters
        /// </summary>
        /// <param name="position">Position to transform</param>
        /// <returns>Query parmater string</returns>
        private string FormatPositionQuery(Vec3Int position, bool isOffset = false)
        {
            string d = isOffset ? "d" : "";
            return $"{d}x={position.X}&{d}y={position.Y}&{d}z={position.Z}";
        }

        /// <summary>
        /// Syncronously get data for a single block
        /// </summary>
        /// <param name="position">The position to get data for</param>
        /// <returns>The Block data</returns>
        public Block GetBlockSync(Vec3Int position)
        {
            return GetBlocksSync(position, new Vec3Int(1, 1, 1))[0];
        }

        public Block[] GetBlocksSync(Vec3Int position, Vec3Int offset)
        {
            using (WebClient client = new WebClient())
            {
                string query = "?" + FormatPositionQuery(position);
                query += "&includeState=true";
                query += "&includeData=true";
                if(offset != new Vec3Int(1,1,1))
                {
                    query += "&" + FormatPositionQuery(offset, true);
                }
                string blockData = client.DownloadString(new Uri(BlockEndpoint.AbsoluteUri + query));
                string[] blocksData = blockData.Split('\n');
                Block[] blocks = new Block[blocksData.Length];
                if (blockData == "") return blocks;

                for (int i = 0; i < blocks.Length; i++)
                {
                    blocks[i] = new Block(blocksData[i]);
                }

                return blocks;
            }
        }

        /// <summary>
        /// Set a block
        /// </summary>
        /// <param name="block">The block data</param>
        /// <returns>True if the block was changed, false otherwise</returns>
        public bool SetBlockSync(Block block)
        {
            using (WebClient webClient = new WebClient())
            {
                string address = $"{BlockEndpoint.AbsoluteUri}?{FormatPositionQuery(block.Position)}";
                string blockData = block.ToString();
                string result = webClient.UploadString(address, WebRequestMethods.Http.Put, blockData);
                return result != "0";
            }
        }

        /// <summary>
        /// Push the given blocks to the server to be set
        /// </summary>
        /// <param name="blocks"></param>
        public void SetBlocksSync(Block[] blocks)
        {
            using (WebClient webClient = new WebClient())
            {
                string body = String.Join("\n", FormatOffsets(Offsets(blocks[0].Position, blocks), blocks));
                string address = $"{BlockEndpoint.AbsoluteUri}?{FormatPositionQuery(blocks[0].Position)}";
                string result = webClient.UploadString(address, WebRequestMethods.Http.Put, body);
            }
        }

        /// <summary>
        /// Syncronously get the biome for a range of blocks
        /// </summary>
        /// <param name="position">The starting point for the query</param>
        /// <param name="offsetRange">The offset from the start point to get data up to</param>
        /// <returns>The Biome data</returns>
        public BiomePoint[] GetBiomesSync(Vec3Int position, Vec3Int offsetRange)
        {
            using (WebClient client = new WebClient())
            {
                string query = "?" + FormatPositionQuery(position);
                if (offsetRange != new Vec3Int(1, 1, 1))
                {
                    query += "&" + FormatPositionQuery(offsetRange, true);
                }
                string biomeData = client.DownloadString(new Uri(BiomeEndpoint.AbsoluteUri + query));

                string[] biomePointStrings = biomeData.Split('\n');
                BiomePoint[] biomePoints = new BiomePoint[biomePointStrings.Length];
                for (int i = 0; i < biomePointStrings.Length; i++)
                {
                    biomePoints[i] = new BiomePoint(biomePointStrings[i]);
                }
                return biomePoints;
            }
        }

        /// <summary>
        /// Get biome data at a position
        /// </summary>
        /// <param name="position">The position to check</param>
        /// <returns>The BiomePoint for that position</returns>
        public BiomePoint GetBiomeSync(Vec3Int position)
        {
            return GetBiomesSync(position, new Vec3Int(1, 1, 1))[0];
        }

        public Structure GetStructureSync(Vec3Int position, Vec3Int offset, bool entities = false)
        {
            using (WebClient webClient = new WebClient())
            {
                string query = "?" + FormatPositionQuery(position);
                query += "&" + FormatPositionQuery(offset, true);
                if(entities)
                {
                    query += "&entities=true";
                }
                byte[] result = webClient.DownloadData(new Uri(StructureEndpoint.AbsoluteUri + query));
                return new Structure(new Area(position, position + offset), result);
            }
        }

        public void SetStructureSync(Structure structure)
        {
            using (WebClient webClient = new WebClient())
            {
                string query = "?" + FormatPositionQuery(Vec3Int.MergeToMin(structure.Position.CornerA, structure.Position.CornerB));
                if(structure.Entities)
                {
                    query += "&entities=true";
                }
                if (structure.Rotation != Rotation.None)
                {
                    query += $"&rotate={(int)structure.Rotation}";
                }
                if (structure.Pivot != Vec3Int.Zero)
                {
                    Vec3Int pivot = structure.Pivot;
                    query += $"&pivotx={pivot.X}&pivoty={pivot.Y}&pivotz={pivot.Z}";
                }
                if(structure.Mirror != Mirror.None)
                {
                    query += $"&mirror={structure.Mirror.ToString()}";
                }
                webClient.UploadData(new Uri(StructureEndpoint.AbsoluteUri + query), structure.Data);
            }
        }

        /// <summary>
        /// Push the given commands to the server
        /// </summary>
        /// <param name="commands">Commands to send</param>
        /// <returns>Server responses</returns>
        public string[] SendCommandsSync(string[] commands)
        {
            using (WebClient webClient = new WebClient())
            {
                string body = String.Join("\n", commands);

                string result = webClient.UploadString(CommandEndpoint.AbsoluteUri, WebRequestMethods.Http.Put, body);
                return result.Split('\n');
            }
        }

        /// <summary>
        /// Push the given commands to the server
        /// </summary>
        /// <param name="commands">Commands to send</param>
        /// <returns>Server responses</returns>
        public string[] SendCommandsSync(ICommand[] commands)
        {
            string[] commandStrings = new string[commands.Length];
            for (int i = 0; i < commands.Length; i++)
            {
                commandStrings[i] = commands[i].ToCommandString();
            }
            return SendCommandsSync(commandStrings);
        }

        /// <summary>
        /// Push the given command to the server
        /// </summary>
        /// <param name="command">Command to send</param>
        /// <returns>Server response</returns>
        public string SendCommandSync(string command)
        {
            return String.Join("\n", SendCommandsSync(new string[] { command }));
        }

        /// <summary>
        /// Push the given command to the server
        /// </summary>
        /// <param name="command">Command to send</param>
        /// <returns>Server response</returns>
        public string SendCommandSync(ICommand command)
        {
            return String.Join("\n", SendCommandsSync(new ICommand[] { command }));
        }

        /// <summary>
        /// Get the build area set on the server
        /// </summary>
        /// <returns>Array with two positions representing two corners of a cuboid build area</returns>
        public Area GetBuildAreaSync()
        {
            using (WebClient client = new WebClient())
            {
                string body;
                try
                {
                    body = client.DownloadString(new Uri(BuildAreaEndpoint.AbsoluteUri));
                }
                catch(WebException)
                {
                    return null;
                }
                Regex numebrRegex = new Regex("(-?[0-9]{1,})");

                MatchCollection matches = numebrRegex.Matches(body);
                int[] numbers = new int[matches.Count];
                int i = 0;
                foreach (Match match in matches)
                {
                    numbers[i] = int.Parse(match.Value);
                    i++;
                }

                return new Area(
                    new Vec3Int(numbers[0],numbers[1],numbers[2]),
                    new Vec3Int(numbers[3],numbers[4],numbers[5])
                );
            }
        }

        /// <summary>
        /// Translate block positions into offsets from a given point
        /// </summary>
        /// <param name="from">The point to offset from</param>
        /// <param name="blocks">Blocks to calculate offsets for</param>
        /// <returns>The offsets</returns>
        private Vec3Int[] Offsets(Vec3Int from, Block[] blocks)
        {
            Vec3Int[] offsets = new Vec3Int[blocks.Length];
            for (int i = 0; i < blocks.Length; i++)
            {
                Vec3Int blockPos = blocks[i].Position;
                offsets[i] = Vec3Int.Sub(blockPos, from);
            }
            return offsets;
        }

        /// <summary>
        /// Format a set of offsets for pushing to the server
        /// </summary>
        /// <param name="offsets">The offsets</param>
        /// <param name="blocks">Matching blocks to insert block names</param>
        /// <returns>Offset string</returns>
        private string[] FormatOffsets(Vec3Int[] offsets, Block[] blocks)
        {
            string[] formatted = new string[offsets.Length];
            for (int i = 0; i < offsets.Length; i++)
            {
                Vec3Int offset = offsets[i];
                formatted[i] = $"~{offset.X} ~{offset.Y} ~{offset.Z} {blocks[i].ToString()}";
            }
            return formatted;
        }
    }
}
