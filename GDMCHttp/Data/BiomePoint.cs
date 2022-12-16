using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace GDMCHttp.Data
{
    /// <summary>
    /// A point with a known biome
    /// </summary>
    public class BiomePoint
    {
        private Biome biome;
        private Vec3Int position;

        public Biome Biome { get => biome; }
        public Vec3Int Position { get => position; }

        public BiomePoint(Biome biome, Vec3Int position)
        {
            this.biome = biome;
            this.position = position;
        }

        public BiomePoint(string rawBiomePoint)
        {
            if(rawBiomePoint == "")
            {
                this.position = new Vec3Int(0, 0, 0);
                this.biome = Biome.UNKNOWN;
                return;
            }

            Regex regex = new Regex(@"(\d*) (\d*) (\d*) minecraft:(\w*)");
            Match match = regex.Match(rawBiomePoint);
            this.position = Vec3Int.Parse(match.Groups[1].Value + " " + match.Groups[2].Value + " " + match.Groups[3].Value);
            this.biome = (Biome)Enum.Parse(typeof(Biome), match.Groups[4].Value);
        }

        public override string ToString()
        {
            return position + " minecraft:" + biome.ToString();
        }

    }
}
