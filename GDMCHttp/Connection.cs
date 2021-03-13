using Cyotek.Data.Nbt;
using Cyotek.Data.Nbt.Serialization;
using GDMCHttp.Data;
using GDMCHttp.Data.Chunks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace GDMCHttp
{
    public class Connection
    {
        private static string blockEndpoint = "/blocks";
        private static string chunkEndpoint = "/chunks";
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

        public Chunk GetChunk(Vec3Int position)
        {
            // Get "chunck coordinate"
            int x = position.X / 16;
            int z = position.Z / 16;
            using (WebClient webClient = new WebClient())
            {
                string query = $"?x={x}&z={z}&dx={1}&dz={1}";
                string address = $"{ChunkEndpoint.AbsoluteUri}{query}";
                webClient.Headers.Add(HttpRequestHeader.Accept, "application/octet-stream");
                byte[] data = webClient.DownloadData(address);

                return new Chunk(data, new Vec3Int(x * 16, 0, z * 16));
            }
        }
    }
}
