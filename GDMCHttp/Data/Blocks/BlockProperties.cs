﻿using GDMCHttp.Data.Position;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace GDMCHttp.Data.Blocks
{
    public class BlockProperties
    {
        private BlockName name;
        private Vec3Int position;
        private Dictionary<BlockProperty, string> properties;
        private string blockStatesString;

        public BlockName Name { get => name; }
        public Vec3Int Position { get => position; }
        public string NamespacedName { get => "minecraft:" + name.ToString(); }
        public Dictionary<BlockProperty, string> Properties { get => properties; }
        public string BlockStateString { get => blockStatesString; }
        public BlockState[] BlockStates
        {
            set
            {
                string blockStates = string.Join(",", (object[])value);
                blockStatesString = "{" + blockStates + "}";
            }
        }

        public BlockProperties(BlockName name, Dictionary<BlockProperty, string> properties, string blockStates, Vec3Int position)
        {
            this.name = name;
            this.properties = properties;
            blockStatesString = blockStates;
            this.position = position;
        }

        public BlockProperties(BlockName name, Vec3Int position) : this(name, new Dictionary<BlockProperty, string>(), "", position)
        {
        }

        public BlockProperties(BlockName name, Vec3Int position, BlockProperties otherProperties) : this(name, otherProperties.properties, otherProperties.blockStatesString, position)
        {

        }

        /// <summary>
        /// Construct a BlockProperties from a raw string
        /// </summary>
        /// <param name="rawString">String of the format namespaced:name[property=value,property2=value2]{blockstate: {value}}</param>
        public BlockProperties(string rawString)
        {
            Regex regex = new Regex(@"(-?\d*) (-?\d*) (-?\d*) (minecraft:(\w*))\[(.*)]({.*})");
            Match match = regex.Match(rawString);
            // 0 = whole match
            // 1,2,3 = x,y,z
            string positionString = $"{match.Groups[1].Value} {match.Groups[2].Value} {match.Groups[3].Value}";
            Vec3Int position = Vec3Int.Parse(positionString);
            this.position = position;
            // 4= minecraft:block, 5 = block
            SetName(match.Groups[4].Value);
            // 6 = property=value,property2=value2...
            string propertiesString = match.Groups[6].Value;
            properties = PropertiesFromString(propertiesString);
            // 7 = blockstates
            string blockstates = match.Groups[7].Value;
            blockStatesString = blockstates;
        }

        public void SetProperty(BlockProperty key, string value)
        {
            if (Properties.ContainsKey(key))
            {
                Properties[key] = value;
            }
            else
            {
                Properties.Add(key, value);
            }
        }

        public string GetProperty(BlockProperty key)
        {
            if (Properties.ContainsKey(key))
            {
                return Properties[key];
            }
            return null;
        }

        /// <summary>
        /// Set the name of this block
        /// </summary>
        /// <param name="fullName">Full block name eg. minecraft:oak_planks</param>
        private void SetName(string fullName)
        {
            string baseName = fullName.Split(':')[1];

            if (!Enum.TryParse(baseName, out name))
            {
                name = BlockName.UNKNOWN;
                Debug.WriteLine(fullName + " is unknown ");
            }
        }

        private Dictionary<BlockProperty, string> PropertiesFromString(string propertyString)
        {
            propertyString = propertyString.Replace("[", "").Replace("]", "");
            string[] pairs = propertyString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            Dictionary<BlockProperty, string> parsedPairs = new Dictionary<BlockProperty, string>();
            for (int i = 0; i < pairs.Length; i++)
            {
                string[] splitPair = pairs[i].Split(new char[] { '=' });
                BlockProperty blockProperty;
                if (Enum.TryParse(splitPair[0], out blockProperty))
                {
                    parsedPairs.Add(blockProperty, splitPair[1]);
                }
                else
                {
                    throw new ArgumentException("Invalid block property name - " + splitPair[0]);
                }
            }
            return parsedPairs;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("[");

            int count = 0;
            foreach (KeyValuePair<BlockProperty, string> propertyPair in Properties)
            {
                count++;
                stringBuilder.Append($"{propertyPair.Key}={propertyPair.Value}");
                if (count != Properties.Count) stringBuilder.Append(",");
            }
            stringBuilder.Append("]");
            stringBuilder.Append(BlockStateString);
            return stringBuilder.ToString();
        }
    }
}
