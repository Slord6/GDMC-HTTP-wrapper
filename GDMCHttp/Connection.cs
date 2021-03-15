using Cyotek.Data.Nbt;
using Cyotek.Data.Nbt.Serialization;
using GDMCHttp.Data;
using GDMCHttp.Data.Chunks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace GDMCHttp
{
    public class Connection
    {
        private static string blockEndpoint = "/blocks";
        private static string chunkEndpoint = "/chunks";
        private static string commandsEndpoint = "/command";
        private static string buildAreaEndpoint = "/buildarea";
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

        private Uri ChunkEndpoint
        {
            get
            {
                return new Uri(root, chunkEndpoint);
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
        private string FormatPositionQuery(Vec3Int position)
        {
            return $"x={position.X}&y={position.Y}&z={position.Z}";
        }

        /// <summary>
        /// Syncronously get data for a single block
        /// </summary>
        /// <param name="position">The position to get data for</param>
        /// <returns>The Block data</returns>
        public Block GetBlockSync(Vec3Int position)
        {
            using(WebClient client = new WebClient())
            {
                string query = "?" + FormatPositionQuery(position);
                string name = client.DownloadString(new Uri(BlockEndpoint.AbsoluteUri + query));
                return new Block(name, position);
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
                string result = webClient.UploadString(address, WebRequestMethods.Http.Put, block.NamespacedName);
                return result != "0";
            }
        }

        public bool SetBlocksSync(Block[] blocks)
        {
            using (WebClient webClient = new WebClient())
            {
                string body = String.Join("\n", FormatOffsets(Offsets(blocks[0].Position, blocks), blocks));

                string address = $"{BlockEndpoint.AbsoluteUri}?{FormatPositionQuery(blocks[0].Position)}";
                string result = webClient.UploadString(address, WebRequestMethods.Http.Put, body);
                return result != "0";
            }
        }

        public string[] SendCommandsSync(string[] commands)
        {
            using (WebClient webClient = new WebClient())
            {
                string body = String.Join("\n", commands);

                string result = webClient.UploadString(CommandEndpoint.AbsoluteUri, WebRequestMethods.Http.Put, body);
                return result.Split('\n');
            }
        }

        public Vec3Int[] GetBuildArea()
        {
            using (WebClient client = new WebClient())
            {
                string body;
                try
                {
                    body = client.DownloadString(new Uri(BuildAreaEndpoint.AbsoluteUri));
                }
                catch(WebException ex)
                {
                    return null;
                }
                Regex numebrRegex = new Regex("([0-9]{1,})");
                int[] numbers = numebrRegex.Matches(body).AsQueryable().Cast<string>().Select(numText => int.Parse(numText)).ToArray();

                return new Vec3Int[]
                {
                    new Vec3Int(numbers[0],numbers[1],numbers[2]),
                    new Vec3Int(numbers[3],numbers[4],numbers[5])
                };
            }
        }

        private Vec3Int[] Offsets(Vec3Int from, Block[] blocks)
        {
            Vec3Int[] offsets = new Vec3Int[blocks.Length];
            for (int i = 0; i < blocks.Length; i++)
            {
                Vec3Int blockPos = blocks[i].Position;
                offsets[i] = new Vec3Int(from.X - blockPos.X, from.Y - blockPos.Y, from.Z - blockPos.Z);
            }
            return offsets;
        }

        private string[] FormatOffsets(Vec3Int[] offsets, Block[] blocks)
        {
            string[] formatted = new string[offsets.Length];
            for (int i = 0; i < offsets.Length; i++)
            {
                Vec3Int offset = offsets[i];
                string blockname = blocks[i].NamespacedName;
                formatted[i] = $"~{offset.X} ~{offset.Y} ~{offset.Z} {blockname}";
            }
            return formatted;
        }

        public Chunk[] GetChunksSync(Vec3Int position, int dx = 1, int dz = 1)
        {
            // Get "chunck coordinate"
            Vec3Int chunkLocation = Chunk.ToChunkCoords(position);

            using (WebClient webClient = new WebClient())
            {
                string query = $"?x={chunkLocation.X}&z={chunkLocation.Z}&dx={dx}&dz={dz}";
                string address = $"{ChunkEndpoint.AbsoluteUri}{query}";
                webClient.Headers.Add(HttpRequestHeader.Accept, "application/octet-stream");
                byte[] data = webClient.DownloadData(address);

                return Chunk.ParseToChunks(data);
            }
        }
    }
}
